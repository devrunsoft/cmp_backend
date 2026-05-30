using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using ClosedXML.Excel;
using CMPNatural.Application.Model;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using Microsoft.AspNetCore.Http;

namespace CMPNatural.Application.Services
{
    public static class VehicleExcelService
    {
        public static IReadOnlyList<VehicleExcelRowData> Parse(IFormFile file, int startRow, string? worksheetName)
        {
            if (file == null || file.Length == 0)
                return Array.Empty<VehicleExcelRowData>();

            var extension = Path.GetExtension(file.FileName)?.ToLowerInvariant();
            if (extension == ".csv")
                return ParseCsv(file, startRow);

            using var stream = file.OpenReadStream();
            using var workbook = new XLWorkbook(stream);
            var worksheet = !string.IsNullOrWhiteSpace(worksheetName)
                ? workbook.Worksheets.FirstOrDefault(w => w.Name.Equals(worksheetName, StringComparison.OrdinalIgnoreCase))
                : workbook.Worksheets.FirstOrDefault();

            if (worksheet == null)
                return Array.Empty<VehicleExcelRowData>();

            var range = worksheet.RangeUsed();
            if (range == null)
                return Array.Empty<VehicleExcelRowData>();

            var rows = new List<VehicleExcelRowData>();
            foreach (var row in range.RowsUsed().Where(r => r.RowNumber() >= startRow))
            {
                var rowData = new VehicleExcelRowData
                {
                    RowNumber = row.RowNumber(),
                    Name = GetCellString(row, VehicleExcelColumns.Name),
                    LicenseNumber = GetCellString(row, VehicleExcelColumns.LicenseNumber),
                    Capacity = GetCellInt(row, VehicleExcelColumns.Capacity),
                    Weight = GetCellDouble(row, VehicleExcelColumns.Weight),
                    VehicleRegistration = GetCellString(row, VehicleExcelColumns.VehicleRegistration),
                    VehicleRegistrationExp = GetCellDate(row, VehicleExcelColumns.VehicleRegistrationExp),
                    VehicleInsurance = GetCellString(row, VehicleExcelColumns.VehicleInsurance),
                    VehicleInsuranceExp = GetCellDate(row, VehicleExcelColumns.VehicleInsuranceExp),
                    InspectionReport = GetCellString(row, VehicleExcelColumns.InspectionReport),
                    InspectionReportExp = GetCellDate(row, VehicleExcelColumns.InspectionReportExp),
                    Picture = GetCellString(row, VehicleExcelColumns.Picture),
                    MeasurementCertificate = GetCellString(row, VehicleExcelColumns.MeasurementCertificate),
                    PeriodicVehicleInspections = GetCellString(row, VehicleExcelColumns.PeriodicVehicleInspections),
                    PeriodicVehicleInspectionsExp = GetCellDate(row, VehicleExcelColumns.PeriodicVehicleInspectionsExp),
                    VehicleCompartments = GetCellString(row, VehicleExcelColumns.VehicleCompartments),
                    VehicleServices = GetCellString(row, VehicleExcelColumns.VehicleServices)
                };

                if (rowData.IsEmpty)
                    continue;

                rows.Add(rowData);
            }

            return rows;
        }

        public static VehicleExcelFileResult Export(IReadOnlyList<Vehicle> vehicles)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Vehicles");

            WriteHeaders(worksheet);

