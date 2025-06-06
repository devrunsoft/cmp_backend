using System;
namespace CMPNatural.Core.Entities
{
	public partial class GoHighLevel
	{
        public long Id { get; set; }
        public string LocationId { get; set; } = null!;
        public string Authorization { get; set; } = null!;
        public string RestApi { get; set; } = null!;
        public string Version { get; set; } = null!;
        public string UpdateContactApi { get; set; } = null!;
        public string ForgotPasswordApi { get; set; } = null!;
        public string ActivationLinkApi { get; set; } = null!;
    }
}

