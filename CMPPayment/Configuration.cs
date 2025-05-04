using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using CMPPayment.Model;
using ScoutDirect.Core.Caching;
using Stripe;
using Stripe.Checkout;

namespace CMPPayment;

public class PaymentConfiguration : IPaymentConfiguration
{
    private readonly IAppInformationRepository _repository;
    private readonly ICacheService _cache;
    public PaymentConfiguration(IAppInformationRepository providerReposiotry, Func<CacheTech, ICacheService> _cacheService)
    {
        _repository = providerReposiotry;
        _cache = _cacheService(CacheTech.Memory);
    }
    public Session CreatePayment(List<BaseServiceAppointment> productPrices)
    {
        StripeConfiguration.ApiKey = information().StripeApikey;

        var options = new SessionCreateOptions
        {
            PaymentMethodConfiguration = information().StripePaymentMethodConfiguration,
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
        StripeConfiguration.ApiKey = information().StripeApikey;
        var service = new SessionService();
        Session session = await service.GetAsync(CheckoutSessionId);
        return session;

    }


    public AppInformation information()
    {

        var cacheKey = $"AppInformation";

        // Check if the data is already cached
        if (!_cache.TryGet(cacheKey, out AppInformation result))
        {
            // If not cached, fetch from the database

             result = (_repository.GetAllsync()).FirstOrDefault()!;
            // Cache the result for 10 minutes
            _cache.Set(cacheKey, result);
        }

        return result;
    }
}

