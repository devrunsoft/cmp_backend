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
        public const string ProvidersWorksheetName = "Providers";
        public const string DriversWorksheetName = "Drivers";
        public const string VehiclesWorksheetName = "Vehicles";

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

        public static ProviderExcelWorkbookData ParseWorkbook(IFormFile file, int startRow)
        {
            var workbookData = new ProviderExcelWorkbookData();
            if (file == null || file.Length == 0)
                return workbookData;

            var extension = Path.GetExtension(file.FileName)?.ToLowerInvariant();
            if (extension == ".csv")
            {
                workbookData.Providers = ParseCsv(file, startRow);
                return workbookData;
            }

            using var stream = file.OpenReadStream();
            using var workbook = new XLWorkbook(stream);

            workbookData.Providers = ParseProviderWorksheet(workbook.Worksheets.FirstOrDefault(x => x.Name.Equals(ProvidersWorksheetName, StringComparison.OrdinalIgnoreCase)), startRow);
            workbookData.Drivers = ParseDriverWorksheet(workbook.Worksheets.FirstOrDefault(x => x.Name.Equals(DriversWorksheetName, StringComparison.OrdinalIgnoreCase)), startRow);
            workbookData.Vehicles = ParseVehicleWorksheet(workbook.Worksheets.FirstOrDefault(x => x.Name.Equals(VehiclesWorksheetName, StringComparison.OrdinalIgnoreCase)), startRow);

            return workbookData;
        }

        public static ProviderExcelFileResult Export(
            IReadOnlyList<Provider> providers,
            IReadOnlyList<ProviderDriverExcelRowData>? drivers = null,
            IReadOnlyList<ProviderVehicleExcelRowData>? vehicles = null)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(ProvidersWorksheetName);

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

            WriteDriverWorksheet(workbook, drivers ?? Array.Empty<ProviderDriverExcelRowData>());
            WriteVehicleWorksheet(workbook, vehicles ?? Array.Empty<ProviderVehicleExcelRowData>());

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

        private static IReadOnlyList<ProviderExcelRowData> ParseProviderWorksheet(IXLWorksheet? worksheet, int startRow)
        {
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

                if (!rowData.IsEmpty)
                    rows.Add(rowData);
            }

            return rows;
        }

        private static IReadOnlyList<ProviderDriverSheetRowData> ParseDriverWorksheet(IXLWorksheet? worksheet, int startRow)
        {
            if (worksheet == null)
                return Array.Empty<ProviderDriverSheetRowData>();

            var range = worksheet.RangeUsed();
            if (range == null)
                return Array.Empty<ProviderDriverSheetRowData>();

            var rows = new List<ProviderDriverSheetRowData>();
            foreach (var row in range.RowsUsed().Where(r => r.RowNumber() >= startRow))
            {
                var rowData = new ProviderDriverSheetRowData
                {
                    RowNumber = row.RowNumber(),
                    ProviderEmail = GetCellString(row, ProviderDriverExcelColumns.ProviderEmail),
                    FirstName = GetCellString(row, ProviderDriverExcelColumns.FirstName),
                    LastName = GetCellString(row, ProviderDriverExcelColumns.LastName),
                    Email = GetCellString(row, ProviderDriverExcelColumns.Email),
                    IsDefault = GetCellBool(row, ProviderDriverExcelColumns.IsDefault),
                    License = GetCellString(row, ProviderDriverExcelColumns.License),
                    LicenseExp = GetCellDate(row, ProviderDriverExcelColumns.LicenseExp),
                    BackgroundCheck = GetCellString(row, ProviderDriverExcelColumns.BackgroundCheck),
                    BackgroundCheckExp = GetCellDate(row, ProviderDriverExcelColumns.BackgroundCheckExp),
                    ProfilePhoto = GetCellString(row, ProviderDriverExcelColumns.ProfilePhoto),
                    Status = GetCellString(row, ProviderDriverExcelColumns.Status),
                    Password = GetCellString(row, ProviderDriverExcelColumns.Password)
                };

                if (!rowData.IsEmpty)
                    rows.Add(rowData);
            }

            return rows;
        }

        private static IReadOnlyList<ProviderVehicleSheetRowData> ParseVehicleWorksheet(IXLWorksheet? worksheet, int startRow)
        {
            if (worksheet == null)
                return Array.Empty<ProviderVehicleSheetRowData>();

            var range = worksheet.RangeUsed();
            if (range == null)
                return Array.Empty<ProviderVehicleSheetRowData>();

            var rows = new List<ProviderVehicleSheetRowData>();
            foreach (var row in range.RowsUsed().Where(r => r.RowNumber() >= startRow))
            {
                var rowData = new ProviderVehicleSheetRowData
                {
                    RowNumber = row.RowNumber(),
                    ProviderEmail = GetCellString(row, ProviderVehicleExcelColumns.ProviderEmail),
                    Name = GetCellString(row, ProviderVehicleExcelColumns.Name),
                    LicenseNumber = GetCellString(row, ProviderVehicleExcelColumns.LicenseNumber),
                    Capacity = GetCellInt(row, ProviderVehicleExcelColumns.Capacity),
                    Weight = GetCellDouble(row, ProviderVehicleExcelColumns.Weight),
                    VehicleRegistration = GetCellString(row, ProviderVehicleExcelColumns.VehicleRegistration),
                    VehicleRegistrationExp = GetCellDate(row, ProviderVehicleExcelColumns.VehicleRegistrationExp),
                    VehicleInsurance = GetCellString(row, ProviderVehicleExcelColumns.VehicleInsurance),
                    VehicleInsuranceExp = GetCellDate(row, ProviderVehicleExcelColumns.VehicleInsuranceExp),
                    InspectionReport = GetCellString(row, ProviderVehicleExcelColumns.InspectionReport),
                    InspectionReportExp = GetCellDate(row, ProviderVehicleExcelColumns.InspectionReportExp),
                    Picture = GetCellString(row, ProviderVehicleExcelColumns.Picture),
                    MeasurementCertificate = GetCellString(row, ProviderVehicleExcelColumns.MeasurementCertificate),
                    PeriodicVehicleInspections = GetCellString(row, ProviderVehicleExcelColumns.PeriodicVehicleInspections),
                    PeriodicVehicleInspectionsExp = GetCellDate(row, ProviderVehicleExcelColumns.PeriodicVehicleInspectionsExp),
                    VehicleCompartments = GetCellString(row, ProviderVehicleExcelColumns.VehicleCompartments),
                    VehicleServices = GetCellString(row, ProviderVehicleExcelColumns.VehicleServices)
                };

                if (!rowData.IsEmpty)
                    rows.Add(rowData);
            }

            return rows;
        }

        private static void WriteDriverWorksheet(XLWorkbook workbook, IReadOnlyList<ProviderDriverExcelRowData> rows)
        {
            var worksheet = workbook.Worksheets.Add(DriversWorksheetName);
            worksheet.Cell(1, ProviderDriverExcelColumns.ProviderEmail).Value = "ProviderEmail";
            worksheet.Cell(1, ProviderDriverExcelColumns.FirstName).Value = "FirstName";
            worksheet.Cell(1, ProviderDriverExcelColumns.LastName).Value = "LastName";
            worksheet.Cell(1, ProviderDriverExcelColumns.Email).Value = "Email";
            worksheet.Cell(1, ProviderDriverExcelColumns.IsDefault).Value = "IsDefault";
            worksheet.Cell(1, ProviderDriverExcelColumns.License).Value = "License";
            worksheet.Cell(1, ProviderDriverExcelColumns.LicenseExp).Value = "LicenseExp";
            worksheet.Cell(1, ProviderDriverExcelColumns.BackgroundCheck).Value = "BackgroundCheck";
            worksheet.Cell(1, ProviderDriverExcelColumns.BackgroundCheckExp).Value = "BackgroundCheckExp";
            worksheet.Cell(1, ProviderDriverExcelColumns.ProfilePhoto).Value = "ProfilePhoto";
            worksheet.Cell(1, ProviderDriverExcelColumns.Status).Value = "Status";
            worksheet.Cell(1, ProviderDriverExcelColumns.Password).Value = "Password";

            var rowIndex = 2;
            foreach (var row in rows)
            {
                worksheet.Cell(rowIndex, ProviderDriverExcelColumns.ProviderEmail).Value = row.ProviderEmail ?? string.Empty;
                worksheet.Cell(rowIndex, ProviderDriverExcelColumns.FirstName).Value = row.FirstName ?? string.Empty;
                worksheet.Cell(rowIndex, ProviderDriverExcelColumns.LastName).Value = row.LastName ?? string.Empty;
                worksheet.Cell(rowIndex, ProviderDriverExcelColumns.Email).Value = row.Email ?? string.Empty;
                worksheet.Cell(rowIndex, ProviderDriverExcelColumns.IsDefault).Value = row.IsDefault ?? false;
                worksheet.Cell(rowIndex, ProviderDriverExcelColumns.License).Value = row.License ?? string.Empty;
                worksheet.Cell(rowIndex, ProviderDriverExcelColumns.LicenseExp).Value = row.LicenseExp;
                worksheet.Cell(rowIndex, ProviderDriverExcelColumns.BackgroundCheck).Value = row.BackgroundCheck ?? string.Empty;
                worksheet.Cell(rowIndex, ProviderDriverExcelColumns.BackgroundCheckExp).Value = row.BackgroundCheckExp;
                worksheet.Cell(rowIndex, ProviderDriverExcelColumns.ProfilePhoto).Value = row.ProfilePhoto ?? string.Empty;
                worksheet.Cell(rowIndex, ProviderDriverExcelColumns.Status).Value = row.Status ?? string.Empty;
                worksheet.Cell(rowIndex, ProviderDriverExcelColumns.Password).Value = string.Empty;
                rowIndex++;
            }

            var headerRange = worksheet.Range(1, 1, 1, ProviderDriverExcelColumns.Password);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
            worksheet.Columns().AdjustToContents();
            worksheet.SheetView.FreezeRows(1);
        }

        private static void WriteVehicleWorksheet(XLWorkbook workbook, IReadOnlyList<ProviderVehicleExcelRowData> rows)
        {
            var worksheet = workbook.Worksheets.Add(VehiclesWorksheetName);
            worksheet.Cell(1, ProviderVehicleExcelColumns.ProviderEmail).Value = "ProviderEmail";
            worksheet.Cell(1, ProviderVehicleExcelColumns.Name).Value = "Name";
            worksheet.Cell(1, ProviderVehicleExcelColumns.LicenseNumber).Value = "LicenseNumber";
            worksheet.Cell(1, ProviderVehicleExcelColumns.Capacity).Value = "Capacity";
            worksheet.Cell(1, ProviderVehicleExcelColumns.Weight).Value = "Weight";
            worksheet.Cell(1, ProviderVehicleExcelColumns.VehicleRegistration).Value = "VehicleRegistration";
            worksheet.Cell(1, ProviderVehicleExcelColumns.VehicleRegistrationExp).Value = "VehicleRegistrationExp";
            worksheet.Cell(1, ProviderVehicleExcelColumns.VehicleInsurance).Value = "VehicleInsurance";
            worksheet.Cell(1, ProviderVehicleExcelColumns.VehicleInsuranceExp).Value = "VehicleInsuranceExp";
            worksheet.Cell(1, ProviderVehicleExcelColumns.InspectionReport).Value = "InspectionReport";
            worksheet.Cell(1, ProviderVehicleExcelColumns.InspectionReportExp).Value = "InspectionReportExp";
            worksheet.Cell(1, ProviderVehicleExcelColumns.Picture).Value = "Picture";
            worksheet.Cell(1, ProviderVehicleExcelColumns.MeasurementCertificate).Value = "MeasurementCertificate";
            worksheet.Cell(1, ProviderVehicleExcelColumns.PeriodicVehicleInspections).Value = "PeriodicVehicleInspections";
            worksheet.Cell(1, ProviderVehicleExcelColumns.PeriodicVehicleInspectionsExp).Value = "PeriodicVehicleInspectionsExp";
            worksheet.Cell(1, ProviderVehicleExcelColumns.VehicleCompartments).Value = "VehicleCompartments";
            worksheet.Cell(1, ProviderVehicleExcelColumns.VehicleServices).Value = "VehicleServices";

            var rowIndex = 2;
            foreach (var row in rows)
            {
                worksheet.Cell(rowIndex, ProviderVehicleExcelColumns.ProviderEmail).Value = row.ProviderEmail ?? string.Empty;
                worksheet.Cell(rowIndex, ProviderVehicleExcelColumns.Name).Value = row.Name ?? string.Empty;
                worksheet.Cell(rowIndex, ProviderVehicleExcelColumns.LicenseNumber).Value = row.LicenseNumber ?? string.Empty;
                worksheet.Cell(rowIndex, ProviderVehicleExcelColumns.Capacity).Value = row.Capacity;
                worksheet.Cell(rowIndex, ProviderVehicleExcelColumns.Weight).Value = row.Weight;
                worksheet.Cell(rowIndex, ProviderVehicleExcelColumns.VehicleRegistration).Value = row.VehicleRegistration ?? string.Empty;
                worksheet.Cell(rowIndex, ProviderVehicleExcelColumns.VehicleRegistrationExp).Value = row.VehicleRegistrationExp;
                worksheet.Cell(rowIndex, ProviderVehicleExcelColumns.VehicleInsurance).Value = row.VehicleInsurance ?? string.Empty;
                worksheet.Cell(rowIndex, ProviderVehicleExcelColumns.VehicleInsuranceExp).Value = row.VehicleInsuranceExp;
                worksheet.Cell(rowIndex, ProviderVehicleExcelColumns.InspectionReport).Value = row.InspectionReport ?? string.Empty;
                worksheet.Cell(rowIndex, ProviderVehicleExcelColumns.InspectionReportExp).Value = row.InspectionReportExp;
                worksheet.Cell(rowIndex, ProviderVehicleExcelColumns.Picture).Value = row.Picture ?? string.Empty;
                worksheet.Cell(rowIndex, ProviderVehicleExcelColumns.MeasurementCertificate).Value = row.MeasurementCertificate ?? string.Empty;
                worksheet.Cell(rowIndex, ProviderVehicleExcelColumns.PeriodicVehicleInspections).Value = row.PeriodicVehicleInspections ?? string.Empty;
                worksheet.Cell(rowIndex, ProviderVehicleExcelColumns.PeriodicVehicleInspectionsExp).Value = row.PeriodicVehicleInspectionsExp;
                worksheet.Cell(rowIndex, ProviderVehicleExcelColumns.VehicleCompartments).Value = row.VehicleCompartments ?? string.Empty;
                worksheet.Cell(rowIndex, ProviderVehicleExcelColumns.VehicleServices).Value = row.VehicleServices ?? string.Empty;
                rowIndex++;
            }

            var headerRange = worksheet.Range(1, 1, 1, ProviderVehicleExcelColumns.VehicleServices);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
            worksheet.Columns().AdjustToContents();
            worksheet.SheetView.FreezeRows(1);
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

        private static bool? GetCellBool(IXLRangeRow row, int columnIndex)
        {
            var cell = row.Cell(columnIndex);
            if (cell == null)
                return null;

            if (cell.TryGetValue<bool>(out var value))
                return value;

            var text = cell.GetValue<string>()?.Trim();
            if (string.IsNullOrWhiteSpace(text))
                return null;

            if (bool.TryParse(text, out value))
                return value;

            if (int.TryParse(text, out var number))
                return number != 0;

            return text.Equals("yes", StringComparison.OrdinalIgnoreCase)
                ? true
                : text.Equals("no", StringComparison.OrdinalIgnoreCase)
                    ? false
                    : null;
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

    public class ProviderExcelWorkbookData
    {
        public IReadOnlyList<ProviderExcelRowData> Providers { get; set; } = Array.Empty<ProviderExcelRowData>();
        public IReadOnlyList<ProviderDriverSheetRowData> Drivers { get; set; } = Array.Empty<ProviderDriverSheetRowData>();
        public IReadOnlyList<ProviderVehicleSheetRowData> Vehicles { get; set; } = Array.Empty<ProviderVehicleSheetRowData>();
    }

    public static class ProviderDriverExcelColumns
    {
        public const int ProviderEmail = 1;
        public const int FirstName = 2;
        public const int LastName = 3;
        public const int Email = 4;
        public const int IsDefault = 5;
        public const int License = 6;
        public const int LicenseExp = 7;
        public const int BackgroundCheck = 8;
        public const int BackgroundCheckExp = 9;
        public const int ProfilePhoto = 10;
        public const int Status = 11;
        public const int Password = 12;
    }

    public class ProviderDriverSheetRowData
    {
        public int RowNumber { get; set; }
        public string? ProviderEmail { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public bool? IsDefault { get; set; }
        public string? License { get; set; }
        public DateTime? LicenseExp { get; set; }
        public string? BackgroundCheck { get; set; }
        public DateTime? BackgroundCheckExp { get; set; }
        public string? ProfilePhoto { get; set; }
        public string? Status { get; set; }
        public string? Password { get; set; }

        public bool IsEmpty =>
            string.IsNullOrWhiteSpace(ProviderEmail) &&
            string.IsNullOrWhiteSpace(FirstName) &&
            string.IsNullOrWhiteSpace(LastName) &&
            string.IsNullOrWhiteSpace(Email) &&
            !IsDefault.HasValue &&
            string.IsNullOrWhiteSpace(License) &&
            !LicenseExp.HasValue &&
            string.IsNullOrWhiteSpace(BackgroundCheck) &&
            !BackgroundCheckExp.HasValue &&
            string.IsNullOrWhiteSpace(ProfilePhoto) &&
            string.IsNullOrWhiteSpace(Status) &&
            string.IsNullOrWhiteSpace(Password);
    }

    public class ProviderDriverExcelRowData : ProviderDriverSheetRowData
    {
    }

    public static class ProviderVehicleExcelColumns
    {
        public const int ProviderEmail = 1;
        public const int Name = 2;
        public const int LicenseNumber = 3;
        public const int Capacity = 4;
        public const int Weight = 5;
        public const int VehicleRegistration = 6;
        public const int VehicleRegistrationExp = 7;
        public const int VehicleInsurance = 8;
        public const int VehicleInsuranceExp = 9;
        public const int InspectionReport = 10;
        public const int InspectionReportExp = 11;
        public const int Picture = 12;
        public const int MeasurementCertificate = 13;
        public const int PeriodicVehicleInspections = 14;
        public const int PeriodicVehicleInspectionsExp = 15;
        public const int VehicleCompartments = 16;
        public const int VehicleServices = 17;
    }

    public class ProviderVehicleSheetRowData
    {
        public int RowNumber { get; set; }
        public string? ProviderEmail { get; set; }
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
            string.IsNullOrWhiteSpace(ProviderEmail) &&
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

    public class ProviderVehicleExcelRowData : ProviderVehicleSheetRowData
    {
    }
}
