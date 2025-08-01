using CmpNatural.CrmManagment.Api.CustomValue;
using CmpNatural.CrmManagment.Contact;
using CmpNatural.CrmManagment.Invoice;
using CmpNatural.CrmManagment.Product;
using CmpNatural.CrmManagment.Webhook;
using CMPNatural;
using CMPNatural.Application.Services;
using CMPNatural.Core.Repositories;
using CMPNatural.infrastructure;
using CMPNatural.infrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;
using ScoutDirect.Core.Repositories;
using ScoutDirect.infrastructure.Repository;
using CMPPayment;
using CMPNatural.Core.Logger;
using CMPNatural.infrastructure.Logger;
using MediatR;
using CMPNatural.Application.Logger;
using infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CMPNatural.infrastructure.DbQuery;
using CMPNatural.Core.Repositories.Chat;
using CMPNatural.infrastructure.Repository.Chat;

namespace ScoutDirect.infrastructure
{
    public static class RegisterDependencies
    {
        public static void RegisterRepositories(this IServiceCollection services, IConfiguration Configuration, bool isDevelopment)
        {
            services.AddTransient<ICompanyRepository, CompanyRepository>();
            services.AddTransient<ILocationCompanyRepository, LocationCompanyRepository>();
            services.AddTransient<IDocumentRepository, DocumentRepository>();
            services.AddTransient<IBillingInformationRepository, BillingInformationRepository>();
            services.AddTransient<IOperationalAddressRepository, OperationalAddressRepository>();
            services.AddTransient<IBusinessTypeRepository, BusinessTypeRepository>();

            services.AddTransient<IServiceAppointmentRepository, ServiceAppointmentRepository>();
            services.AddTransient<IServiceAppointmentEmergencyRepository, ServiceAppointmentEmergencyRepository>();
            services.AddTransient<IProductPriceRepository, ProductPriceRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IinvoiceRepository, InvoiceRepository>();
            services.AddTransient<IShoppingCardRepository, ShoppingCardRepository>();
            services.AddTransient<IAdminRepository, AdminRepository>();
            services.AddTransient<IProviderReposiotry, ProviderReposiotry>();
            services.AddTransient<IProviderServiceAssignmentRepository, ProviderServiceAssignmentRepository>();
            services.AddTransient<IBaseServiceAppointmentRepository, BaseServiceAppointmentRepository>();
            services.AddTransient<IinvoiceProductRepository, InvoiceProductRepository>();
            //provider
            services.AddTransient<IDriverRepository, DriverRepository>();
            services.AddTransient<IVehicleCompartmentRepository, VehicleCompartmentRepository>();
            services.AddTransient<IVehicleRepository, VehicleRepository>();
            services.AddTransient<IVehicleServiceRepository, VehicleServiceRepository>();
            services.AddTransient<IPersonRepository, PersonRepository>();
            services.AddTransient<ICapacityRepository, CapacityRepository>();
            services.AddTransient<IProviderServiceRepository, ProviderServiceRepository>();
            services.AddTransient<IMenuRepository, MenuRepository>();
            services.AddTransient<IAdminMenuAccessRepository, AdminMenuAccessRepository>();
            services.AddTransient<IAppInformationRepository, AppInformationRepository>();
            services.AddTransient<IContractRepository, ContractRepository>();
            services.AddTransient<IInvoiceSourceRepository, InvoiceSourceRepository>();
            services.AddTransient<ICompanyContractRepository, CompanyContractRepository>();
            services.AddTransient<ITermsConditionsRepository, TermsConditionsRepository>();
            services.AddTransient<IManifestRepository, ManifestRepository>();
            services.AddTransient<IBillingInformationProviderRepository, BillingInformationProviderRepository>();
            services.AddTransient<IServiceAreaRepository, ServiceAreaRepository>();
            services.AddTransient<IServiceAppointmentLocationRepository, ServiceAppointmentLocationRepository>();
            services.AddTransient<IPaymentRepository, PaymentRepository>();
            services.AddTransient<IAppLogRepository, AppLogRepository>();
            services.AddTransient<IGoHighLevelRepository, GoHighLevelRepository>();
            services.AddTransient<IRequestTerminateRepository, RequestTerminateRepository>();
            services.AddTransient<IAmendmentCompanyContractRepository, AmendmentCompanyContractRepository>();

            services.AddTransient<IChatSessionRepository, ChatSessionRepository>();
            services.AddTransient<IChatParticipantRepository, ChatParticipantRepository>();
            services.AddTransient<IChatMessageRepository, ChatMessageRepository>();
            services.AddTransient<IChatMentionRepository, ChatMentionRepository>();
            services.AddTransient<IChatNotificationRepository, ChatNotificationRepository>();
            services.AddTransient<IChatMessageNoteRepository, ChatMessageNoteRepository>();

            //services.AddTransient<IinvoiceProviderRepository, InvoiceProviderRepository>();

            services.AddTransient<ProductListApi>();
            services.AddTransient<ProductPriceApi>();
            services.AddTransient<ContactApi>();
            services.AddTransient<InvoiceApi>();
            services.AddTransient<UpdateContactTokenApi>();
            services.AddTransient<CustomValueApi>();
            services.AddTransient<SyncByCrm>();

            services.AddTransient<IPaymentConfiguration, PaymentConfiguration>();

            services.AddScoped<ICustomDbLogger, CustomDbLogger>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

            if (isDevelopment)
            {

                DbQueryCreator.Init(Configuration.GetConnectionString("LocalConnection"));
            }
            else
            {
                DbQueryCreator.Init(Configuration.GetConnectionString("DefaultConnection"));
            }


        }

        public static void RegisterServices(this IServiceCollection services)
        {
            //services.AddTransient<ICustomerService, CustomerService>();
            //services.AddTransient<IShopService, ShopService>();
            //services.AddTransient<IInboxService, InboxService>();
            //services.AddTransient<IShopUserService, ShopUserService>();
        }
    }
}
