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
using CMPFile;

namespace CMPNatural.Application.Handlers.CommandHandlers
{
    public class EditDcoumentHanlder : IRequestHandler<EditDocumentCommand, CommandResponse<object>>
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IFileStorage fileStorage;

        public EditDcoumentHanlder(IDocumentRepository documentRepository, IFileStorage fileStorage)
        {
            _documentRepository = documentRepository;
            this.fileStorage = fileStorage;
        }

        public async Task<CommandResponse<object>> Handle(EditDocumentCommand request, CancellationToken cancellationToken)
        {
            var entity = await _documentRepository.GetByIdAsync(request.Id);

            if (entity.CompanyId != request.CompanyId)
            {
                return new NoAcess() { Message = "No Access to Edit!" };
            }

            var BusinessLicense = await fileStorage.AppfileHandler(request.BusinessLicense);
            var HealthDepartmentCertificate = await fileStorage.AppfileHandler(request.HealthDepartmentCertificate);


            entity.BusinessLicense = BusinessLicense;
            entity.HealthDepartmentCertificate = HealthDepartmentCertificate;
            entity.CompanyId = request.CompanyId;


             await _documentRepository.UpdateAsync(entity);


            return new CommandResponse<object>() { Success = true, Data = entity, Message = "Files Updated successfully!" };
        }


    }
}