            var rowIndex = 2;
            foreach (var vehicle in vehicles)
            {
                worksheet.Cell(rowIndex, VehicleExcelColumns.Name).Value = vehicle.Name ?? string.Empty;
                worksheet.Cell(rowIndex, VehicleExcelColumns.LicenseNumber).Value = vehicle.LicenseNumber ?? string.Empty;
                worksheet.Cell(rowIndex, VehicleExcelColumns.Capacity).Value = vehicle.Capacity;
                worksheet.Cell(rowIndex, VehicleExcelColumns.Weight).Value = vehicle.Weight;
                worksheet.Cell(rowIndex, VehicleExcelColumns.VehicleRegistration).Value = vehicle.VehicleRegistration ?? string.Empty;
                worksheet.Cell(rowIndex, VehicleExcelColumns.VehicleRegistrationExp).Value = vehicle.VehicleRegistrationExp;
                worksheet.Cell(rowIndex, VehicleExcelColumns.VehicleInsurance).Value = vehicle.VehicleInsurance ?? string.Empty;
                worksheet.Cell(rowIndex, VehicleExcelColumns.VehicleInsuranceExp).Value = vehicle.VehicleInsuranceExp;
                worksheet.Cell(rowIndex, VehicleExcelColumns.InspectionReport).Value = vehicle.InspectionReport ?? string.Empty;
                worksheet.Cell(rowIndex, VehicleExcelColumns.InspectionReportExp).Value = vehicle.InspectionReportExp;
                worksheet.Cell(rowIndex, VehicleExcelColumns.Picture).Value = vehicle.Picture ?? string.Empty;
                worksheet.Cell(rowIndex, VehicleExcelColumns.MeasurementCertificate).Value = vehicle.MeasurementCertificate ?? string.Empty;
                worksheet.Cell(rowIndex, VehicleExcelColumns.PeriodicVehicleInspections).Value = vehicle.PeriodicVehicleInspections ?? string.Empty;
                worksheet.Cell(rowIndex, VehicleExcelColumns.PeriodicVehicleInspectionsExp).Value = vehicle.PeriodicVehicleInspectionsExp;
                worksheet.Cell(rowIndex, VehicleExcelColumns.VehicleCompartments).Value = string.Join(",", vehicle.VehicleCompartment.Select(x => x.Capacity));
                worksheet.Cell(rowIndex, VehicleExcelColumns.VehicleServices).Value = string.Join(",", vehicle.VehicleService.Select(x => $"{x.VehicleServiceStatus}:{x.Capacity}"));
                rowIndex++;
            }

            worksheet.Columns().AdjustToContents();
            worksheet.SheetView.FreezeRows(1);

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return new VehicleExcelFileResult
            {
                FileName = $"vehicles-{DateTime.UtcNow:yyyyMMddHHmmss}.xlsx",
                Content = stream.ToArray()
            };
        }

        private static void WriteHeaders(IXLWorksheet worksheet)
        {
            worksheet.Cell(1, VehicleExcelColumns.Name).Value = "Name";
            worksheet.Cell(1, VehicleExcelColumns.LicenseNumber).Value = "LicenseNumber";
            worksheet.Cell(1, VehicleExcelColumns.Capacity).Value = "Capacity";
            worksheet.Cell(1, VehicleExcelColumns.Weight).Value = "Weight";
            worksheet.Cell(1, VehicleExcelColumns.VehicleRegistration).Value = "VehicleRegistration";
            worksheet.Cell(1, VehicleExcelColumns.VehicleRegistrationExp).Value = "VehicleRegistrationExp";
            worksheet.Cell(1, VehicleExcelColumns.VehicleInsurance).Value = "VehicleInsurance";
            worksheet.Cell(1, VehicleExcelColumns.VehicleInsuranceExp).Value = "VehicleInsuranceExp";
            worksheet.Cell(1, VehicleExcelColumns.InspectionReport).Value = "InspectionReport";
            worksheet.Cell(1, VehicleExcelColumns.InspectionReportExp).Value = "InspectionReportExp";
            worksheet.Cell(1, VehicleExcelColumns.Picture).Value = "Picture";
            worksheet.Cell(1, VehicleExcelColumns.MeasurementCertificate).Value = "MeasurementCertificate";
            worksheet.Cell(1, VehicleExcelColumns.PeriodicVehicleInspections).Value = "PeriodicVehicleInspections";
            worksheet.Cell(1, VehicleExcelColumns.PeriodicVehicleInspectionsExp).Value = "PeriodicVehicleInspectionsExp";
            worksheet.Cell(1, VehicleExcelColumns.VehicleCompartments).Value = "VehicleCompartments";
            worksheet.Cell(1, VehicleExcelColumns.VehicleServices).Value = "VehicleServices";

            var headerRange = worksheet.Range(1, 1, 1, VehicleExcelColumns.VehicleServices);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
        }

