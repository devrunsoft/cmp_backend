using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Text.Json.Serialization;
using CMPNatural.Core.Enums;

namespace CMPNatural.Core.Entities
{
	public partial class Manifest
	{
		public long Id { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ManifestStatus Status { get; set; }

		public long RequestId { get; set; }

        public long OperationalAddressId { get; set; }

        public long ServiceAppointmentLocationId { get; set; }

        public long? ProviderId { get; set; }

		public string Content { get; set; } = "";

		public string? Comment { get; set; } = "";

		public string? BeforeImages { get; set; }

        public string? AfterImages { get; set; }

        public DateTime PreferredDate { get; set; }

        public DateTime? ServiceDateTime { get; set; }

		public DateTime? StartTime { get; set; }

        public DateTime? DoingStartTime { get; set; }

        public DateTime? FinishTime { get; set; }

		public bool? IsEdited { get; set; }

        public long ContractId { get; set; }

        public DateTime CreatedAt { get; set; }

        public long CompanyId { get; set; }

        public string ManifestNumber { get; set; } = string.Empty;

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

		public virtual RequestEntity Request { get; set; }
        public virtual RouteServiceAppointmentLocation RouteServiceAppointmentLocation { get; set; }
        public virtual Provider? Provider { get; set; }
        public virtual DriverManifest DriverManifest { get; set; }
        public virtual ServiceAppointmentLocation ServiceAppointmentLocation { get; set; }
        public virtual OperationalAddress? OperationalAddress { get; set; }
        public virtual Company? Company { get; set; }

        [NotMapped]
        public ProviderContract? ProviderContract { get; set; }

    }


    public static class QueryManifestExtensions
    {
        public static Expression<Func<Manifest, bool>> FilterByQuery(string? search, ManifestStatus? status)
        {
            return FilterByQuery(search, status, null, null);
        }

        public static Expression<Func<Manifest, bool>> FilterByQuery(
            string? search,
            ManifestStatus? status,
            DateTime? startDate,
            DateTime? endDate)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                return p =>
                    (status == null || p.Status == status) &&
                    (startDate == null || startDate <= p.PreferredDate) &&
                    (endDate == null || endDate >= p.PreferredDate);
            }

            var loweredSearch = search.Trim().ToLower();
            return p =>
                (status == null || p.Status == status) &&
                (startDate == null || startDate <= p.PreferredDate) &&
                (endDate == null || endDate >= p.PreferredDate) &&
                (
                    p.Id.ToString() == loweredSearch ||
                    (p.ManifestNumber != null && p.ManifestNumber.ToLower().Contains(loweredSearch)) ||
                    (p.Content != null && p.Content.ToLower().Contains(loweredSearch)) ||
                    (p.Comment != null && p.Comment.ToLower().Contains(loweredSearch)) ||
                    (p.Company != null && p.Company.CompanyName != null && p.Company.CompanyName.ToLower().Contains(loweredSearch)) ||
                    (p.Provider != null && p.Provider.Name != null && p.Provider.Name.ToLower().Contains(loweredSearch)) ||
                    (p.Request != null && p.Request.RequestNumber != null && p.Request.RequestNumber.ToLower().Contains(loweredSearch)) ||
                    (p.OperationalAddress != null && p.OperationalAddress.Name != null && p.OperationalAddress.Name.ToLower().Contains(loweredSearch)) ||
                    (p.OperationalAddress != null && p.OperationalAddress.Address != null && p.OperationalAddress.Address.ToLower().Contains(loweredSearch))
                );
        }
    }
}
