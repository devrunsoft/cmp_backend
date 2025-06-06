using System;
using CMPNatural.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands
{
    public class AdminGoHighLevelPutCommand : IRequest<CommandResponse<GoHighLevel>>
    {
        public AdminGoHighLevelPutCommand()
        {
        }

        public string LocationId { get; set; }
        public string Authorization { get; set; }
        public string RestApi { get; set; }
        public string Version { get; set; }
        public string UpdateContactApi { get; set; }
        public string ForgotPasswordApi { get; set; }
        public string ActivationLinkApi { get; set; }
    }
}

