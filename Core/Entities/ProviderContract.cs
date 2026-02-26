using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using CMPNatural.Core.Enums;

namespace CMPNatural.Core.Entities
{
    public partial class ProviderContract
    {
        public long Id { get; set; }
        public string Content { get; set; } = "";
        public long ContractId { get; set; }
        public long CompanyId { get; set; }
        public long ProviderId { get; set; }
        public string ManifestIsd { get; set; } = string.Empty;
        public long RequestId { get; set; }
        public string? Sign { get; set; }
        public string? AdminSign { get; set; }
        public DateTime? ClientSignDate { get; set; }
        public DateTime? AdminSignDate { get; set; }
        public CompanyContractStatus Status { get; set; }
        public long OperationalAddressId { get; set; }
        public string ContractNumber { get; set; } = string.Empty;

        public Company Company { get; set; }

        [NotMapped]
        public string Number
        {
            get
            {
                return $"PC{CreatedAt.Year}-{this.ProviderId}/{this.OperationalAddressId}-{this.Id}";
            }
        }
        public DateTime CreatedAt { get; set; }

        [NotMapped]
        public string NoteTitle
        {
            get
            {
                return $"{Number}";
            }
        }
    }

    public static class ProviderContractExtensions
    {
        public static Expression<Func<CompanyContract, bool>> FilterByQuery(string? search)
        {
            if (string.IsNullOrWhiteSpace(search))
                return _ => true;

            var loweredSearch = search.Trim().ToLower();
            var addressFilter = QueryExtensions.FilterByQuery(search);
            return p =>
             (p.Content != null && p.Content.ToLower().Contains(loweredSearch)) ||
             (p.ContractNumber != null && p.ContractNumber.ToLower().Contains(loweredSearch)) ||
             (p.RequestId != null && p.RequestId.ToString().Contains(loweredSearch)) ||
             (p.RequestId != null && p.RequestId.ToString().Contains(loweredSearch));
        }
    }
}

