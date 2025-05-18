using System;
namespace CMPNatural.Core.Entities
{
	public partial class GoHighLevel
	{
        public long Id { get; set; }
        public string LocationId { get; set; }
        public string Authorization { get; set; }
        public string RestApi { get; set; }
        public string Version { get; set; }
    }
}

