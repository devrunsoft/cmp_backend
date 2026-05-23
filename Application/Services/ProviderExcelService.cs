using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using ClosedXML.Excel;
using CMPNatural.Application.Model;
using CMPNatural.Core.Entities;
using Microsoft.AspNetCore.Http;

namespace CMPNatural.Application.Services
{
    public static class ProviderExcelService
    {
        public static IReadOnlyList<ProviderExcelRowData> Parse(IFormFile file, int startRow, string? worksheetName)
        {
            if (file == null || file.Length == 0)
                return Array.Empty<ProviderExcelRowData>();

            var extension = Path.GetExtension(file.FileName)?.ToLowerInvariant();
            if (extension == ".csv")
                return ParseCsv(file, startRow);

            using var stream = file.OpenReadStream();
            using var workbook = new XLWorkbook(stream);
            var worksheet = !string.IsNullOrWhiteSpace(worksheetName)
                ? workbook.Worksheets.FirstOrDefault(w => w.Name.Equals(worksheetName, StringComparison.OrdinalIgnoreCase))
                : workbook.Worksheets.FirstOrDefault();

            if (worksheet == null)
                return Array.Empty<ProviderExcelRowData>();

            var range = worksheet.RangeUsed();
            if (range == null)
                return Array.Empty<ProviderExcelRowData>();

            var rows = new List<ProviderExcelRowData>();
            foreach (var row in range.RowsUsed().Where(r => r.RowNumber() >= startRow))
            {
                var rowData = new ProviderExcelRowData
                {
                    RowNumber = row.RowNumber(),
                    Name = GetCellString(row, ProviderExcelColumns.Name),
                    Email = GetCellString(row, ProviderExcelColumns.Email),
                    PhoneNumber = GetCellString(row, ProviderExcelColumns.PhoneNumber),
                    Status = GetCellString(row, ProviderExcelColumns.Status),
                    City = GetCellString(row, ProviderExcelColumns.City),
                    Address = GetCellString(row, ProviderExcelColumns.Address),
                    County = GetCellString(row, ProviderExcelColumns.County),
                    Lat = GetCellDouble(row, ProviderExcelColumns.Lat),
                    Long = GetCellDouble(row, ProviderExcelColumns.Long),
                    AreaLocation = GetCellDouble(row, ProviderExcelColumns.AreaLocation),
                    Rating = GetCellDouble(row, ProviderExcelColumns.Rating),
                    ManagerFirstName = GetCellString(row, ProviderExcelColumns.ManagerFirstName),
                    ManagerLastName = GetCellString(row, ProviderExcelColumns.ManagerLastName),
                    ManagerPhoneNumber = GetCellString(row, ProviderExcelColumns.ManagerPhoneNumber),
                    ProductIds = GetCellString(row, ProviderExcelColumns.ProductIds),
                    ProductNames = GetCellString(row, ProviderExcelColumns.ProductNames),
                    Password = GetCellString(row, ProviderExcelColumns.Password)
                };

                if (rowData.IsEmpty)
                    continue;

                rows.Add(rowData);
            }

            return rows;
        }

        public static ProviderExcelFileResult Export(IReadOnlyList<Provider> providers)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Providers");

            WriteHeaders(worksheet);

            var rowIndex = 2;
            foreach (var provider in providers)
            {
                worksheet.Cell(rowIndex, ProviderExcelColumns.Name).Value = provider.Name ?? string.Empty;
                worksheet.Cell(rowIndex, ProviderExcelColumns.Email).Value = provider.Email ?? string.Empty;
                worksheet.Cell(rowIndex, ProviderExcelColumns.PhoneNumber).Value = provider.PhoneNumber ?? string.Empty;
                worksheet.Cell(rowIndex, ProviderExcelColumns.Status).Value = provider.Status.ToString();
                worksheet.Cell(rowIndex, ProviderExcelColumns.City).Value = provider.City ?? string.Empty;
                worksheet.Cell(rowIndex, ProviderExcelColumns.Address).Value = provider.Address ?? string.Empty;
                worksheet.Cell(rowIndex, ProviderExcelColumns.County).Value = provider.County ?? string.Empty;
                worksheet.Cell(rowIndex, ProviderExcelColumns.Lat).Value = provider.Lat;
                worksheet.Cell(rowIndex, ProviderExcelColumns.Long).Value = provider.Long;
                worksheet.Cell(rowIndex, ProviderExcelColumns.AreaLocation).Value = provider.AreaLocation;
                worksheet.Cell(rowIndex, ProviderExcelColumns.Rating).Value = provider.Rating;
                worksheet.Cell(rowIndex, ProviderExcelColumns.ManagerFirstName).Value = provider.ManagerFirstName ?? string.Empty;
                worksheet.Cell(rowIndex, ProviderExcelColumns.ManagerLastName).Value = provider.ManagerLastName ?? string.Empty;
                worksheet.Cell(rowIndex, ProviderExcelColumns.ManagerPhoneNumber).Value = provider.ManagerPhoneNumber ?? string.Empty;
                worksheet.Cell(rowIndex, ProviderExcelColumns.ProductIds).Value = string.Join(",", provider.ProviderService.Select(x => x.ProductId).Distinct().OrderBy(x => x));
                worksheet.Cell(rowIndex, ProviderExcelColumns.ProductNames).Value = string.Join(",", provider.ProviderService
                    .Select(x => x.Product?.Name)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Distinct()
                    .OrderBy(x => x));
                worksheet.Cell(rowIndex, ProviderExcelColumns.Password).Value = string.Empty;

                rowIndex++;
            }

