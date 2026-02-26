using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CMPNatural.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ContractProviderKeysEnum
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

        [Description("{ManagmentCompanySignDateTime}")]
        ManagmentCompanySignDateTime,

        [Description("{ContractNumber}")]
        ContractNumber,
        /// <summary>
        /// 
        /// </summary>

        [Description("{ProviderName}")]
        ProviderName,

        [Description("{ProviderAddress}")]
        ProviderAddress,

        [Description("{ProviderCityStateZip}")]
        ProviderCityStateZip,

        [Description("{ProviderPhone}")]
        ProviderPhone,

        [Description("{ProviderEmail}")]
        ProviderEmail,

        [Description("{ProviderSign}")]
        ProviderSign,

        /// <summary>
        /// 
        /// </summary>

        [Description("{EffectiveDate}")]
        EffectiveDate,

        /// <summary>
        /// 
        /// </summary>
        [Description("{ProviderSignDateTime}")]
        ProviderSignDateTime,

        /// <summary>
        /// 
        /// </summary>
        [Description("{ServiceItems}")]
        ServiceItems,

    }
}
