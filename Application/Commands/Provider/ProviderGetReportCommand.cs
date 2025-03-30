using System;
using CMPNatural.Application.Responses.Report;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Provider
{
	public class ProviderGetReportCommand : IRequest<CommandResponse<ReportResponse>>
    {
		public ProviderGetReportCommand()
		{
		}
		public long ProviderId { get; set; }
	}
}

