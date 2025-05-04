using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CMPNatural.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum InvoiceStatus
    {
        [Description("Draft")]
        Draft,

        [Description("Pending_Signature")]
        Pending_Signature,

        [Description("Needs_Admin_Signature")]
        Needs_Admin_Signature,

        [Description("Needs_Assignment")]
        Needs_Assignment,

        [Description("Processing_Provider")]
        Processing_Provider,

        [Description("Complete")]
        Complete,

        [Description("Canceled")]
        Canceled,

        [Description("Updated_Provider")]
        Updated_Provider,

        [Description("Submited_Provider")]
        Submited_Provider,

        [Description("Send_Payment")]
        Send_Payment,

        [Description("Deleted")]
        Deleted,

        [Description("Scaduled")]
        Scaduled,

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


