using System;
using CMPNatural.Application.Model;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands
{
    public class AddDocumentCommand : DocumentInput, IRequest<CommandResponse<object>>
    {
        public AddDocumentCommand()
        {
        }

        public long CompanyId { get; set; }
        public string BaseVirtualPath { get; set; }

    }
}

