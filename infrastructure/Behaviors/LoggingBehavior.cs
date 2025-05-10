using System;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using CMPNatural.Core.Logger;
using Microsoft.AspNetCore.Http;
using CMPNatural.Core.Enums;
using ScoutDirect.Core.Authentication;

namespace CMPNatural.Application.Logger
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<TRequest> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICustomDbLogger _dbLogger;

        public LoggingBehavior(
            ILogger<TRequest> logger,
            IHttpContextAccessor httpContextAccessor, ICustomDbLogger dbLogger)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _dbLogger = dbLogger;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            lock(this){
                var requestName = typeof(TRequest).Name;
                var stopwatch = Stopwatch.StartNew();

                var user = _httpContextAccessor.HttpContext?.User;
                var fullName = user?.FindFirst("FullName")?.Value ?? "Unknown";
                var isAdmin = false;
                Guid PersonId;
                bool.TryParse(user?.FindFirst("isAdmin")?.Value, out isAdmin);
                Guid.TryParse(user?.FindFirst("PersonId")?.Value, out PersonId);

                //_logger.LogInformation("Handling {RequestName}: {@Request}", requestName, request);


                if (fullName != "Unknown")
                {
                    _dbLogger.LogAsync(PersonId, fullName, isAdmin ? LogTypeEnum.Admin : LogTypeEnum.Client, $"{requestName}");
                }
            }
            var response = await next();
            return response;

            //try
            //{
            //    var response = await next();
            //    stopwatch.Stop();

            //    _logger.LogInformation("Handled {RequestName} in {Elapsed}ms", requestName, stopwatch.ElapsedMilliseconds);
            //    return response;
            //}
            //catch (Exception ex)
            //{
            //    stopwatch.Stop();
            //    _logger.LogError(ex, "Error handling {RequestName} after {Elapsed}ms", requestName, stopwatch.ElapsedMilliseconds);
            //    throw;
            //}
        }
    }
}
