using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CMPNatural.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RouteStatus
    {
        [Description("Draft")]
        Draft,

        [Description("InProcess")]
        InProcess,

        [Description("Partially_Completed")]
        Partially_Completed,

        [Description("Completed")]
        Completed,

        [Description("Canceled")]
        Canceled
    }
}