            worksheet.Columns().AdjustToContents();
            worksheet.SheetView.FreezeRows(1);

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return new ProviderExcelFileResult
            {
                FileName = $"providers-{DateTime.UtcNow:yyyyMMddHHmmss}.xlsx",
                Content = stream.ToArray()
            };
        }

        private static void WriteHeaders(IXLWorksheet worksheet)
        {
            worksheet.Cell(1, ProviderExcelColumns.Name).Value = "Name";
            worksheet.Cell(1, ProviderExcelColumns.Email).Value = "Email";
            worksheet.Cell(1, ProviderExcelColumns.PhoneNumber).Value = "PhoneNumber";
            worksheet.Cell(1, ProviderExcelColumns.Status).Value = "Status";
            worksheet.Cell(1, ProviderExcelColumns.City).Value = "City";
            worksheet.Cell(1, ProviderExcelColumns.Address).Value = "Address";
            worksheet.Cell(1, ProviderExcelColumns.County).Value = "County";
            worksheet.Cell(1, ProviderExcelColumns.Lat).Value = "Lat";
            worksheet.Cell(1, ProviderExcelColumns.Long).Value = "Long";
            worksheet.Cell(1, ProviderExcelColumns.AreaLocation).Value = "AreaLocation";
            worksheet.Cell(1, ProviderExcelColumns.Rating).Value = "Rating";
            worksheet.Cell(1, ProviderExcelColumns.ManagerFirstName).Value = "ManagerFirstName";
            worksheet.Cell(1, ProviderExcelColumns.ManagerLastName).Value = "ManagerLastName";
            worksheet.Cell(1, ProviderExcelColumns.ManagerPhoneNumber).Value = "ManagerPhoneNumber";
            worksheet.Cell(1, ProviderExcelColumns.ProductIds).Value = "ProductIds";
            worksheet.Cell(1, ProviderExcelColumns.ProductNames).Value = "ProductNames";
            worksheet.Cell(1, ProviderExcelColumns.Password).Value = "Password";

            var headerRange = worksheet.Range(1, 1, 1, ProviderExcelColumns.Password);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
        }

        private static string? GetCellString(IXLRangeRow row, int columnIndex)
        {
            var cell = row.Cell(columnIndex);
            if (cell == null)
                return null;

            var value = cell.GetValue<string>()?.Trim();
            return string.IsNullOrWhiteSpace(value) ? null : value;
        }

        private static double? GetCellDouble(IXLRangeRow row, int columnIndex)
        {
            var cell = row.Cell(columnIndex);
            if (cell == null)
                return null;

            if (cell.TryGetValue<double>(out var numericValue))
                return numericValue;

            var text = cell.GetValue<string>()?.Trim();
            if (string.IsNullOrWhiteSpace(text))
                return null;

            if (double.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out numericValue))
                return numericValue;

