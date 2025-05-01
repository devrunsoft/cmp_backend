using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CMPNatural.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ServiceStatus
    {
        [Description("Draft")]
        Draft,

        [Description("ShouldPay")]
        ShouldPay,

        [Description("Proccessing")]
        Proccessing,

        [Description("Complete")]
        Complete,

        [Description("Canceled")]
        Canceled,

        [Description("Scaduled")]
        Scaduled,

        [Description("Updated_Provider")]
        Updated_Provider,

        [Description("Submited_Provider")]
        Submited_Provider,

    }
}

