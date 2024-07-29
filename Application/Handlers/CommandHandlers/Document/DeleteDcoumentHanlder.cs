using Barbara.Core.Entities;
using Barbara.Application.Responses;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using ScoutDirect.Core.Repositories;
using ScoutDirect.Core.Entities;
using ScoutDirect.Application.Responses;

using CMPNatural.Application.Responses;
using CMPNatural.Core.Entities;
using CMPNatural.Application.Mapper;
using CMPNatural.Application.Commands;
using CMPNatural.Core.Repositories;
using System.IO;
using Microsoft.AspNetCore.Http;
using CMPNatural.Application.Services;

namespace CMPNatural.Application.Handlers.CommandHandlers
{
    public class DeleteDcoumentHanlder : IRequestHandler<DeleteDocumentCommand, CommandResponse<object>>
    {
        private readonly IDocumentRepository _documentRepository;

        public DeleteDcoumentHanlder(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }

        public async Task<CommandResponse<object>> Handle(DeleteDocumentCommand request, CancellationToken cancellationToken)
        {
            var entity = await _documentRepository.GetByIdAsync(request.Id);

            if (entity.CompanyId != request.CompanyId)
            {
                return new NoAcess() { Message = "No Access to Edit!" };
            }

            await _documentRepository.DeleteAsync(entity);

            return new CommandResponse<object>() { Success = true, Message = "Files Updated successfully!" };
        }


    }
}
