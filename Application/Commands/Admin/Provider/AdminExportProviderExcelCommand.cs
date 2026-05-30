using CMPNatural.Application.Model;
using MediatR;

namespace CMPNatural.Application
{
    public class AdminExportProviderExcelCommand : IRequest<ProviderExcelFileResult>
    {
    }
}
