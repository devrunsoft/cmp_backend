using System;
using CMPNatural.Core.Enums;
using CMPNatural.Core.Helper;
using Microsoft.VisualBasic;
using ScoutDirect.Core.Entities.Base;
using System.Text;
using CMPNatural.Core.Entities;
using System.Linq;
using CMPNatural.Core.Extentions;
using CMPNatural.Core.Repositories;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CMPNatural.Application
{
	public class CreateManifestContent
	{
		public static async Task CreateContent(Invoice services,AppInformation information, Manifest entity, IServiceAppointmentLocationRepository locationRepository)
		{

            var serviceHtmlParts = new List<string>();

            foreach (var x in services.BaseServiceAppointment)
            {
                var locations = (await locationRepository.GetAsync(
                    p => p.ServiceAppointmentId == x.Id,
                    query => query.Include(l => l.LocationCompany))
                ).ToList();

                var addressLines = locations.Any()
                    ? string.Join("<br/>", locations.Select(loc =>
                        $"- {loc.LocationCompany?.Address ?? "---"} (Capacity: {loc.LocationCompany?.Capacity ?? 0})"
                    ))
                    : "";

                var part =
                    $"<strong>{x.Product?.Name}</strong> - <strong>{x.ProductPrice?.Name}</strong> " +
                    $"<br> <strong>Start Date:</strong> {x.StartDate.ToDateString()}" +
                    $"- <strong>Preferred Days:</strong> {x.DayOfWeek} " +
                    $"({x.FromHour.ConvertTimeToString()} until {x.ToHour.ConvertTimeToString()})" +
                    $"<br/><em>{addressLines}</em>";
                    //+
                    //"<br/><hr style='border-top: 1px solid #ccc;' />";

                serviceHtmlParts.Add(part);
            }

            string servicesHtml = string.Join("<br/><br/>", serviceHtmlParts);

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
            sb.AppendLine($"        <p><strong>{information.CompanyTitle}</strong>" +
                $"<br>");
            sb.AppendLine($"        {information.CompanyAddress}</p>");
            sb.AppendLine($"<p><strong>{information.CompanyPhoneNumber.FormatPhoneNumber()}</strong><p>");
            sb.AppendLine("    </div>");
            sb.AppendLine("    <div class=\"title\">Receipt</div>");
            sb.AppendLine("    <table>");
            sb.AppendLine("        <tr>");
            sb.AppendLine("            <td class=\"bold\">Date:</td>");
            sb.AppendLine($"            <td><p>{ManifestKeyEnum.Date.GetDescription()}</p></td>");
            sb.AppendLine("            <td class=\"bold\">Time:</td>");
            sb.AppendLine($"            <td><p>{ManifestKeyEnum.Time.GetDescription()}</p></td>");
            sb.AppendLine("            <td class=\"bold\">Manifest Number:</td>");
            sb.AppendLine($"            <td>{entity.ManifestNumber}</td>");
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
            sb.AppendLine("            <p class=\"bold\">Client Representative Signature:</p>");
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
        }
	}
}

