namespace CMPNatural.Application.Model
{
    public class DriverExcelFileResult
    {
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        public byte[] Content { get; set; } = System.Array.Empty<byte>();
    }
}
