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

        [Description("Needs_Admin_Signature")]
        Needs_Admin_Signature = 3,

        [Description("Needs_Assignment")]
        Needs_Assignment = 4,

        [Description("Processing_Provider")]
        Processing_Provider = 5,

        [Description("Complete")]
        Complete = 6,

        [Description("Canceled")]
        Canceled = 7,

        [Description("Updated_Provider")]
        Updated_Provider = 8,

        [Description("Submited_Provider")]
        Submited_Provider = 9,

        [Description("Send_Payment")]
        Send_Payment = 10,

        [Description("Deleted")]
        Deleted = 11,

    }

    public static class InvoiceStatusExtensions
    {
        //public static List<InvoiceStatus> invoiceSkipable { get {

        //        return new List<InvoiceStatus>()
        //        {
        //             InvoiceStatus.Draft,
        //             InvoiceStatus.Needs_Admin_Signature,
        //             InvoiceStatus.Pending_Signature,
        //             InvoiceStatus.Needs_Assignment,
        //             InvoiceStatus.Processing_Provider
        //    };
        //    } }
        public static bool IsSkippable(this InvoiceStatus status)
        {
            return status == InvoiceStatus.Draft ||
                   status == InvoiceStatus.Needs_Admin_Signature ||
                   status == InvoiceStatus.Pending_Signature ||
                   status == InvoiceStatus.Needs_Assignment ||
                   status == InvoiceStatus.Processing_Provider;
        }
    }
}


