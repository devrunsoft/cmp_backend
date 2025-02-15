using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CMPNatural.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ProductCollection
    {
        [Description("Service")]
        Service = 1,

        [Description("Product")]
        Product = 2,

    }
}

