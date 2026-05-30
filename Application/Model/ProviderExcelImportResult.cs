using System.Collections.Generic;

namespace CMPNatural.Application.Model
{
    public class ProviderExcelImportResult
    {
        public int TotalRows { get; set; }
        public int ImportedRows { get; set; }
        public int CreatedRows { get; set; }
        public int UpdatedRows { get; set; }
        public ProviderExcelColumnMap ColumnMap { get; set; } = new ProviderExcelColumnMap();
        public List<ProviderExcelImportRowResult> Rows { get; set; } = new List<ProviderExcelImportRowResult>();
        public DriverExcelImportResult Drivers { get; set; } = new DriverExcelImportResult();
        public VehicleExcelImportResult Vehicles { get; set; } = new VehicleExcelImportResult();
    }

    public class ProviderExcelImportRowResult
    {
        public int RowNumber { get; set; }
        public long? ProviderId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Action { get; set; }
        public List<string> MissingFields { get; set; } = new List<string>();
        public string? Error { get; set; }
    }

    public class ProviderExcelColumnMap
    {
        public ProviderExcelColumnInfo Name { get; set; } = new ProviderExcelColumnInfo(1, "A");
        public ProviderExcelColumnInfo Email { get; set; } = new ProviderExcelColumnInfo(2, "B");
        public ProviderExcelColumnInfo PhoneNumber { get; set; } = new ProviderExcelColumnInfo(3, "C");
        public ProviderExcelColumnInfo Status { get; set; } = new ProviderExcelColumnInfo(4, "D");
        public ProviderExcelColumnInfo City { get; set; } = new ProviderExcelColumnInfo(5, "E");
        public ProviderExcelColumnInfo Address { get; set; } = new ProviderExcelColumnInfo(6, "F");
        public ProviderExcelColumnInfo County { get; set; } = new ProviderExcelColumnInfo(7, "G");
        public ProviderExcelColumnInfo Lat { get; set; } = new ProviderExcelColumnInfo(8, "H");
        public ProviderExcelColumnInfo Long { get; set; } = new ProviderExcelColumnInfo(9, "I");
        public ProviderExcelColumnInfo AreaLocation { get; set; } = new ProviderExcelColumnInfo(10, "J");
        public ProviderExcelColumnInfo Rating { get; set; } = new ProviderExcelColumnInfo(11, "K");
        public ProviderExcelColumnInfo ManagerFirstName { get; set; } = new ProviderExcelColumnInfo(12, "L");
        public ProviderExcelColumnInfo ManagerLastName { get; set; } = new ProviderExcelColumnInfo(13, "M");
        public ProviderExcelColumnInfo ManagerPhoneNumber { get; set; } = new ProviderExcelColumnInfo(14, "N");
        public ProviderExcelColumnInfo ProductIds { get; set; } = new ProviderExcelColumnInfo(15, "O");
        public ProviderExcelColumnInfo ProductNames { get; set; } = new ProviderExcelColumnInfo(16, "P");
        public ProviderExcelColumnInfo Password { get; set; } = new ProviderExcelColumnInfo(17, "Q");
    }

    public class ProviderExcelColumnInfo
    {
        public ProviderExcelColumnInfo()
        {
        }

        public ProviderExcelColumnInfo(int index, string letter)
        {
            Index = index;
            Letter = letter;
        }

        public int Index { get; set; }
        public string Letter { get; set; } = string.Empty;
    }
}
