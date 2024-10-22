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

        [Description("sent")]
        sent = 2,

        [Description("payment_processing")]
        payment_processing = 3,

        [Description("paid")]
        paid = 4,

        [Description("partially_paid")]
        partially_paid = 5,

    }
}
