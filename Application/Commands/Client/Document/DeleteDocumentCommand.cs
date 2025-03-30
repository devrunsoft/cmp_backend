using System;
using CMPNatural.Application.Model;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands
{
    public class DeleteDocumentCommand :  IRequest<CommandResponse<object>>
    {
        public DeleteDocumentCommand()
        {
        }
        public long Id { get; set; }

        public long CompanyId { get; set; }

    }
}

