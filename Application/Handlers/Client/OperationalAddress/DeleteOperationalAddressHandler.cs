using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Barbara.Application.Responses;
using CMPNatural.Application.Commands;
using CMPNatural.Application.Commands.OperationalAddress;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using CMPNatural.Core.Repositories;
using CMPNatural.Core.Repositories.Chat;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Handlers.CommandHandlers
{
    public class DeleteOperationalAddressHandler : IRequestHandler<DeleteOperationalAddressCommand, CommandResponse<OperationalAddress>>
    {
        private readonly IMediator _mediator;
        private readonly IOperationalAddressRepository _operationalAddressRepository;
        private readonly ILocationCompanyRepository _locationCompanyRepository;
        private readonly ILocationDateTimeRepository _locationDateTimeRepository;
        private readonly IBaseServiceAppointmentRepository _baseServiceAppointmentRepository;
        private readonly IServiceAppointmentRepository _serviceAppointmentRepository;
        private readonly IServiceAppointmentEmergencyRepository _serviceAppointmentEmergencyRepository;
        private readonly IRequestRepository _requestRepository;
        private readonly IinvoiceRepository _invoiceRepository;
        private readonly IInvoiceSourceRepository _invoiceSourceRepository;
        private readonly IShoppingCardRepository _shoppingCardRepository;
        private readonly IRequestTerminateRepository _requestTerminateRepository;
        private readonly ICompanyContractRepository _companyContractRepository;
        private readonly IProviderContractRepository _providerContractRepository;
        private readonly IManifestRepository _manifestRepository;
        private readonly IChatSessionRepository _chatSessionRepository;
        private readonly IChatMessageRepository _chatMessageRepository;
        private readonly IChatMessageNoteRepository _chatMessageNoteRepository;
        private readonly IChatMessageManualNoteRepository _chatMessageManualNoteRepository;
        private readonly IChatMentionRepository _chatMentionRepository;

        public DeleteOperationalAddressHandler(
            IMediator mediator,
            IOperationalAddressRepository operationalAddressRepository,
            ILocationCompanyRepository locationCompanyRepository,
            ILocationDateTimeRepository locationDateTimeRepository,
            IBaseServiceAppointmentRepository baseServiceAppointmentRepository,
            IServiceAppointmentRepository serviceAppointmentRepository,
            IServiceAppointmentEmergencyRepository serviceAppointmentEmergencyRepository,
            IRequestRepository requestRepository,
            IinvoiceRepository invoiceRepository,
            IInvoiceSourceRepository invoiceSourceRepository,
            IShoppingCardRepository shoppingCardRepository,
            IRequestTerminateRepository requestTerminateRepository,
            ICompanyContractRepository companyContractRepository,
            IProviderContractRepository providerContractRepository,
            IManifestRepository manifestRepository,
            IChatSessionRepository chatSessionRepository,
            IChatMessageRepository chatMessageRepository,
            IChatMessageNoteRepository chatMessageNoteRepository,
            IChatMessageManualNoteRepository chatMessageManualNoteRepository,
            IChatMentionRepository chatMentionRepository)
        {
            _mediator = mediator;
            _operationalAddressRepository = operationalAddressRepository;
            _locationCompanyRepository = locationCompanyRepository;
            _locationDateTimeRepository = locationDateTimeRepository;
            _baseServiceAppointmentRepository = baseServiceAppointmentRepository;
            _serviceAppointmentRepository = serviceAppointmentRepository;
            _serviceAppointmentEmergencyRepository = serviceAppointmentEmergencyRepository;
            _requestRepository = requestRepository;
            _invoiceRepository = invoiceRepository;
            _invoiceSourceRepository = invoiceSourceRepository;
            _shoppingCardRepository = shoppingCardRepository;
            _requestTerminateRepository = requestTerminateRepository;
            _companyContractRepository = companyContractRepository;
            _providerContractRepository = providerContractRepository;
            _manifestRepository = manifestRepository;
            _chatSessionRepository = chatSessionRepository;
            _chatMessageRepository = chatMessageRepository;
            _chatMessageNoteRepository = chatMessageNoteRepository;
            _chatMessageManualNoteRepository = chatMessageManualNoteRepository;
            _chatMentionRepository = chatMentionRepository;
        }

        public async Task<CommandResponse<OperationalAddress>> Handle(DeleteOperationalAddressCommand request, CancellationToken cancellationToken)
        {
            var entity = await _operationalAddressRepository.GetByIdAsync(request.Id);
            if (entity == null || entity.CompanyId != request.CompanyId)
            {
                return new NoAcess<OperationalAddress>() { Message = "No access to delete!" };
            }

            var locations = await _locationCompanyRepository.GetAsync(x => x.OperationalAddressId == request.Id && x.Type != (int)LocationType.Other);
            if (locations.Count > 0)
            {
                return new NoAcess<OperationalAddress>()
                {
                    Message = "Please delete all locations for this operational address first."
                };
            }

            if (await HasRelationsAsync(request.Id))
            {
                return new NoAcess<OperationalAddress>()
                {
                    Message = "This operational address cannot be deleted because it is associated with other records."
                };
            }

            var otherLocation = (await _locationCompanyRepository.GetAsync(x => x.OperationalAddressId == request.Id && x.Type == (int)LocationType.Other)).FirstOrDefault();

            if (otherLocation != null)
            {
                var result = await _mediator.Send(new DeleteLocationCommand() { Id = otherLocation.Id, CompanyId = otherLocation.CompanyId});
                if (!result.IsSucces())
                {
                    return new NoAcess<OperationalAddress>()
                    {
                        Message = result.Message
                    };
                }
            }

            var locationDateTimes = await _locationDateTimeRepository.GetAsync(x => x.OperationalAddressId == request.Id);
            if (locationDateTimes.Count > 0)
            {
                await _locationDateTimeRepository.DeleteRangeAsync(locationDateTimes.ToList());
            }

            await _operationalAddressRepository.DeleteAsync(entity);

            return new Success<OperationalAddress>()
            {
                Success = true,
                Data = entity,
                Message = "OperationalAddres deleted successfully!"
            };
        }

        private async Task<bool> HasRelationsAsync(long operationalAddressId)
        {
            return
                //(await _locationDateTimeRepository.GetAsync(x => x.OperationalAddressId == operationalAddressId)).Count > 0 ||
                (await _baseServiceAppointmentRepository.GetAsync(x => x.OperationalAddressId == operationalAddressId)).Count > 0 ||
                (await _serviceAppointmentRepository.GetAsync(x => x.OperationalAddressId == operationalAddressId)).Count > 0 ||
                (await _serviceAppointmentEmergencyRepository.GetAsync(x => x.OperationalAddressId == operationalAddressId)).Count > 0 ||
                (await _requestRepository.GetAsync(x => x.OperationalAddressId == operationalAddressId)).Count > 0 ||
                (await _invoiceRepository.GetAsync(x => x.OperationalAddressId == operationalAddressId)).Count > 0 ||
                (await _invoiceSourceRepository.GetAsync(x => x.OperationalAddressId == operationalAddressId)).Count > 0 ||
                (await _shoppingCardRepository.GetAsync(x => x.OperationalAddressId == operationalAddressId)).Count > 0 ||
                (await _requestTerminateRepository.GetAsync(x => x.OperationalAddressId == operationalAddressId)).Count > 0 ||
                (await _companyContractRepository.GetAsync(x => x.OperationalAddressId == operationalAddressId)).Count > 0 ||
                (await _providerContractRepository.GetAsync(x => x.OperationalAddressId == operationalAddressId)).Count > 0 ||
                (await _manifestRepository.GetAsync(x => x.OperationalAddressId == operationalAddressId)).Count > 0;
                //(await _chatSessionRepository.GetAsync(x => x.OperationalAddressId == operationalAddressId)).Count > 0 ||
                //(await _chatMessageRepository.GetAsync(x => x.OperationalAddressId == operationalAddressId)).Count > 0 ||
                //(await _chatMessageNoteRepository.GetAsync(x => x.OperationalAddressId == operationalAddressId)).Count > 0 ||
                //(await _chatMessageManualNoteRepository.GetAsync(x => x.OperationalAddressId == operationalAddressId)).Count > 0 ||
                //(await _chatMentionRepository.GetAsync(x => x.OperationalAddressId == operationalAddressId)).Count > 0;
        }
    }
}
