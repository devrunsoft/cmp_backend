using System;
namespace CmpNatural.CrmManagment.Model
{

    public class ContactResponse
    {
        public string Id { get; set; }
        public string LocationId { get; set; }
        public string ContactName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FirstNameRaw { get; set; }
        public string LastNameRaw { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool Dnd { get; set; }
        public string Type { get; set; }
        public string Source { get; set; }
        public string AssignedTo { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Address1 { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string BusinessId { get; set; }
        public List<string> Tags { get; set; }
        public List<string> Followers { get; set; }
        public string Country { get; set; }
        public string Website { get; set; }
    }
}

