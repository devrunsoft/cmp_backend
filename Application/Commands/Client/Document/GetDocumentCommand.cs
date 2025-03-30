using System;
using CMPNatural.Application.Model;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Document
{
    public class GetDocumentCommand : IRequest<CommandResponse<DocumentSubmission>>
    {

        public long CompanyId { get; set; }

    }
}

