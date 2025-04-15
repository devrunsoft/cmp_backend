using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CMPNatural.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PaymentStatus
    {
        [Description("Draft")]
        Draft = 1,

        //force pay
        [Description("Sent_For_Pay")]
        Sent_For_Pay = 2,

        //optional pay
        [Description("Ready_For_Pay")]
        Ready_For_Pay = 3,

        [Description("Partially_paid")]
        Partially_paid = 4,

        [Description("Paid")]
        Paid = 5,
    }
}

