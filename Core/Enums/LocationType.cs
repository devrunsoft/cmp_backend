using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CMPNatural.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum LocationType
	{
        [Description("OilLocation")]
        OilLocation = 1,

        [Description("GreaseLocation")]
        GreaseLocation = 2,

    }
}