        private static IReadOnlyList<VehicleExcelRowData> ParseCsv(IFormFile file, int startRow)
        {
            var rows = new List<VehicleExcelRowData>();
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
                var rowData = new VehicleExcelRowData
                {
                    RowNumber = rowNumber,
                    Name = GetCsvField(fields, VehicleExcelColumns.Name),
                    LicenseNumber = GetCsvField(fields, VehicleExcelColumns.LicenseNumber),
                    Capacity = GetCsvInt(fields, VehicleExcelColumns.Capacity),
                    Weight = GetCsvDouble(fields, VehicleExcelColumns.Weight),
                    VehicleRegistration = GetCsvField(fields, VehicleExcelColumns.VehicleRegistration),
                    VehicleRegistrationExp = GetCsvDate(fields, VehicleExcelColumns.VehicleRegistrationExp),
                    VehicleInsurance = GetCsvField(fields, VehicleExcelColumns.VehicleInsurance),
                    VehicleInsuranceExp = GetCsvDate(fields, VehicleExcelColumns.VehicleInsuranceExp),
                    InspectionReport = GetCsvField(fields, VehicleExcelColumns.InspectionReport),
                    InspectionReportExp = GetCsvDate(fields, VehicleExcelColumns.InspectionReportExp),
                    Picture = GetCsvField(fields, VehicleExcelColumns.Picture),
                    MeasurementCertificate = GetCsvField(fields, VehicleExcelColumns.MeasurementCertificate),
                    PeriodicVehicleInspections = GetCsvField(fields, VehicleExcelColumns.PeriodicVehicleInspections),
                    PeriodicVehicleInspectionsExp = GetCsvDate(fields, VehicleExcelColumns.PeriodicVehicleInspectionsExp),
                    VehicleCompartments = GetCsvField(fields, VehicleExcelColumns.VehicleCompartments),
                    VehicleServices = GetCsvField(fields, VehicleExcelColumns.VehicleServices)
                };

                if (rowData.IsEmpty)
                    continue;

                rows.Add(rowData);
            }

