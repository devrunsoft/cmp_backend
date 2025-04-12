using System;
using CMPNatural.Core.Entities;
using ScoutDirect.Application.Responses;
using System.Threading.Tasks;
using CMPNatural.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using CMPNatural.Core.Enums;
using System.Collections.Generic;
using CMPNatural.Core.Extentions;
using CMPNatural.Core.Helper;
using System.Reflection.Metadata;

namespace CMPNatural.Application
{
	public class AdminCreateManifestHandler
	{
        private readonly IinvoiceRepository _invoiceRepository;
        private readonly IAppInformationRepository _apprepository;
        private readonly IManifestRepository _repository;
        public AdminCreateManifestHandler(IManifestRepository _repository, IinvoiceRepository _invoiceRepository, IAppInformationRepository _apprepository)
        {

            this._invoiceRepository = _invoiceRepository;
            this._apprepository = _apprepository;
            this._repository = _repository;
        }
        public async Task<CommandResponse<Manifest>> Create(Invoice invoice)
		{
            var information = (await _apprepository.GetAllAsync()).LastOrDefault();
            var services = (await _invoiceRepository.GetAsync(x => x.InvoiceId == invoice.InvoiceId, query => query
                .Include(x => x.BaseServiceAppointment)
                .ThenInclude(x => x.Product)
                .Include(x => x.BaseServiceAppointment)
                .ThenInclude(x => x.ProductPrice)
                )).FirstOrDefault();

            var entity = new Manifest()
            {
                InvoiceId = invoice.Id,
                Status = ManifestStatus.Draft,
                Content="",
            };

            var result = await _repository.AddAsync(entity);
            string servicesHtml = string.Join("<br/>",
                services.BaseServiceAppointment.Select(x =>
                    $"<strong>{x.Product.Name}</strong> - <strong>{x.ProductPrice.Name}</strong> " +
                    $"- <strong>Preferred Days:</strong> {x.DayOfWeek} " +
                    $"({x.FromHour.ConvertTimeToString()} until {x.ToHour.ConvertTimeToString()})"
                )
            );

            string finalHtmlRow =
                $"<tr>" +
                $"<td class=\"bold\">Services:</td>" +
                $"<td colspan=\"5\">{servicesHtml}</td>" +
                $"</tr>";


            var managementCompany = new
            {
                Logo = $"<img width={40} height={40}  src='https://api.app-cmp.com{information.CompanyIcon}' alt='Company Logo' />",

                Services = finalHtmlRow
            };

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<style>");
            sb.AppendLine("    body { font-family: Arial, sans-serif; margin: 20px; }");
            sb.AppendLine("    .container { min-width: 700px; max-width: 1000px; margin: 0 auto; border: 1px solid #000; padding: 10px; }");
            sb.AppendLine("    .header { text-align: left; font-weight: bold; }");
            sb.AppendLine("    .title { text-align: center; font-size: 20px; margin: 10px 0; }");
            sb.AppendLine("    table { width: 100%; border-collapse: collapse; margin-top: 10px; }");
            sb.AppendLine("    th, td { border: 1px solid #000; padding: 8px; text-align: left; }");
            sb.AppendLine("    .bold { font-weight: bold; }");
            sb.AppendLine("    .signature { margin-top: 20px; display: flex; justify-content: space-between; }");
            sb.AppendLine("    .signature div { text-align: center; flex: 1; }");
            sb.AppendLine("</style>");

            sb.AppendLine("<div class=\"container\">");
            sb.AppendLine("    <div class=\"header\">");
            sb.AppendLine($"              {managementCompany.Logo}");
            sb.AppendLine($"        <p><strong>{information.CompanyTitle}</strong><br>");
            sb.AppendLine($"        {information.CompanyAddress}</p>");
            sb.AppendLine("    </div>");
            sb.AppendLine("    <div class=\"title\">Receipt</div>");
            sb.AppendLine("    <table>");
            sb.AppendLine("        <tr>");
            sb.AppendLine("            <td class=\"bold\">Date:</td>");
            sb.AppendLine($"            <td><p>{ManifestKeyEnum.Date.GetDescription()}</p></td>");
            sb.AppendLine("            <td class=\"bold\">Time:</td>");
            sb.AppendLine($"            <td><p>{ManifestKeyEnum.Time.GetDescription()}</p></td>");
            sb.AppendLine("            <td class=\"bold\">Manifest Number:</td>");
            sb.AppendLine($"            <td>{entity.Id}</td>");
            sb.AppendLine("        </tr>");
            sb.AppendLine("        <tr>");
            sb.AppendLine("            <td class=\"bold\">Provider Name:</td>");
            sb.AppendLine($"            <td colspan=\"5\"><p>{ManifestKeyEnum.ProviderName.GetDescription()}</p></td>");
            sb.AppendLine("        </tr>");
            //sb.AppendLine("        <tr>");
            //sb.AppendLine("            <td class=\"bold\">Provider Address:</td>");
            //sb.AppendLine($"            <td colspan=\"5\"><p>{ManifestKeyEnum.ProviderAddress.GetDescription()}</p></td>");
            //sb.AppendLine("        </tr>");
            sb.AppendLine("        <tr>");
            sb.AppendLine("            <td class=\"bold\">Driver Name:</td>");
            sb.AppendLine("            <td colspan=\"5\"></td>");
            sb.AppendLine("        </tr>");
            sb.AppendLine($"        {managementCompany.Services}");
            sb.AppendLine("    </table>");
            sb.AppendLine("    <div class=\"signature\">");
            sb.AppendLine("        <div>");
            sb.AppendLine("            <p class=\"bold\">Driver Signature:</p>");
            sb.AppendLine("            <p><em></em></p>");
            sb.AppendLine("        </div>");
            sb.AppendLine("        <div>");
            sb.AppendLine("            <p class=\"bold\">Generator Representative Signature:</p>");
            sb.AppendLine("            <p>Signature On-file</p>");
            sb.AppendLine("        </div>");
            sb.AppendLine("    </div>");
            sb.AppendLine("</div>");

            string receiptHtml = sb.ToString();
            receiptHtml = CompanyContractHelper.HideByKey(ManifestKeyEnum.Date.GetDescription(), receiptHtml);
            receiptHtml = CompanyContractHelper.HideByKey(ManifestKeyEnum.Time.GetDescription(), receiptHtml);
            receiptHtml = CompanyContractHelper.HideByKey(ManifestKeyEnum.ProviderAddress.GetDescription(), receiptHtml);
            receiptHtml = CompanyContractHelper.HideByKey(ManifestKeyEnum.ProviderName.GetDescription(), receiptHtml);
            entity.Content = receiptHtml;
            await _repository.UpdateAsync(entity);
            return new Success<Manifest>() { Data = result, Message = "Successfull!" };

        }
    }
}

