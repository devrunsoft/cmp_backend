using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CMPNatural.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ServiceStatus
    {
        [Description("Draft")]
        Draft,

        [Description("ShouldPay")]
        ShouldPay,

        [Description("Proccessing")]
        Proccessing,

        [Description("Complete")]
        Complete,

        [Description("Canceled")]
        Canceled,

        [Description("UpComingScaduled")]
        UpComingScaduled,

        [Description("Scaduled")]
        Scaduled,

        [Description("Updated_Provider")]
        Updated_Provider,

        [Description("Submited_Provider")]
        Submited_Provider,

        [Description("In_Process")]
        In_Process,

        [Description("Arrived")]
        Arrived,

        [Description("Photo_Before_Work")]
        Photo_Before_Work,

        [Description("Driver_Update_Service")]
        Driver_Update_Service,

        [Description("Photo_After_Work")]
        Photo_After_Work,

        [Description("Done_Driver")]
        Done_Driver,

    }
}

