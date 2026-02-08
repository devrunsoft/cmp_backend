using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using ClosedXML.Excel;
using CMPNatural.Application.Model;
using Microsoft.AspNetCore.Http;

namespace CMPNatural.Application.Services
{
    public static class CompanyExcelImportService
    {
        public static IReadOnlyList<CompanyExcelRowData> Parse(IFormFile file, int startRow, string? worksheetName)
        {
            if (file == null || file.Length == 0)
                return Array.Empty<CompanyExcelRowData>();

            var extension = Path.GetExtension(file.FileName)?.ToLowerInvariant();
            if (extension == ".csv")
                return ParseCsv(file, startRow);

            using var stream = file.OpenReadStream();
            using var workbook = new XLWorkbook(stream);
            var worksheet = !string.IsNullOrWhiteSpace(worksheetName)
                ? workbook.Worksheets.FirstOrDefault(w => w.Name.Equals(worksheetName, StringComparison.OrdinalIgnoreCase))
                : workbook.Worksheets.FirstOrDefault();

            if (worksheet == null)
                return Array.Empty<CompanyExcelRowData>();

            var range = worksheet.RangeUsed();
            if (range == null)
                return Array.Empty<CompanyExcelRowData>();

            var rows = new List<CompanyExcelRowData>();
            foreach (var row in range.RowsUsed().Where(r => r.RowNumber() >= startRow))
            {
                var rowData = new CompanyExcelRowData
                {
                    RowNumber = row.RowNumber(),
                    OperationalUsername = GetCellString(row, CompanyExcelColumns.OperationalUsername),
                    CompanyName = GetCellString(row, CompanyExcelColumns.CompanyName),
                    Street = GetCellString(row, CompanyExcelColumns.Street),
                    City = GetCellString(row, CompanyExcelColumns.City),
                    State = GetCellString(row, CompanyExcelColumns.State),
                    Zip = GetCellString(row, CompanyExcelColumns.Zip),
                    Phone = GetCellString(row, CompanyExcelColumns.Phone),
                    CompanyCreatedAt = GetCellDate(row, CompanyExcelColumns.CompanyCreatedAt)
                };

                if (rowData.IsEmpty)
                    continue;

                rows.Add(rowData);
            }

            return rows;
        }

        private static string? GetCellString(IXLRangeRow row, int columnIndex)
        {
            var cell = row.Cell(columnIndex);
            if (cell == null)
                return null;

            var value = cell.GetValue<string>()?.Trim();
            return string.IsNullOrWhiteSpace(value) ? null : value;
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

        private static IReadOnlyList<CompanyExcelRowData> ParseCsv(IFormFile file, int startRow)
        {
            var rows = new List<CompanyExcelRowData>();
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

                var rowData = new CompanyExcelRowData
                {
                    RowNumber = rowNumber,
                    OperationalUsername = GetCsvField(fields, CompanyExcelColumns.OperationalUsername),
                    CompanyName = GetCsvField(fields, CompanyExcelColumns.CompanyName),
                    Street = GetCsvField(fields, CompanyExcelColumns.Street),
                    City = GetCsvField(fields, CompanyExcelColumns.City),
                    State = GetCsvField(fields, CompanyExcelColumns.State),
                    Zip = GetCsvField(fields, CompanyExcelColumns.Zip),
                    Phone = GetCsvField(fields, CompanyExcelColumns.Phone),
                    CompanyCreatedAt = GetCsvDate(fields, CompanyExcelColumns.CompanyCreatedAt),
                    ContactPerson = GetCsvField(fields, CompanyExcelColumns.ContactPerson),
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

        private static DateTime? GetCsvDate(List<string> fields, int columnIndex)
        {
            var value = GetCsvField(fields, columnIndex);
            if (string.IsNullOrWhiteSpace(value))
                return null;

            if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var dateValue))
                return dateValue;

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

    public static class CompanyExcelColumns
    {
        public const int OperationalUsername = 1; // A
        public const int CompanyCreatedAt = 2; // B
        public const int CompanyName = 4; // D
        public const int Street = 5; // E
        public const int City = 6; // F
        public const int State = 7; // G
        public const int Zip = 8; // H
        public const int Phone = 9; // I
        public const int ContactPerson = 11; // I
    }

    public class CompanyExcelRowData
    {
        public int RowNumber { get; set; }
        public string? OperationalUsername { get; set; }
        public DateTime? CompanyCreatedAt { get; set; }
        public string? CompanyName { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
        public string? Phone { get; set; }
        public string? ContactPerson { get; set; }

        public bool IsEmpty =>
            string.IsNullOrWhiteSpace(OperationalUsername) &&
            string.IsNullOrWhiteSpace(CompanyName) &&
            string.IsNullOrWhiteSpace(Street) &&
            string.IsNullOrWhiteSpace(City) &&
            string.IsNullOrWhiteSpace(State) &&
            string.IsNullOrWhiteSpace(Zip) &&
            string.IsNullOrWhiteSpace(Phone);
    }
}
