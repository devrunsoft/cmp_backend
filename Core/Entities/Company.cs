using System;
using System.Linq.Expressions;
using CMPNatural.Core.Enums;
using ScoutDirect.Core.Entities;
using Stripe.Forwarding;

namespace CMPNatural.Core.Entities
{
	public partial class Company
	{
		public Company()
		{
		}

		public long Id { get; set; }
		public string CompanyName { get; set; }
		public string PrimaryFirstName { get; set; }
		public string PrimaryLastName { get; set; }
		public string PrimaryPhonNumber { get; set; }
		public string BusinessEmail { get; set; }
		public string Position { get; set; }
		public string? SecondaryFirstName { get; set; } = null;
        public string? SecondaryLastName { get; set; } = null;
        public string? SecondaryPhoneNumber { get; set; } = null;
        public string ReferredBy { get; set; }
		public string AccountNumber { get; set; }
		public string? Password { get; set; }
		public int Type { get; set; }
		public bool Registered { get; set; }
		public bool Accepted { get; set; }
        public CompanyStatus Status { get; set; }
        public Guid? ActivationLink { get; set; } = null;
        public Guid? PersonId { get; set; } = null;
        public string? ProfilePicture { get; set; }
		public string CorporateAddress { get; set; }

        public virtual ICollection<OperationalAddress> OperationalAddress { get; set; }
        public virtual ICollection<BillingInformation> BillingInformations { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
        public virtual ICollection<RequestEntity> Requests { get; set; }
        public virtual ICollection<CompanyContract> CompanyContract { get; set; }

    }


    public static class QueryCompanyExtensions
    {
        public static Expression<Func<Company, bool>> FilterByQuery(string? search, CompanyStatus? Status)
        {
            if (string.IsNullOrWhiteSpace(search))
                return p =>
                  (Status == null || p.Status == Status);

            var loweredSearch = search.Trim().ToLower();
            var addressFilter = QueryExtensions.FilterByQuery(search);
            var companyExt = CompanyContractExtensions.FilterByQuery(search);
            return p =>
                  (Status == null || p.Status == Status) &&
                (p.Id.ToString() == loweredSearch) ||
                (p.CompanyName != null && p.CompanyName.ToLower().Contains(loweredSearch)) ||
                (p.PrimaryFirstName != null && p.PrimaryFirstName.ToLower().Contains(loweredSearch)) ||
                (p.PrimaryLastName != null && p.PrimaryLastName.ToLower().Contains(loweredSearch)) ||
                (p.PrimaryPhonNumber != null && p.PrimaryPhonNumber.ToLower().Contains(loweredSearch)) ||
                (p.BusinessEmail != null && p.BusinessEmail.ToLower().Contains(loweredSearch)) ||
                (p.Position != null && p.Position.ToLower().Contains(loweredSearch)) ||
                (p.SecondaryFirstName != null && p.SecondaryFirstName.ToLower().Contains(loweredSearch)) ||
                (p.SecondaryLastName != null && p.SecondaryLastName.ToLower().Contains(loweredSearch)) ||
                (p.SecondaryPhoneNumber != null && p.SecondaryPhoneNumber.ToLower().Contains(loweredSearch)) ||
                (p.ReferredBy != null && p.ReferredBy.ToLower().Contains(loweredSearch)) ||
                (p.AccountNumber != null && p.AccountNumber.ToLower().Contains(loweredSearch)) ||
                (p.CorporateAddress != null && p.CorporateAddress.ToLower().Contains(loweredSearch)) ||
                (p.AccountNumber != null && p.AccountNumber.ToLower().Contains(loweredSearch)) ||
                p.OperationalAddress.AsQueryable().Any(addressFilter) ||
                p.CompanyContract.AsQueryable().Any(companyExt);
        }

        public static Expression<Func<Company, bool>> FilterByCompanyQuery(string? search)
        {
            if (string.IsNullOrWhiteSpace(search))
                return _ => true;

            var loweredSearch = search.Trim().ToLower();
            return p =>
                (p.Id.ToString() == loweredSearch) ||
                (p.CompanyName != null && p.CompanyName.ToLower().Contains(loweredSearch)) ||
                (p.PrimaryFirstName != null && p.PrimaryFirstName.ToLower().Contains(loweredSearch)) ||
                (p.PrimaryLastName != null && p.PrimaryLastName.ToLower().Contains(loweredSearch)) ||
                (p.PrimaryPhonNumber != null && p.PrimaryPhonNumber.ToLower().Contains(loweredSearch)) ||
                (p.BusinessEmail != null && p.BusinessEmail.ToLower().Contains(loweredSearch)) ||
                (p.Position != null && p.Position.ToLower().Contains(loweredSearch)) ||
                (p.SecondaryFirstName != null && p.SecondaryFirstName.ToLower().Contains(loweredSearch)) ||
                (p.SecondaryLastName != null && p.SecondaryLastName.ToLower().Contains(loweredSearch)) ||
                (p.SecondaryPhoneNumber != null && p.SecondaryPhoneNumber.ToLower().Contains(loweredSearch)) ||
                (p.ReferredBy != null && p.ReferredBy.ToLower().Contains(loweredSearch)) ||
                (p.AccountNumber != null && p.AccountNumber.ToLower().Contains(loweredSearch)) ||
                (p.CorporateAddress != null && p.CorporateAddress.ToLower().Contains(loweredSearch)) ||
                (p.AccountNumber != null && p.AccountNumber.ToLower().Contains(loweredSearch));
        }

    }
}

