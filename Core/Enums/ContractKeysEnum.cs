using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CMPNatural.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ContractKeysEnum
    {
        [Description("{ManagmentCompanyLogo}")]
        ManagmentCompanyLogo,

        [Description("{ManagmentCompanyName}")]
        ManagmentCompanyName,

        [Description("{ManagmentCompanyEmail}")]
        ManagmentCompanyEmail,

        [Description("{ManagmentCompanyPhoneNumber}")]
        ManagmentCompanyPhoneNumber,

        [Description("{ManagmentCompanyFirstName}")]
        ManagmentCompanyFirstName,

        [Description("{ManagmentCompanyLastName}")]
        ManagmentCompanyLastName,

        [Description("{ManagmentCompanyAddress}")]
        ManagmentCompanyAddress,

        [Description("{ManagmentCompanySign}")]
        ManagmentCompanySign,

        /////////////////////////////////

        [Description("{ClientFirstName}")]
        ClientFirstName,

        [Description("{ClientLastName}")]
        ClientLastName,

        [Description("{ClientAddress}")]
        ClientAddress,

        [Description("{ClientSign}")]
        ClientSign,

        [Description("{ClientSignDateTime}")]
        ClientSignDateTime,

        [Description("{ClientCompanyName}")]
        ClientCompanyName,
        /////////////////////////////////

        [Description("{ServiceItems}")]
        ServiceItems,

        [Description("{ContractNumber}")]
        ContractNumber,
    }
}

