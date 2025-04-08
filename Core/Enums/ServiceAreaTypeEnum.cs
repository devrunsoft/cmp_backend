using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CMPNatural.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ServiceAreaTypeEnum
    {
        [Description("Circle")]
        Circle=1,

        [Description("Polygon")]
        Polygon=2,
    }
}

