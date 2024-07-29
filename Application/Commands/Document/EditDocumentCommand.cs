using System;
using CMPNatural.Application.Model;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands
{
    public class EditDocumentCommand : DocumentInput, IRequest<CommandResponse<object>>
    {
        public EditDocumentCommand()
        {
        }

        public long CompanyId { get; set; }
        public string BaseVirtualPath { get; set; }
        public long Id { get; set; }

    }
}

