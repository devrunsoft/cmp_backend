using Bazaro.Core.Entities;

namespace Bazaro.Core.Models
{
    public class InboxUserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Logo { get; set; }
        public DateTime? Age { get; set; }
        public int? Gender { get; set; }
        public ICollection<Device> Devices { get; set; }
    }
}
