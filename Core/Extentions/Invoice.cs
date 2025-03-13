using System;
namespace CMPNatural.Core.Entities
{
	public partial class Invoice
	{
        public double CalculateAmount()
        {
            double amount = 0;

            foreach (var item in this.BaseServiceAppointment)
            {
                var am = (item.ProductPrice.Amount * item.Qty);
                var seviceAmount = ((am < item.ProductPrice.MinimumAmount) ?
                    item.ProductPrice.MinimumAmount : am) - item.Subsidy;

                amount += seviceAmount;
            }

            this.Amount = amount;
            return amount;
        }
      }
}

