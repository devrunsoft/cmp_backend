using System;
using System.Text.Json.Serialization;

namespace CMPNatural.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TermsConditionsEnum
    {
        Client,

        Provider
    }
}

