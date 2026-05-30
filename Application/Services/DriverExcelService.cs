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
    public static class DriverExcelService
    {
        public static IReadOnlyList<DriverExcelRowData> Parse(IFormFile file, int startRow, string? worksheetName)
        {
            if (file == null || file.Length == 0)
                return Array.Empty<DriverExcelRowData>();

            var extension = Path.GetExtension(file.FileName)?.ToLowerInvariant();
            if (extension == ".csv")
                return ParseCsv(file, startRow);

            using var stream = file.OpenReadStream();
            using var workbook = new XLWorkbook(stream);
            var worksheet = !string.IsNullOrWhiteSpace(worksheetName)
                ? workbook.Worksheets.FirstOrDefault(w => w.Name.Equals(worksheetName, StringComparison.OrdinalIgnoreCase))
                : workbook.Worksheets.FirstOrDefault();

            if (worksheet == null)
                return Array.Empty<DriverExcelRowData>();

            var range = worksheet.RangeUsed();
            if (range == null)
                return Array.Empty<DriverExcelRowData>();

            var rows = new List<DriverExcelRowData>();
            foreach (var row in range.RowsUsed().Where(r => r.RowNumber() >= startRow))
            {
                var rowData = new DriverExcelRowData
                {
                    RowNumber = row.RowNumber(),
                    FirstName = GetCellString(row, DriverExcelColumns.FirstName),
                    LastName = GetCellString(row, DriverExcelColumns.LastName),
                    Email = GetCellString(row, DriverExcelColumns.Email),
                    IsDefault = GetCellBool(row, DriverExcelColumns.IsDefault),
                    License = GetCellString(row, DriverExcelColumns.License),
                    LicenseExp = GetCellDate(row, DriverExcelColumns.LicenseExp),
                    BackgroundCheck = GetCellString(row, DriverExcelColumns.BackgroundCheck),
                    BackgroundCheckExp = GetCellDate(row, DriverExcelColumns.BackgroundCheckExp),
                    ProfilePhoto = GetCellString(row, DriverExcelColumns.ProfilePhoto),
                    Status = GetCellString(row, DriverExcelColumns.Status),
                    Password = GetCellString(row, DriverExcelColumns.Password)
                };

                if (rowData.IsEmpty)
                    continue;

                rows.Add(rowData);
            }

            return rows;
        }

        public static DriverExcelFileResult Export(IReadOnlyList<DriverResponse> drivers)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Drivers");

            WriteHeaders(worksheet);

            var rowIndex = 2;
            foreach (var driver in drivers)
            {
                worksheet.Cell(rowIndex, DriverExcelColumns.FirstName).Value = driver.FirstName ?? string.Empty;
                worksheet.Cell(rowIndex, DriverExcelColumns.LastName).Value = driver.LastName ?? string.Empty;
                worksheet.Cell(rowIndex, DriverExcelColumns.Email).Value = driver.Email ?? string.Empty;
                worksheet.Cell(rowIndex, DriverExcelColumns.IsDefault).Value = driver.IsDefault;
                worksheet.Cell(rowIndex, DriverExcelColumns.License).Value = driver.License ?? string.Empty;
                worksheet.Cell(rowIndex, DriverExcelColumns.LicenseExp).Value = driver.LicenseExp;
                worksheet.Cell(rowIndex, DriverExcelColumns.BackgroundCheck).Value = driver.BackgroundCheck ?? string.Empty;
                worksheet.Cell(rowIndex, DriverExcelColumns.BackgroundCheckExp).Value = driver.BackgroundCheckExp;
                worksheet.Cell(rowIndex, DriverExcelColumns.ProfilePhoto).Value = driver.ProfilePhoto ?? string.Empty;
                worksheet.Cell(rowIndex, DriverExcelColumns.Status).Value = driver.Status.ToString();
                worksheet.Cell(rowIndex, DriverExcelColumns.Password).Value = string.Empty;
                rowIndex++;
            }

            worksheet.Columns().AdjustToContents();
            worksheet.SheetView.FreezeRows(1);

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return new DriverExcelFileResult
            {
                FileName = $"drivers-{DateTime.UtcNow:yyyyMMddHHmmss}.xlsx",
                Content = stream.ToArray()
            };
        }

        private static void WriteHeaders(IXLWorksheet worksheet)
        {
            worksheet.Cell(1, DriverExcelColumns.FirstName).Value = "FirstName";
            worksheet.Cell(1, DriverExcelColumns.LastName).Value = "LastName";
            worksheet.Cell(1, DriverExcelColumns.Email).Value = "Email";
            worksheet.Cell(1, DriverExcelColumns.IsDefault).Value = "IsDefault";
            worksheet.Cell(1, DriverExcelColumns.License).Value = "License";
            worksheet.Cell(1, DriverExcelColumns.LicenseExp).Value = "LicenseExp";
            worksheet.Cell(1, DriverExcelColumns.BackgroundCheck).Value = "BackgroundCheck";
            worksheet.Cell(1, DriverExcelColumns.BackgroundCheckExp).Value = "BackgroundCheckExp";
            worksheet.Cell(1, DriverExcelColumns.ProfilePhoto).Value = "ProfilePhoto";
            worksheet.Cell(1, DriverExcelColumns.Status).Value = "Status";
            worksheet.Cell(1, DriverExcelColumns.Password).Value = "Password";

            var headerRange = worksheet.Range(1, 1, 1, DriverExcelColumns.Password);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
        }

        private static IReadOnlyList<DriverExcelRowData> ParseCsv(IFormFile file, int startRow)
        {
            var rows = new List<DriverExcelRowData>();
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
                var rowData = new DriverExcelRowData
                {
                    RowNumber = rowNumber,
                    FirstName = GetCsvField(fields, DriverExcelColumns.FirstName),
                    LastName = GetCsvField(fields, DriverExcelColumns.LastName),
                    Email = GetCsvField(fields, DriverExcelColumns.Email),
                    IsDefault = GetCsvBool(fields, DriverExcelColumns.IsDefault),
                    License = GetCsvField(fields, DriverExcelColumns.License),
                    LicenseExp = GetCsvDate(fields, DriverExcelColumns.LicenseExp),
                    BackgroundCheck = GetCsvField(fields, DriverExcelColumns.BackgroundCheck),
                    BackgroundCheckExp = GetCsvDate(fields, DriverExcelColumns.BackgroundCheckExp),
                    ProfilePhoto = GetCsvField(fields, DriverExcelColumns.ProfilePhoto),
                    Status = GetCsvField(fields, DriverExcelColumns.Status),
                    Password = GetCsvField(fields, DriverExcelColumns.Password)
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

        private static bool? GetCellBool(IXLRangeRow row, int columnIndex)
        {
            var cell = row.Cell(columnIndex);
            if (cell == null)
                return null;

            if (cell.TryGetValue<bool>(out var value))
                return value;

            var text = cell.GetValue<string>()?.Trim();
            return ParseBool(text);
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

        private static bool? GetCsvBool(List<string> fields, int columnIndex)
        {
            return ParseBool(GetCsvField(fields, columnIndex));
        }

        private static bool? ParseBool(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            if (bool.TryParse(value, out var result))
                return result;

            if (int.TryParse(value, out var numeric))
                return numeric != 0;

            return value.Trim().Equals("yes", StringComparison.OrdinalIgnoreCase)
                ? true
                : value.Trim().Equals("no", StringComparison.OrdinalIgnoreCase)
                    ? false
                    : null;
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

    public static class DriverExcelColumns
    {
        public const int FirstName = 1;
        public const int LastName = 2;
        public const int Email = 3;
        public const int IsDefault = 4;
        public const int License = 5;
        public const int LicenseExp = 6;
        public const int BackgroundCheck = 7;
        public const int BackgroundCheckExp = 8;
        public const int ProfilePhoto = 9;
        public const int Status = 10;
        public const int Password = 11;
    }

    public class DriverExcelRowData
    {
        public int RowNumber { get; set; }
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
}
