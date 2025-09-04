using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CMPNatural.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ManifestStatus
    {
        [Description("Draft")]
        Draft,

        [Description("AssignDateTime")]
        AssignDateTime,

        [Description("Assigned")]
        Assigned,

        [Description("Processing")]
        Processing,

        [Description("Canceled")]
        Canceled,

        [Description("Send_To_Admin")]
        Send_To_Admin,

        [Description("Start_Driver")]
        Start_Driver,

        [Description("Completed")]
        Completed,

        [Description("Scaduled")]
        Scaduled,
    }
}