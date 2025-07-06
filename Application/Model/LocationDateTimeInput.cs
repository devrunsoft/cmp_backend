namespace CMPNatural.Application.Model
{
	public class LocationDateTimeInput
	{
        public long? Id { get; set; }

        public string DayName { get; set; } = null!;

        public int FromTime { get; set; }

        public int ToTime { get; set; }
    }
}