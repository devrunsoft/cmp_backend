using CMPNatural.Application.Model;
using MediatR;
using Microsoft.AspNetCore.Http;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Admin.Company
{
    public class AdminImportCompanyExcelCommand : IRequest<CommandResponse<CompanyExcelImportResult>>
    {
        public IFormFile File { get; set; } = null!;
        public int StartRow { get; set; } = 2;
        public string? WorksheetName { get; set; }
    }
}
