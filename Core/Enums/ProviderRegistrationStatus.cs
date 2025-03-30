using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CMPNatural.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ProviderRegistrationStatus
    {
        [Description("Basic_Information")]
        Basic_Information,
        [Description("Billing_Details")]
        Billing_Details,
        [Description("Operational_Address")]
        Operational_Address,
        [Description("Document_Submission")]
        Document_Submission,
        [Description("Vehicle_Registration")]
        Vehicle_Registration,
        [Description("Complete")]
        Complete,

    }

}