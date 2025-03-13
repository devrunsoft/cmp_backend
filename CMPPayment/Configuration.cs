using CMPNatural.Core.Entities;
using CMPPayment.Model;
using Stripe;
using Stripe.Checkout;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CMPPayment;

public class PaymentConfiguration
{
    //public void init()
    //{
    //    StripeConfiguration.ApiKey = "sk_test_51R0pwFFpQyxnBeACsqotBRAHRw3CbHdA0RJbWtb5uqwapSC8YSYclGczjMgBGQ8SkqCufYxS5isoOSb3LfN1E8ZM00cfG0SQKh";
    //    var productCreateOptions = new PaymentIntentCreateOptions() { Amount = 10000 , Currency = "usd" ,
    //        ReturnUrl  = "https://localhost:7089/api/Payment/CheckStatus",
    //        Confirm = true,
    //        AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions() { Enabled = true , AllowRedirects = "always" } ,
    //        PaymentMethodConfiguration = "pmc_1R0pwmFpQyxnBeACYdsdBPGM",
    //        PaymentMethod = "pm_card_visa",
    //        //ApplicationFeeAmount = 200,
    //        //TransferData = new PaymentIntentTransferDataOptions() { Amount = 200 , Destination = "we_1R0uMbFpQyxnBeACY0BbOsvC" }
    //    };

    //    var service = new PaymentIntentService();
    //   var tt= service.Create(productCreateOptions);
    //    service.Confirm(tt.Id);

    //    //List<PaymentLinkLineItemOptions> line = new List<PaymentLinkLineItemOptions>();
    //    //line.Add(new PaymentLinkLineItemOptions() { Id ="1",  Price = "100", Quantity = 2 });
    //    //var options =  new PaymentLinkCreateOptions() { ApplicationFeeAmount = 100 , LineItems = line };
    //    //var service = new Service();
    //    //var customer = service.Create(options);
    //    var t = tt;
    //}


    public void CreateProductCommand(string ProductName)
    {

    }



    public Session CreatePayment(List<BaseServiceAppointment> productPrices)
    {
        StripeConfiguration.ApiKey = "sk_test_51R0pwFFpQyxnBeACsqotBRAHRw3CbHdA0RJbWtb5uqwapSC8YSYclGczjMgBGQ8SkqCufYxS5isoOSb3LfN1E8ZM00cfG0SQKh";

        var options = new SessionCreateOptions
        {
            PaymentMethodConfiguration = "pmc_1R0pwmFpQyxnBeACYdsdBPGM",
            //PaymentMethodTypes = new List<string> { "card" },
            LineItems = CommandBuilder.Command(productPrices),
            Mode = "payment",
            SuccessUrl = "https://api.app-cmp.com/api/Payment/CheckStatus?session_id={CHECKOUT_SESSION_ID}",
            CancelUrl = "https://api.app-cmp.com/api/Payment/Cancel"
        };

        var service = new SessionService();
        Session session = service.Create(options);

        // Redirect the client to this URL
        string paymentUrl = session.Url;
        Console.WriteLine("Redirect the client to: " + paymentUrl);
        return session;

    }
}

