using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CMPNatural.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CancelEnum
    {
        [Description("ByClient")]
        ByClient,

        [Description("ByAdmin")]
        ByAdmin,

        [Description("ByProvider")]
        ByProvider,

    }
}

