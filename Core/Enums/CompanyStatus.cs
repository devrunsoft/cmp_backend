using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CMPNatural.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CompanyStatus
    {
        [Description("Approved")]
        Approved,

        [Description("Pending")]
        Pending,

        [Description("Blocked")]
        Blocked
    }

}