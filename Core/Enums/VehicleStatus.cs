using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CMPNatural.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum VehicleServiceStatus
    {
        [Description("Cooking_Oil_Collection")]
        Cooking_Oil_Collection,

        [Description("Grease_Trap_Management")]
        Grease_Trap_Management,

    }
}

