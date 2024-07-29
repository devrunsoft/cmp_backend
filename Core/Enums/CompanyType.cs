using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CMPNatural.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CompanyType
	{
        [Description("StandAloneBusiness")]
        StandAloneBusiness = 1,

        [Description("Chain")]
        Chain = 2,

    }
}

