using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CMPNatural.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ServiceProviderStatus
    {
        [Description("Assigned")]
        Assigned = 1,

        [Description("Proccessing")]
        Proccessing = 2,

    }
}

