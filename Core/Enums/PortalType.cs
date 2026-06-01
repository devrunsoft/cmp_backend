using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CMPNatural.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PortalType
    {
        [Description("Client")]
        Client ,

        [Description("Admin")]
        Admin ,

        [Description("Provider")]
        Provider ,


        [Description("Driver")]
        Driver,

    }
}

