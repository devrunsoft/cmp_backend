using System;
using CMPNatural.Application.Model;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Company
{
    public class UploadProfilePictureCommand : ProfilePictureInput, IRequest<CommandResponse<object>>
    {
        public UploadProfilePictureCommand()
        {
        }
        public long CompanyId { get; set; }
        public string BaseVirtualPath { get; set; }

    }
}

