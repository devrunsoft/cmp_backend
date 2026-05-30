using CMPNatural.Application.Model;
using MediatR;

namespace CMPNatural.Application
{
    public class ExportVehicleExcelCommand : IRequest<VehicleExcelFileResult>
    {
        public long ProviderId { get; set; }
    }
}