            return rows;
        }

        private static string? GetCellString(IXLRangeRow row, int columnIndex)
        {
            var text = row.Cell(columnIndex)?.GetValue<string>()?.Trim();
            return string.IsNullOrWhiteSpace(text) ? null : text;
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

            if (DateTime.TryParse(text, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var value))
                return value;

            return null;
        }

        private static int? GetCellInt(IXLRangeRow row, int columnIndex)
        {
            var cell = row.Cell(columnIndex);
            if (cell == null)
                return null;

            if (cell.TryGetValue<int>(out var value))
                return value;

            var text = cell.GetValue<string>()?.Trim();
            if (string.IsNullOrWhiteSpace(text))
                return null;

            return int.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out value) ? value : null;
        }

        private static double? GetCellDouble(IXLRangeRow row, int columnIndex)
        {
            var cell = row.Cell(columnIndex);
            if (cell == null)
                return null;

            if (cell.TryGetValue<double>(out var value))
                return value;

            var text = cell.GetValue<string>()?.Trim();
            if (string.IsNullOrWhiteSpace(text))
                return null;

            return double.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out value) ? value : null;
        }

        private static string? GetCsvField(List<string> fields, int columnIndex)
        {
            var idx = columnIndex - 1;
            if (idx < 0 || idx >= fields.Count)
                return null;

            var value = fields[idx]?.Trim();
            return string.IsNullOrWhiteSpace(value) ? null : value;
        }

        private static DateTime? GetCsvDate(List<string> fields, int columnIndex)
        {
            var value = GetCsvField(fields, columnIndex);
            if (string.IsNullOrWhiteSpace(value))
                return null;

            if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var dateValue))
                return dateValue;

            return null;
        }

        private static int? GetCsvInt(List<string> fields, int columnIndex)
        {
            var value = GetCsvField(fields, columnIndex);
            if (string.IsNullOrWhiteSpace(value))
                return null;

            return int.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var numericValue) ? numericValue : null;
        }

        private static double? GetCsvDouble(List<string> fields, int columnIndex)
        {
            var value = GetCsvField(fields, columnIndex);
            if (string.IsNullOrWhiteSpace(value))
                return null;

            return double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var numericValue) ? numericValue : null;
        }

        private static List<string> ParseCsvLine(string line)
        {
            var fields = new List<string>();
            var current = string.Empty;
            var inQuotes = false;

            for (var i = 0; i < line.Length; i++)
            {
                var ch = line[i];
                if (ch == '"')
                {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        current += '"';
                        i++;
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }
                }
                else if (ch == ',' && !inQuotes)
                {
                    fields.Add(current);
                    current = string.Empty;
                }
                else
                {
                    current += ch;
                }
            }

            fields.Add(current);
            return fields;
        }
    }

    public static class VehicleExcelColumns
    {
        public const int Name = 1;
        public const int LicenseNumber = 2;
        public const int Capacity = 3;
        public const int Weight = 4;
        public const int VehicleRegistration = 5;
        public const int VehicleRegistrationExp = 6;
        public const int VehicleInsurance = 7;
        public const int VehicleInsuranceExp = 8;
        public const int InspectionReport = 9;
        public const int InspectionReportExp = 10;
        public const int Picture = 11;
        public const int MeasurementCertificate = 12;
        public const int PeriodicVehicleInspections = 13;
        public const int PeriodicVehicleInspectionsExp = 14;
        public const int VehicleCompartments = 15;
        public const int VehicleServices = 16;
    }

    public class VehicleExcelRowData
    {
        public int RowNumber { get; set; }
        public string? Name { get; set; }
        public string? LicenseNumber { get; set; }
        public int? Capacity { get; set; }
        public double? Weight { get; set; }
        public string? VehicleRegistration { get; set; }
        public DateTime? VehicleRegistrationExp { get; set; }
        public string? VehicleInsurance { get; set; }
        public DateTime? VehicleInsuranceExp { get; set; }
        public string? InspectionReport { get; set; }
        public DateTime? InspectionReportExp { get; set; }
        public string? Picture { get; set; }
        public string? MeasurementCertificate { get; set; }
        public string? PeriodicVehicleInspections { get; set; }
        public DateTime? PeriodicVehicleInspectionsExp { get; set; }
        public string? VehicleCompartments { get; set; }
        public string? VehicleServices { get; set; }

        public bool IsEmpty =>
            string.IsNullOrWhiteSpace(Name) &&
            string.IsNullOrWhiteSpace(LicenseNumber) &&
            !Capacity.HasValue &&
            !Weight.HasValue &&
            string.IsNullOrWhiteSpace(VehicleRegistration) &&
            !VehicleRegistrationExp.HasValue &&
            string.IsNullOrWhiteSpace(VehicleInsurance) &&
            !VehicleInsuranceExp.HasValue &&
            string.IsNullOrWhiteSpace(InspectionReport) &&
            !InspectionReportExp.HasValue &&
            string.IsNullOrWhiteSpace(Picture) &&
            string.IsNullOrWhiteSpace(MeasurementCertificate) &&
            string.IsNullOrWhiteSpace(PeriodicVehicleInspections) &&
            !PeriodicVehicleInspectionsExp.HasValue &&
            string.IsNullOrWhiteSpace(VehicleCompartments) &&
            string.IsNullOrWhiteSpace(VehicleServices);
    }
}
