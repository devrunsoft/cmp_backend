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
                foreach (var i in item.ServiceAppointmentLocations)
                {
                    var am = Math.Round((item.ProductPrice.Amount * (i.FactQty ?? i.Qty)) + double.Epsilon, 2);
                    var seviceAmount = ((am < item.ProductPrice.MinimumAmount) ?
                        item.ProductPrice.MinimumAmount : am) - item.Subsidy;

                    item.TotalAmount = (decimal)seviceAmount;
                    amount += seviceAmount;
                }
            }

            this.Amount = amount;
            return amount;
        }

        public double CalculateAmountByamount()
        {
            double amount = 0;

            foreach (var item in this.BaseServiceAppointment)
            {
                foreach (var i in item.ServiceAppointmentLocations)
                {

                var am = Math.Round((item.Amount * (i.FactQty ?? i.Qty)) + double.Epsilon, 2);
                var seviceAmount = ((am < item.ProductPrice.MinimumAmount) ?
                    item.ProductPrice.MinimumAmount : am) - item.Subsidy;

                item.TotalAmount = (decimal)seviceAmount;
                amount += seviceAmount;
                }
            }

            this.Amount = amount;
            return amount;
        }

    }

    public partial class RequestEntity
    {
        public double CalculateAmount()
        {
            double amount = 0;

            foreach (var item in this.BaseServiceAppointment)
            {
                var am = Math.Round((item.ProductPrice.Amount *  (item.Qty)) + double.Epsilon, 2);
                var seviceAmount = ((am < item.ProductPrice.MinimumAmount) ?
                    item.ProductPrice.MinimumAmount : am) - item.Subsidy;

                item.TotalAmount = (decimal)seviceAmount;
                amount += seviceAmount;
            }

            this.Amount = amount;
            return amount;
        }

        public double CalculateAmountByamount()
        {
            double amount = 0;

            foreach (var item in this.BaseServiceAppointment)
            {
                var am = Math.Round((item.Amount * (item.Qty)) + double.Epsilon, 2);
                var seviceAmount = ((am < item.ProductPrice.MinimumAmount) ?
                    item.ProductPrice.MinimumAmount : am) - item.Subsidy;

                item.TotalAmount = (decimal)seviceAmount;
                amount += seviceAmount;
            }

            this.Amount = amount;
            return amount;
        }

    }
}

