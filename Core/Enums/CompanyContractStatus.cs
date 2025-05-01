using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CMPNatural.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CompanyContractStatus
    {
        [Description("Created")]
        Created,

        [Description("Send")]
        Send,

        [Description("Visit")]
        Visit,

        [Description("Signed")]
        Signed,

        [Description("Needs_Admin_Signature")]
        Needs_Admin_Signature,

        [Description("Canceld")]
        Canceld

    }
}

