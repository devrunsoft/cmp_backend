using System;
namespace CMPNatural.Application.Hub
{
	public class LocationPayload
	{
        public double lat { get; set; }
        public double lng { get; set; }
        public string PersonId { get; set; } = string.Empty;
        public long ProviderId { get; set; } = 0;
    }
}

