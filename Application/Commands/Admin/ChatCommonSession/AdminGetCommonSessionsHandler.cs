using System;
using System.Collections.Generic;
using CMPNatural.Application.Responses.ChatCommon;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Base;

namespace CMPNatural.Application
{
    public class AdminGetCommonSessionsCommand :  IRequest<CommandResponse<ChatCommonSessionEntity>>
    {
        public long AdminId { get; set; }
        public long ClientId { get; set; }
    }
}

