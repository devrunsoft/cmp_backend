using System;
namespace CMPNatural.Core.Entities
{
    public partial class ProductPrice { 

    public long Id { get; set; }
    public long ProductId { get; set; }
    public string Name { get; set; }
    //Recurring or Onetime
    //public long Type { get; set; }
    public double Amount { get; set; }
    public double MinimumAmount { get; set; }
    public long BillingPeriod { get; set; }
    public int NumberofPayments { get; set; }
    public double SetupFee { get; set; }
    public string? ServiceCrmId { get; set; } = "";
    public string? ServicePriceCrmId { get; set; } = "";
    public bool Enable { get; set; }
    public int Order { get; set; }

    public virtual Product Product { get; set; }
    public virtual ICollection<InvoiceProduct> InvoiceProduct { get; set; } = new List<InvoiceProduct>();
    public virtual ICollection<BaseServiceAppointment> ServiceAppointment { get; set; } = new List<BaseServiceAppointment>();

    }
}

