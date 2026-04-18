using System;
using System.Text.Json.Serialization;

namespace CMPNatural.Core.Entities
{
    public partial class ManifestGreaseServiceDetail
    {
        public long Id { get; set; }

        public long ServiceAppointmentLocationId { get; set; }

        public decimal GreasePercentage { get; set; }
        public decimal WaterPercentage { get; set; }
        public decimal SolidsPercentage { get; set; }

        public decimal? CodAmount { get; set; }

        public virtual ServiceAppointmentLocation ServiceAppointmentLocation { get; set; }
        //public PaymentMethod? PaymentMethod { get; set; }
        //public string? CheckNumber { get; set; }

    }

    //[JsonConverter(typeof(JsonStringEnumConverter))]
    //public enum PaymentMethod
    //{
    //    Cash,
    //    Check
    //}
}