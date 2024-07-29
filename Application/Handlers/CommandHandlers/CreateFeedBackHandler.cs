using Bazaro.Application.Commands;
using Bazaro.Application.Responses.Base;
using Bazaro.Core.Entities;
using Bazaro.Core.Repositories;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bazaro.Application.Handlers.CommandHandlers
{
    public class CreateFeedBackHandler : IRequestHandler<CreateFeedBackCommand, CommandResponse>
    {
        private readonly IFeedBackRepository _feedBackRepository;
        private readonly IBenchmarkRepository _benchMarkRepository;
        //private readonly IFeedbackBenchMarkRepository _feedbackBenchMarkRepository;

        public CreateFeedBackHandler(IFeedBackRepository feedBackRepository,
            IBenchmarkRepository benchMarkRepository
            //, IFeedbackBenchMarkRepository feedbackBenchMarkRepository
            )
        {
            _feedBackRepository = feedBackRepository;
            _benchMarkRepository = benchMarkRepository;
            //_feedbackBenchMarkRepository = feedbackBenchMarkRepository;
        }

        public async Task<CommandResponse> Handle(CreateFeedBackCommand request, CancellationToken cancellationToken)
        {
            var benchmarks = await _benchMarkRepository.GetAsync(p => request.BenchmarkIds.Contains(p.Id));

            var dbModel = new InboxFeedBack()
            {
                InboxId = request.InboxId,
                CreatorPersonId = request.CreatorPersonId,
                IsShop = request.IsShop,
                Value = benchmarks.Sum(p => p.Value),
                Body = request.Body,
                IsActive = true,
                CreatedAt = System.DateTime.Now,
            };

            foreach (var benchmark in benchmarks)
            {
                dbModel.InboxFeedbackBenchMarks.Add(new InboxFeedbackBenchMark()
                {
                    BenchMark = benchmark,
                    CreatedAt = System.DateTime.Now,
                });
            }

            var resultModel = await _feedBackRepository.AddAsync(dbModel);

            return new Success()
            {
                Data = new InboxFeedBack()
                {
                    InboxId = resultModel.InboxId,
                    CreatorPersonId = resultModel.CreatorPersonId,
                    IsShop = resultModel.IsShop,
                    Value = resultModel.Value,
                    Body = resultModel.Body,
                    IsActive = resultModel.IsActive,
                    CreatedAt = resultModel.CreatedAt,
                }
            };
        }
    }

}
