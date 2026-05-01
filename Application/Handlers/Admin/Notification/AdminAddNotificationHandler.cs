using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Repositories;
using System;
using System.Text.Json;

namespace CMPNatural.Application
{
    public class AdminAddNotificationHandler : IRequestHandler<AdminAddNotificationCommand, CommandResponse<Notification>>
    {
        private readonly INotificationRepository _repository;

        public AdminAddNotificationHandler(INotificationRepository _repository)
        {
            this._repository = _repository;
        }

        public async Task<CommandResponse<Notification>> Handle(AdminAddNotificationCommand request, CancellationToken cancellationToken)
        {
            var notification = new Notification()
            {
                Title=request.Title,
                Body=request.Body,
                Payload = request.Payload == null ? null : JsonSerializer.Serialize(request.Payload),
                CreateAt = DateTime.Now,
                type = request.type,
            };

            var result = (await _repository.AddAsync(notification));
            return new Success<Notification>() { Data = result };
        }
    }
}

