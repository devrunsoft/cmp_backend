using CMPNatural.Core.Entities;
using Stripe.Checkout;

namespace CMPPayment
{
    public interface IPaymentConfiguration
    {
        public Session CreatePayment(List<BaseServiceAppointment> productPrices);
        public Task<Session> GetPayment(string CheckoutSessionId);

    }
}

