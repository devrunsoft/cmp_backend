using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Repositories;
using CMPNatural.Application.Commands.Admin.Notification;
using System.Linq;

namespace CMPNatural.Application
{
    public class AdminGetUnSeenNotificationHandler : IRequestHandler<AdminGetUnSeenNotificationCommand, CommandResponse<int>>
    {
        private readonly INotificationRepository _repository;

        public AdminGetUnSeenNotificationHandler(INotificationRepository _repository)
        {
            this._repository = _repository;
        }

        public async Task<CommandResponse<int>> Handle(AdminGetUnSeenNotificationCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetAsync(p=>p.Seen==null)).Count();
            return new Success<int>() { Data = result };
        }
    }
}

