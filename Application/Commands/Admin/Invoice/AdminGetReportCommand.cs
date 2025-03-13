using System;
using CMPNatural.Application.Responses.Report;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class AdminGetReportCommand : IRequest<CommandResponse<ReportResponse>>
	{
		public AdminGetReportCommand()
		{
		}
	}
}

