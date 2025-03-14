using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CMPNatural.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum InvoiceStatus
    {
        [Description("Draft")]
        Draft = 1,

        [Description("Pending_Signature")]
        Pending_Signature = 2,

        [Description("Pending")]
        Pending = 3,

        [Description("Needs_Assignment")]
        Needs_Assignment = 4,

        [Description("Processing_Provider")]
        Processing_Provider = 5,

        [Description("Complete")]
        Complete = 6,

        [Description("Canceled")]
        Canceled = 7,

    }
}

