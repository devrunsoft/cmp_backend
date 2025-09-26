using System;
using CMPNatural.Core.Entities;
using System.Collections.Generic;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application.Responses.Driver
{
	public class RouteDateResponse
	{
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public List<RouteLocationResponse> Routes { get; set; } = new List<RouteLocationResponse>();
    }

    public class RouteLocationResponse
    {
        public long Id { get; set; }
        public string Address { get; set; }
        public int Capacity { get; set; }
        public string PrimaryFirstName { get; set; } = string.Empty;
        public string PrimaryLastName { get; set; } = string.Empty;
        public string PrimaryPhonNumber { get; set; } = string.Empty;

        public double Lat { get; set; }
        public double Lng { get; set; }

        public string ManifestNumber { get; set; }
        public string ServiceType { get; set; }
        //
        public bool IsEmegency { get; set; }
        public string ProductName { get; set; }
        public string ProductPriceName { get; set; }

    }
}

