using System;
using System.Collections.Generic;
using CMPNatural.Application.Responses;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Base;

namespace CMPNatural.Application
{
    public class AdminGetAllCompanyCommand : PagedQueryRequest, IRequest<CommandResponse<PagesQueryResponse<CompanyResponse>>>
    {
    }
}

