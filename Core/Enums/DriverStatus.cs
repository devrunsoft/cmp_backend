using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CMPNatural.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DriverStatus
    {
        [Description("Pending")]
        Pending = 1,
        [Description("Approved")]
        Approved = 2,
        [Description("Blocked")]
        Blocked = 3,
    }

}   