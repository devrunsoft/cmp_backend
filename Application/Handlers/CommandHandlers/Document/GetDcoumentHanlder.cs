using System;
using CMPNatural.Application.Commands.Billing;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using CMPNatural.Application.Commands.Document;

namespace CMPNatural.Application.Handlers.CommandHandlers.Document
{
    public class GetDcoumentHanlder : IRequestHandler<GetDocumentCommand, CommandResponse<DocumentSubmission>>
    {
        private readonly IDocumentRepository _documentRepository;

        public GetDcoumentHanlder(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }

        public async Task<CommandResponse<DocumentSubmission>> Handle(GetDocumentCommand request, CancellationToken cancellationToken)
        {

            var result = (await _documentRepository.GetAsync(p => p.CompanyId == request.CompanyId)).FirstOrDefault();

            return new Success<DocumentSubmission>() { Data = result };
        }

    }
}

