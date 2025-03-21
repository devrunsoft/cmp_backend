using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CMPNatural.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ProviderServiceAssignmentStatus
    {
        [Description("Assigned")]
        Assigned = 1,

        [Description("Canceled_By_Admin")]
        Canceled_By_Admin = 2,

        [Description("canceled_By_Provider")]
        canceled_By_Provider = 3,

        [Description("Prosessing")]
        Prosessing = 4,

        [Description("Completed")]
        Completed = 5,
    }
}

