using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CMPNatural.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PaymentHistoryStatus
    {
        [Description("Draft")]
        Draft,

        [Description("Partially_paid")]
        Partially_paid,

        [Description("Paid")]
        Paid,

        [Description("Canceled")]
        Canceled,

        [Description("Manual_Paid")]
        Manual_Paid,
    }
}

