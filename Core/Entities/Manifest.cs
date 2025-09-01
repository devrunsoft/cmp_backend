using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using CMPNatural.Core.Enums;

namespace CMPNatural.Core.Entities
{
	public partial class Manifest
	{
		public long Id { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ManifestStatus Status { get; set; }

		public long InvoiceId { get; set; }

        public long OperationalAddressId { get; set; }

        public long? ProviderId { get; set; }

		public string Content { get; set; } = "";

		public string? Comment { get; set; } = "";

		public string? BeforeImages { get; set; }

        public string? AfterImages { get; set; }

		public DateTime? ServiceDateTime { get; set; }

		public DateTime? StartTime { get; set; }

		public DateTime? FinishTime { get; set; }

		public bool? IsEdited { get; set; }

        public long ContractId { get; set; }

        public DateTime CreatedAt { get; set; }

        public long CompanyId { get; set; }

        public string ManifestNumber { get; set; }

        [NotMapped]
        public string NoteTitle
        {
            get
            {
                return $"{ManifestNumber}";
            }
        }


        public string Number
        {
			get
			{
				return $"M{this.CreatedAt.Year}-{this.CompanyId}/{this.OperationalAddressId}-{(this.ContractId)}-{Id}";
			}
		}

		public virtual Invoice Invoice { get; set; }
        public virtual Provider? Provider { get; set; }
        public virtual DriverManifest DriverManifest { get; set; }

    }
}
