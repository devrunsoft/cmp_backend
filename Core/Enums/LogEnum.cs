using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CMPNatural.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum LogTypeEnum
    {
        [Description("Client")]
        Client,

        [Description("Admin")]
        Admin,

        [Description("Provider")]
        Provider,
    }
}

