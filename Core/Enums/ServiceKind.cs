using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CMPNatural.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ServiceKind
    {
        [Description("Custom")]
        Custom = 1,

        [Description("Emergency")]
        Emergency = 2,
    }
}

