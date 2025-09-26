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
    public class AddDcoumentHanlder : IRequestHandler<AddDocumentCommand, CommandResponse<DocumentSubmission>>
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IFileStorage fileStorage;

        public AddDcoumentHanlder(IDocumentRepository documentRepository, IFileStorage fileStorage)
        {
            _documentRepository = documentRepository;
            this.fileStorage = fileStorage;
        }

        public async Task<CommandResponse<DocumentSubmission>> Handle(AddDocumentCommand request, CancellationToken cancellationToken)
        {
            var lastResult = await _documentRepository.GetAsync(p=>p.CompanyId==request.CompanyId);
            if (lastResult == null || lastResult.Count==0)
            {
               string BusinessLicense = "";
                string HealthDepartmentCertificate = "";

                if (request.BusinessLicense!=null)
                 BusinessLicense = await fileStorage.AppfileHandler(request.BusinessLicense);

                if (request.HealthDepartmentCertificate != null)
                    HealthDepartmentCertificate = await fileStorage.AppfileHandler(request.HealthDepartmentCertificate);

                var entity = new DocumentSubmission()
                {
                    BusinessLicense = BusinessLicense,
                    HealthDepartmentCertificate = HealthDepartmentCertificate,
                    CompanyId = request.CompanyId
                };
                
                var result = await _documentRepository.AddAsync(entity);


                return new Success<DocumentSubmission>() {  Data = result, Message = "Files added successfully!" };

            }
            else
            {
                return new NoAcess<DocumentSubmission>() { Message = "Documantion Already Registred!" };
            }
        }


    }
}
