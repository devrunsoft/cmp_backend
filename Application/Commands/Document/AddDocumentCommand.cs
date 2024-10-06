using System;
using CMPNatural.Application.Model;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands
{
    public class AddDocumentCommand : DocumentInput, IRequest<CommandResponse<DocumentSubmission>>
    {

        public long CompanyId { get; set; }
        public string BaseVirtualPath { get; set; }

    }
}

