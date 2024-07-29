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
    public class AddDcoumentHanlder : IRequestHandler<AddDocumentCommand, CommandResponse<object>>
    {
        private readonly IDocumentRepository _documentRepository;

        public AddDcoumentHanlder(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }

        public async Task<CommandResponse<object>> Handle(AddDocumentCommand request, CancellationToken cancellationToken)
        {


            var BusinessLicense = FileHandler.fileHandler(request.BaseVirtualPath, request.BusinessLicense, "BusinessLicense");
            var HealthDepartmentCertificate = FileHandler.fileHandler(request.BaseVirtualPath, request.HealthDepartmentCertificate, "HealthDepartmentCertificate");

            var entity = new DocumentSubmission()
            {
                BusinessLicense= BusinessLicense,
                HealthDepartmentCertificate= HealthDepartmentCertificate,
                CompanyId=request.CompanyId
            };

            var result = await _documentRepository.AddAsync(entity);


            return new CommandResponse<object>() { Success = true, Data= result, Message = "Files added successfully!" };
        }


    }
}
