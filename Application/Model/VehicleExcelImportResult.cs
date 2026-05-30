using System.Collections.Generic;

namespace CMPNatural.Application.Model
{
    public class VehicleExcelImportResult
    {
        public int TotalRows { get; set; }
        public int ImportedRows { get; set; }
        public int CreatedRows { get; set; }
        public int UpdatedRows { get; set; }
        public VehicleExcelColumnMap ColumnMap { get; set; } = new VehicleExcelColumnMap();
        public List<VehicleExcelImportRowResult> Rows { get; set; } = new List<VehicleExcelImportRowResult>();
    }

    public class VehicleExcelImportRowResult
    {
        public int RowNumber { get; set; }
        public long? VehicleId { get; set; }
        public string? LicenseNumber { get; set; }
        public string? Action { get; set; }
        public List<string> MissingFields { get; set; } = new List<string>();
        public string? Error { get; set; }
    }

    public class VehicleExcelColumnMap
    {
        public VehicleExcelColumnInfo Name { get; set; } = new VehicleExcelColumnInfo(1, "A");
        public VehicleExcelColumnInfo LicenseNumber { get; set; } = new VehicleExcelColumnInfo(2, "B");
        public VehicleExcelColumnInfo Capacity { get; set; } = new VehicleExcelColumnInfo(3, "C");
        public VehicleExcelColumnInfo Weight { get; set; } = new VehicleExcelColumnInfo(4, "D");
        public VehicleExcelColumnInfo VehicleRegistration { get; set; } = new VehicleExcelColumnInfo(5, "E");
        public VehicleExcelColumnInfo VehicleRegistrationExp { get; set; } = new VehicleExcelColumnInfo(6, "F");
        public VehicleExcelColumnInfo VehicleInsurance { get; set; } = new VehicleExcelColumnInfo(7, "G");
        public VehicleExcelColumnInfo VehicleInsuranceExp { get; set; } = new VehicleExcelColumnInfo(8, "H");
        public VehicleExcelColumnInfo InspectionReport { get; set; } = new VehicleExcelColumnInfo(9, "I");
        public VehicleExcelColumnInfo InspectionReportExp { get; set; } = new VehicleExcelColumnInfo(10, "J");
        public VehicleExcelColumnInfo Picture { get; set; } = new VehicleExcelColumnInfo(11, "K");
        public VehicleExcelColumnInfo MeasurementCertificate { get; set; } = new VehicleExcelColumnInfo(12, "L");
        public VehicleExcelColumnInfo PeriodicVehicleInspections { get; set; } = new VehicleExcelColumnInfo(13, "M");
        public VehicleExcelColumnInfo PeriodicVehicleInspectionsExp { get; set; } = new VehicleExcelColumnInfo(14, "N");
        public VehicleExcelColumnInfo VehicleCompartments { get; set; } = new VehicleExcelColumnInfo(15, "O");
        public VehicleExcelColumnInfo VehicleServices { get; set; } = new VehicleExcelColumnInfo(16, "P");
    }

    public class VehicleExcelColumnInfo
    {
        public VehicleExcelColumnInfo()
        {
        }

        public VehicleExcelColumnInfo(int index, string letter)
        {
            Index = index;
            Letter = letter;
        }

        public int Index { get; set; }
        public string Letter { get; set; } = string.Empty;
    }
}
