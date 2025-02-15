using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CMPNatural.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ProviderServiceAssignmentStatus
    {
        [Description("assigned")]
        assigned = 1,

        [Description("canceledByAdmin")]
        canceled = 2,

        [Description("canceledByProvider")]
        canceledByProvider = 3,

        [Description("completed")]
        completed = 4,
    }
}

