using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CMPNatural.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum InvoiceStatus
    {
        [Description("Draft")]
        draft = 1,

        [Description("SentForPay")]
        SentForPay = 2,

        [Description("Processing")]
        Processing = 3,

//[Description("sentForcePay")]
//sentForcePay = 4,

        [Description("Payment_processing")]
        payment_processing = 5,

        [Description("Paid")]
        paid = 6,

        [Description("Partially_paid")]
        partially_paid = 7,

        [Description("Canceled")]
        canceled = 8,

        [Description("ProcessingProvider")]
        ProcessingProvider = 9,

        [Description("ProcessingPartialProvider")]
        ProcessingPartialProvider = 10,

        [Description("ProcessingSeprateProvider")]
        ProcessingSeprateProvider = 11,

        [Description("Complete")]
        Complete = 12,

        [Description("PartialComplete")]
        PartialComplete = 13,

    }
}

