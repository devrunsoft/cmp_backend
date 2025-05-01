using CMPNatural.Core.Entities;
using CMPPayment.Model;
using Stripe;
using Stripe.Checkout;

namespace CMPPayment;

public class PaymentConfiguration : IPaymentConfiguration
{
    public Session CreatePayment(List<BaseServiceAppointment> productPrices)
    {
        StripeConfiguration.ApiKey = "sk_test_51R0pwFFpQyxnBeACsqotBRAHRw3CbHdA0RJbWtb5uqwapSC8YSYclGczjMgBGQ8SkqCufYxS5isoOSb3LfN1E8ZM00cfG0SQKh";

        var options = new SessionCreateOptions
        {
            PaymentMethodConfiguration = "pmc_1R0u24FpQyxnBeACNDCg51fA",
            //PaymentMethodTypes = new List<string> { "card" },
            LineItems = CommandBuilder.Command(productPrices),
            Mode = "payment",
            SuccessUrl = "https://api.app-cmp.com/api/Payment/CheckStatus?session_id={CHECKOUT_SESSION_ID}",
            CancelUrl = "https://api.app-cmp.com/api/Payment/Cancel?session_id={CHECKOUT_SESSION_ID}"
        };

        var service = new SessionService();
        Session session = service.Create(options);
        return session;
    }

    public async Task<Session> GetPayment(string CheckoutSessionId)
    {
        StripeConfiguration.ApiKey = "sk_test_51R0pwFFpQyxnBeACsqotBRAHRw3CbHdA0RJbWtb5uqwapSC8YSYclGczjMgBGQ8SkqCufYxS5isoOSb3LfN1E8ZM00cfG0SQKh";
        var service = new SessionService();
        Session session = await service.GetAsync(CheckoutSessionId);
        return session;

    }
}

