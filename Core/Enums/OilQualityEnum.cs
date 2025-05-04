using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CMPNatural.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OilQualityEnum
    {
        [Description("Dirty")]
        Dirty,

        [Description("Semi_dirty")]
        Semi_dirty,

        [Description("Clear")]
        Clear,

    }
}

