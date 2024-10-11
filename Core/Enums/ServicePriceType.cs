using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CMPNatural.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ServicePriceType
    {
        [Description("one_time")]
        one_time = 1,

        [Description("recurring")]
        recurring = 2,

    }
}


