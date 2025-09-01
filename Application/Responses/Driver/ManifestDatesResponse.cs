using System;
using CMPNatural.Core.Entities;
using System.Collections.Generic;

namespace CMPNatural.Application.Responses.Driver
{
	public class ManifestDatesResponse  
	{
        //public long Id { get; set; }
        public DateTime Date { get; set; }
        public int Count { get; set; }
        public List<DriverManifest> DriverManifests { get; set; }
    }
}

