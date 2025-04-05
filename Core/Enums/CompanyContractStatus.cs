using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CMPNatural.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CompanyContractStatus
    {
        [Description("Created")]
        Created = 1,

        [Description("Visit")]
        Visit = 2,

        [Description("Signed")]
        Signed = 3,

        [Description("Needs_Admin_Signature")]
        Needs_Admin_Signature = 4,

        [Description("Canceld")]
        Canceld = 5,

    }
}

