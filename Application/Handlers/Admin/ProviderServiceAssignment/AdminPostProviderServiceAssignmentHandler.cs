//using CMPNatural.Core.Entities;
//using MediatR;
//using ScoutDirect.Application.Responses;
//using System.Threading;
//using System.Threading.Tasks;
//using CMPNatural.Core.Repositories;
//using CMPNatural.Core.Enums;
//using System;

//namespace CMPNatural.Application
//{

//    public class AdminPostProviderServiceAssignmentHandler : IRequestHandler<AdminPostProviderServiceAssignmentCommand, CommandResponse<ProviderServiceAssignment>>
//    {
//        private readonly IProviderServiceAssignmentRepository _providerServiceAssignmentRepository;
//        private readonly IinvoiceRepository _invoiceRepository;

//        public AdminPostProviderServiceAssignmentHandler(IProviderServiceAssignmentRepository providerServiceAssignmentRepository,
//            IinvoiceRepository invoiceRepository
//            )
//        {
//            _providerServiceAssignmentRepository = providerServiceAssignmentRepository;
//            _invoiceRepository = invoiceRepository;
//        }

//        public async Task<CommandResponse<ProviderServiceAssignment>> Handle(AdminPostProviderServiceAssignmentCommand request, CancellationToken cancellationToken)
//        {
//            var invoice = await _invoiceRepository.GetByIdAsync(request.InvoiceId);

//            if (invoice.ProviderId!=null)
//            {
//                return new NoAcess<ProviderServiceAssignment>() { Message = "You Assigned this Service!" };
//            }

//            //if (invoice.Status == ServiceStatus.paid.GetDescription())
//            //{
//            //    return new NoAcess<ProviderServiceAssignment>() { Message = "You can't Assign this service" };
//            //}

//            var entity = new ProviderServiceAssignment()
//            {
//                AssignTime = DateTime.Now,
//                CompanyId = invoice.CompanyId,
//                InvoiceId = request.InvoiceId,
//                ProviderId = request.ProviderId,
//                Status = (int) ProviderServiceAssignmentStatus.assigned
//            };
//            var result = await _providerServiceAssignmentRepository.AddAsync(entity);

//            //
//            invoice.ProviderId = request.ProviderId;
//            await _invoiceRepository.UpdateAsync(invoice);
//            //

//            return new Success<ProviderServiceAssignment>() { Data = result };
//        }
//    }
//}

