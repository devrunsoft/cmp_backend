namespace CMPNatural.Core.Entities
{
	public partial class DriverManifest
	{
		public long Id { get; set; }
		public long ManifestId { get; set; }
		public long DriverId { get; set; }
		public long ProviderId { get; set; }
		public DateTime CreateAt { get; set; }
        public DateTime? RemoveAt { get; set; }
        public virtual Manifest Manifest { get; set; }
	}
}

