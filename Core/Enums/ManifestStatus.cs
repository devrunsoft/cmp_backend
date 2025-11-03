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

        [Description("Assigned_To_Driver")]
        Assigned_To_Driver,

        [Description("Processing")]
        Processing,

        [Description("Canceled")]
        Canceled,

        [Description("Send_To_Admin")]
        Send_To_Admin,

        [Description("Send_To_Provider")]
        Send_To_Provider,

        [Description("Start_Driver")]
        Start_Driver,

        [Description("Completed")]
        Completed,

        [Description("Partially_Completed")]
        Partially_Completed,

        [Description("Scaduled")]
        Scaduled,
    }
}