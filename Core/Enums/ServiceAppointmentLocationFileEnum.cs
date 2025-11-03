using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CMPNatural.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ServiceAppointmentLocationFileEnum
    {
        [Description("Before")]
        Before,

        [Description("After")]
        After,
    }

}

