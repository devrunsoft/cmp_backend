using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CMPNatural.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PaymentStatus
    {
        [Description("Draft")]
        Draft,
        //force pay
        [Description("Sent_For_Pay")]
        Sent_For_Pay,

        //optional pay
        [Description("Ready_For_Pay")]
        Ready_For_Pay,

        [Description("Partially_paid")]
        Partially_paid,

        [Description("Paid")]
        Paid,

        [Description("Manual_Paid")]
        Manual_Paid,
    }
}

