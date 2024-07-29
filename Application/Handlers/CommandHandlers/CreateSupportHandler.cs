
using Bazaro.Application.Commands;
using Bazaro.Application.Responses.Base;
using Bazaro.Core.Entities;
using Bazaro.Core.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Bazaro.Application.Handlers.CommandHandlers
{
    public class CreateSupportHandler : IRequestHandler<CreateSupportCommand, CommandResponse>
    {
        private readonly ISupportRepository _supportRepository;

        public CreateSupportHandler(ISupportRepository supportRepository)
        {
            _supportRepository = supportRepository;
        }

        public async Task<CommandResponse> Handle(CreateSupportCommand request, CancellationToken cancellationToken)
        {
            var support = new Support()
            {
                Title = request.Description,
                SupportCategoryId = request.SupportCategoryId,
                IsActive = true,
                CreatedAt = System.DateTime.Now,
                PersonId = (long)request.PersonId
            };

            await _supportRepository.AddAsync(support);
            return new Success() { Data = support };
        }
    }

}
