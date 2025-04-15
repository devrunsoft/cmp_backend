using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CMPNatural.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ServiceStatus
    {
        [Description("Draft")]
        Draft = 1,

        [Description("ShouldPay")]
        ShouldPay = 2,

        [Description("Proccessing")]
        Proccessing = 3,

        [Description("Complete")]
        Complete = 4,

        [Description("Canceled")]
        Canceled = 5,

        [Description("Scaduled")]
        Scaduled = 6,

        [Description("Updated_Provider")]
        Updated_Provider = 7,

        [Description("Submited_Provider")]
        Submited_Provider = 8,

    }
}

