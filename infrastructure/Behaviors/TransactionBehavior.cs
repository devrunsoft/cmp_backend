using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using infrastructure.Data;
using ScoutDirect.Application.Responses;
using CMPNatural.Application.Responses;

public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ScoutDBContext _dbContext;
    private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;

    public TransactionBehavior(ScoutDBContext dbContext, ILogger<TransactionBehavior<TRequest, TResponse>> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    bool IsDerivedFromCommandResponse(object obj)
    {
        var baseType = obj?.GetType().BaseType;
        return baseType != null &&
               baseType.IsGenericType &&
               baseType.GetGenericTypeDefinition() == typeof(CommandResponse<>);
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var strategy = _dbContext.Database.CreateExecutionStrategy();
        return await strategy.ExecuteAsync(async () =>
        {
            if (_dbContext.Database.CurrentTransaction != null)
            {
                return await next();
            }
            await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var response = await next();
                if (IsDerivedFromCommandResponse(response))
                {
                dynamic dynResponse = response;
                    if (dynResponse.Success == false)
                    {
                        await transaction.RollbackAsync(cancellationToken);
                        return response;
                    }
                }
                //await _dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Transaction failed for {typeof(TRequest).Name}");
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        });
    }
}
