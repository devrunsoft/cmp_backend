using CMPNatural.Application.Model;
using MediatR;

namespace CMPNatural.Application
{
    public class ExportDriverExcelCommand : IRequest<DriverExcelFileResult>
    {
        public long ProviderId { get; set; }
    }
}
