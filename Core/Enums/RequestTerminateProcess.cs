using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CMPNatural.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RequestTerminateProcessEnum
    {
        [Description("Terminated")]
        Terminated,
        [Description("Updated")]
        Updated

    }

}