            return null;
        }

        private static DateTime? GetCellDate(IXLRangeRow row, int columnIndex)
        {
            var cell = row.Cell(columnIndex);
            if (cell == null)
                return null;

            if (cell.DataType == XLDataType.DateTime)
                return cell.GetDateTime();

            var text = cell.GetValue<string>()?.Trim();
            if (string.IsNullOrWhiteSpace(text))
                return null;

            if (DateTime.TryParse(text, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var dateValue))
                return dateValue;

            return null;
        }

        private static IReadOnlyList<ProviderExcelRowData> ParseCsv(IFormFile file, int startRow)
        {
            var rows = new List<ProviderExcelRowData>();
            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);

            var rowNumber = 0;
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                rowNumber++;
                if (rowNumber < startRow)
                    continue;

                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var fields = ParseCsvLine(line);
                var rowData = new ProviderExcelRowData
                {
                    RowNumber = rowNumber,
                    Name = GetCsvField(fields, ProviderExcelColumns.Name),
                    Email = GetCsvField(fields, ProviderExcelColumns.Email),
                    PhoneNumber = GetCsvField(fields, ProviderExcelColumns.PhoneNumber),
                    Status = GetCsvField(fields, ProviderExcelColumns.Status),
                    City = GetCsvField(fields, ProviderExcelColumns.City),
                    Address = GetCsvField(fields, ProviderExcelColumns.Address),
                    County = GetCsvField(fields, ProviderExcelColumns.County),
                    Lat = GetCsvDouble(fields, ProviderExcelColumns.Lat),
                    Long = GetCsvDouble(fields, ProviderExcelColumns.Long),
                    AreaLocation = GetCsvDouble(fields, ProviderExcelColumns.AreaLocation),
                    Rating = GetCsvDouble(fields, ProviderExcelColumns.Rating),
                    ManagerFirstName = GetCsvField(fields, ProviderExcelColumns.ManagerFirstName),
                    ManagerLastName = GetCsvField(fields, ProviderExcelColumns.ManagerLastName),
                    ManagerPhoneNumber = GetCsvField(fields, ProviderExcelColumns.ManagerPhoneNumber),
                    ProductIds = GetCsvField(fields, ProviderExcelColumns.ProductIds),
                    ProductNames = GetCsvField(fields, ProviderExcelColumns.ProductNames),
                    Password = GetCsvField(fields, ProviderExcelColumns.Password)
                };

                if (rowData.IsEmpty)
                    continue;

                rows.Add(rowData);
            }

            return rows;
        }

        private static string? GetCsvField(List<string> fields, int columnIndex)
        {
            var idx = columnIndex - 1;
            if (idx < 0 || idx >= fields.Count)
                return null;

            var value = fields[idx]?.Trim();
            return string.IsNullOrWhiteSpace(value) ? null : value;
        }

        private static double? GetCsvDouble(List<string> fields, int columnIndex)
        {
            var value = GetCsvField(fields, columnIndex);
            if (string.IsNullOrWhiteSpace(value))
                return null;

            if (double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var numericValue))
                return numericValue;

            return null;
        }

        private static List<string> ParseCsvLine(string line)
        {
            var fields = new List<string>();
            var current = new System.Text.StringBuilder();
            var inQuotes = false;

            for (var i = 0; i < line.Length; i++)
            {
                var c = line[i];
                if (c == '"')
                {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        current.Append('"');
                        i++;
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }

                    continue;
                }

                if (c == ',' && !inQuotes)
                {
                    fields.Add(current.ToString());
                    current.Clear();
                    continue;
                }

                current.Append(c);
            }

            fields.Add(current.ToString());
            return fields;
        }
    }

    public static class ProviderExcelColumns
    {
        public const int Name = 1;
        public const int Email = 2;
        public const int PhoneNumber = 3;
        public const int Status = 4;
        public const int City = 5;
        public const int Address = 6;
        public const int County = 7;
        public const int Lat = 8;
        public const int Long = 9;
        public const int AreaLocation = 10;
        public const int Rating = 11;
        public const int ManagerFirstName = 12;
        public const int ManagerLastName = 13;
        public const int ManagerPhoneNumber = 14;
        public const int ProductIds = 15;
        public const int ProductNames = 16;
        public const int Password = 17;
    }

    public class ProviderExcelRowData
    {
        public int RowNumber { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Status { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }
        public string? County { get; set; }
        public double? Lat { get; set; }
        public double? Long { get; set; }
        public double? AreaLocation { get; set; }
        public double? Rating { get; set; }
        public string? ManagerFirstName { get; set; }
        public string? ManagerLastName { get; set; }
        public string? ManagerPhoneNumber { get; set; }
        public string? ProductIds { get; set; }
        public string? ProductNames { get; set; }
        public string? Password { get; set; }

        public bool IsEmpty =>
            string.IsNullOrWhiteSpace(Name) &&
            string.IsNullOrWhiteSpace(Email) &&
            string.IsNullOrWhiteSpace(PhoneNumber) &&
            string.IsNullOrWhiteSpace(ProductIds) &&
            string.IsNullOrWhiteSpace(ProductNames);
    }
}
