using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CMPNatural.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum FrequencyType
    {
        [Description("1x yr")]
        x1_yr = 1,

        [Description("2x yr")]
        x2_yr = 2,

        [Description("3x yr")]
        x3_yr = 3,

        [Description("4x yr")]
        x4_yr = 4,

        [Description("6x yr")]
        x6_yr = 6,

        [Description("8x yr")]
        x8_yr = 8,

        [Description("12x yr")]
        x12_yr = 12,

        [Description("24x yr")]
        x24_yr = 24,

        [Description("26x yr")]
        x26_yr = 26,

        [Description("52x yr")]
        x52_yr = 52,

    }
}

