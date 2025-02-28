using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CMPNatural.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ServiceStatus
    {
        [Description("draft")]
        draft = 1,

        [Description("ShouldPay")]
        ShouldPay = 2,

        [Description("Proccessing")]
        Proccessing = 3,

        [Description("Complete")]
        Complete = 4,

        [Description("Canceled")]
        Canceled = 5,

    }
}

