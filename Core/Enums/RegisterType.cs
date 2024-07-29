using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CMPNatural.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RegisterType
    {
        [Description("NotRegistered")]
        NotRegistered = 1,
        [Description("ProfessionalInformation")]
        ProfessionalInformation = 2,
        [Description("DocumentSubmission")]
        DocumentSubmission = 3,
        [Description("BillingDetails")]
        BillingDetails = 4,
        [Description("Registered")]
        Registered = 5,

    }
}

