using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CMPNatural.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RequestTerminateEnum
    {
        [Description("Terminate")]
        Terminate,
        [Description("Edit")]
        Edit

    }

}