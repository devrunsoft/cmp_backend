using System;
using CMPNatural.Core.Entities;
using Stripe.Checkout;

namespace CMPPayment.Model
{
	public class CommandBuilder
	{
        public static List<SessionLineItemOptions> Command(List<BaseServiceAppointment> productPrices)
        {

            var command = new List<SessionLineItemOptions>();

            productPrices.ForEach(e =>
            {
                command.Add(
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",
                        UnitAmount = Convert.ToInt64(e.Amount * 100), // Amount in cents ($100)
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = e.ProductPrice.Name,
                        }
                    },
                    Quantity = e.ServiceAppointmentLocations.Sum(x=>x.FactQty)??0,
                }
                );
            });
            return command;

        }
    }
}

