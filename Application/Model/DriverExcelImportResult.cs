using System.Collections.Generic;

namespace CMPNatural.Application.Model
{
    public class DriverExcelImportResult
    {
        public int TotalRows { get; set; }
        public int ImportedRows { get; set; }
        public int CreatedRows { get; set; }
        public int UpdatedRows { get; set; }
        public DriverExcelColumnMap ColumnMap { get; set; } = new DriverExcelColumnMap();
        public List<DriverExcelImportRowResult> Rows { get; set; } = new List<DriverExcelImportRowResult>();
    }

    public class DriverExcelImportRowResult
    {
        public int RowNumber { get; set; }
        public long? DriverId { get; set; }
        public string? Email { get; set; }
        public string? Action { get; set; }
        public List<string> MissingFields { get; set; } = new List<string>();
        public string? Error { get; set; }
    }

    public class DriverExcelColumnMap
    {
        public DriverExcelColumnInfo FirstName { get; set; } = new DriverExcelColumnInfo(1, "A");
        public DriverExcelColumnInfo LastName { get; set; } = new DriverExcelColumnInfo(2, "B");
        public DriverExcelColumnInfo Email { get; set; } = new DriverExcelColumnInfo(3, "C");
        public DriverExcelColumnInfo IsDefault { get; set; } = new DriverExcelColumnInfo(4, "D");
        public DriverExcelColumnInfo License { get; set; } = new DriverExcelColumnInfo(5, "E");
        public DriverExcelColumnInfo LicenseExp { get; set; } = new DriverExcelColumnInfo(6, "F");
        public DriverExcelColumnInfo BackgroundCheck { get; set; } = new DriverExcelColumnInfo(7, "G");
        public DriverExcelColumnInfo BackgroundCheckExp { get; set; } = new DriverExcelColumnInfo(8, "H");
        public DriverExcelColumnInfo ProfilePhoto { get; set; } = new DriverExcelColumnInfo(9, "I");
        public DriverExcelColumnInfo Status { get; set; } = new DriverExcelColumnInfo(10, "J");
        public DriverExcelColumnInfo Password { get; set; } = new DriverExcelColumnInfo(11, "K");
    }

    public class DriverExcelColumnInfo
    {
        public DriverExcelColumnInfo()
        {
        }

        public DriverExcelColumnInfo(int index, string letter)
        {
            Index = index;
            Letter = letter;
        }

        public int Index { get; set; }
        public string Letter { get; set; } = string.Empty;
    }
}
