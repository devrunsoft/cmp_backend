namespace CMPNatural.Core.Entities
{
	public partial class AmendmentCompanyContract
	{
        public long Id { get; set; }
        public long ContractId { get; set; }
        public string ContractNumber { get; set; } = null!;
        public string Content { get; set; } = "";
    }
}