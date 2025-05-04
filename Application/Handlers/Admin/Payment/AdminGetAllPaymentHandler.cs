using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace CMPNatural.Application
{
    public class AdminGetAllPaymentHandler : IRequestHandler<AdminGetAllPaymentCommand, CommandResponse<PagesQueryResponse<Payment>>>
    {
        private readonly IPaymentRepository _repository;

        public AdminGetAllPaymentHandler(IPaymentRepository _repository)
        {
            this._repository = _repository;
        }

        public async Task<CommandResponse<PagesQueryResponse<Payment>>> Handle(AdminGetAllPaymentCommand request, CancellationToken cancellationToken)
        {
            var invoices = (await _repository.GetBasePagedAsync(request, p => request.Status == null || p.Status == request.Status, null));
            return new Success<PagesQueryResponse<Payment>>() { Data = invoices };
        }
    }
}

