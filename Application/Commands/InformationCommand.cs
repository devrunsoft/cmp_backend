using System;
using CMPNatural.Application.Model;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands
{

    public class InformationCommand : IRequest<CommandResponse<CommonInformationModel>>
    {

    }
}

