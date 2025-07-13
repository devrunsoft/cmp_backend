using System;
using System.Linq.Expressions;

namespace CMPNatural.Core.Entities
{
	public partial class OperationalAddress
	{
		public OperationalAddress()
		{
		}

        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string? Name { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public string Address { get; set; }
		public string CrossStreet { get; set; }
		public string County { get; set; }
		public string LocationPhone { get; set; }
		public long BusinessId { get; set; }
		public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<LocationCompany> LocationCompany { get; set; }
        public virtual ICollection<LocationDateTime> LocationDateTimes { get; set; } = new List<LocationDateTime>();

        public virtual Company Company { get; set; }

    }

    public static class QueryExtensions
    {
        public static Expression<Func<OperationalAddress, bool>> FilterByQuery(string? search)
        {
            if (string.IsNullOrWhiteSpace(search))  
                return _ => true;

            var loweredSearch = search.Trim().ToLower();

            return p =>
                (p.Address != null && p.Address.ToLower().Contains(loweredSearch)) ||
                (p.Name != null && p.Name.ToLower().Contains(loweredSearch)) ||
                (p.County != null && p.County.ToLower().Contains(loweredSearch)) ||
                (p.CrossStreet != null && p.CrossStreet.ToLower().Contains(loweredSearch)) ||
                (p.FirstName != null && p.FirstName.ToLower().Contains(loweredSearch)) ||
                (p.LastName != null && p.LastName.ToLower().Contains(loweredSearch)) ||
                (p.LocationPhone != null && p.LocationPhone.ToLower().Contains(loweredSearch)) ||
                (
                    ((p.Name ?? "") + " - " +
                     (p.Address ?? "") + " - " +
                     (p.LocationPhone ?? "") + " - #" +
                     p.Id.ToString()).ToLower().Contains(loweredSearch)
                );
        }
    }

}

