using CMPNatural.Application.Model;
using MediatR;
using Microsoft.AspNetCore.Http;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class ImportVehicleExcelCommand : IRequest<CommandResponse<VehicleExcelImportResult>>
    {
        public long ProviderId { get; set; }
        public IFormFile File { get; set; } = null!;
        public int StartRow { get; set; } = 2;
        public string? WorksheetName { get; set; }
    }
}
