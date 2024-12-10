using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CMPNatural.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ServiceType
    {
        [Description("Cooking_Oil_Collection")]
        Cooking_Oil_Collection = 1,

        [Description("Grease_Trap_Management")]
        Grease_Trap_Management = 2,

        [Description("Other")]
        Other = 3,

        //[Description("Kitchen_Hood_Cleaning")]
        //Kitchen_Hood_Cleaning = 4,

        //[Description("Power_Washing")]
        //Power_Washing = 5,

        //[Description("Extra_Services")]
        //Extra_Services = 6,

    }
}

