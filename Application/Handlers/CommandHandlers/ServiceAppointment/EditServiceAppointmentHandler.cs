using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CMPNatural.Application.Handlers
    {
        public class EditServiceAppointmentHandler : IRequestHandler<EditerviceAppointmentCommand, CommandResponse<ServiceAppointment>>
        {
            private readonly IServiceAppointmentRepository _serviceAppointmentRepository;

            public EditServiceAppointmentHandler(IServiceAppointmentRepository billingInformationRepository)
            {
                _serviceAppointmentRepository = billingInformationRepository;
            }

            public async Task<CommandResponse<ServiceAppointment>> Handle(EditerviceAppointmentCommand request, CancellationToken cancellationToken)
            {


            var result = (await _serviceAppointmentRepository.GetAsync(p => p.ServiceCrmId == request.ServiceId));
            var entity = result.FirstOrDefault();
            entity.Status = (int)request.Status;

                await _serviceAppointmentRepository.UpdateAsync(entity);

                return new Success<ServiceAppointment>() { Data = entity, Message = "Service updated Successfully!" };

            }

        }
    }
