using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CMPNatural.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ManifestKeyEnum
    {
        [Description("{Date}")]
        Date,

        [Description("{Time}")]
        Time,

        [Description("{ProviderName}")]
        ProviderName,

        [Description("{ProviderAddress}")]
        ProviderAddress,

        [Description("{DriverName}")]
        DriverName,
    }
}

