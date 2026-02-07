using System.Collections.Generic;

namespace CMPNatural.Application.Model
{
    public class CompanyExcelImportResult
    {
        public int TotalRows { get; set; }
        public int ImportedRows { get; set; }
        public CompanyExcelColumnMap ColumnMap { get; set; } = new CompanyExcelColumnMap();
        public List<CompanyExcelImportRowResult> Rows { get; set; } = new List<CompanyExcelImportRowResult>();
    }

    public class CompanyExcelImportRowResult
    {
        public int RowNumber { get; set; }
        public string? CompanyName { get; set; }
        public string? OperationalUsername { get; set; }
        public long? CompanyId { get; set; }
        public long? OperationalAddressId { get; set; }
        public List<string> MissingFields { get; set; } = new List<string>();
        public string? Error { get; set; }
    }

    public class CompanyExcelColumnMap
    {
        public CompanyExcelColumnInfo OperationalUsername { get; set; } = new CompanyExcelColumnInfo(1, "A");
        public CompanyExcelColumnInfo CompanyCreatedAt { get; set; } = new CompanyExcelColumnInfo(2, "B");
        public CompanyExcelColumnInfo CompanyName { get; set; } = new CompanyExcelColumnInfo(4, "D");
        public CompanyExcelColumnInfo Street { get; set; } = new CompanyExcelColumnInfo(5, "E");
        public CompanyExcelColumnInfo City { get; set; } = new CompanyExcelColumnInfo(6, "F");
        public CompanyExcelColumnInfo State { get; set; } = new CompanyExcelColumnInfo(7, "G");
        public CompanyExcelColumnInfo Zip { get; set; } = new CompanyExcelColumnInfo(8, "H");
        public CompanyExcelColumnInfo Phone { get; set; } = new CompanyExcelColumnInfo(9, "I");
    }

    public class CompanyExcelColumnInfo
    {
        public CompanyExcelColumnInfo()
        {
        }

        public CompanyExcelColumnInfo(int index, string letter)
        {
            Index = index;
            Letter = letter;
        }

        public int Index { get; set; }
        public string Letter { get; set; } = string.Empty;
    }
}
