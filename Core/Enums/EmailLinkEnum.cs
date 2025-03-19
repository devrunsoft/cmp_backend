using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CMPNatural.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EmailLinkEnum
    {
        AdminInvoices,

        AdminHasCreateContract,

        ClientHasSigned,

        AdminHasSigned
    }
}

