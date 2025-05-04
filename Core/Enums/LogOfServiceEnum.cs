using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CMPNatural.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum LogOfServiceEnum
    {
        [Description("Next_30_days")]
        Next_30_days,

        [Description("All_upcoming_services")]
        All_upcoming_services,

        [Description("All_past_services")]
        All_past_services,
    }
}

