using System;
namespace CMPNatural.Application.Model
{
	public class ProductPriceInput
    {
        public string Name { get; set; }
        public double Amount { get; set; }
        public double MinimumAmount { get; set; }
        public long BillingPeriod { get; set; }
        public int NumberofPayments { get; set; }
        public double SetupFee { get; set; }
        public bool Enable { get; set; }
        public int Order { get; set; } = 1;
    }
}

