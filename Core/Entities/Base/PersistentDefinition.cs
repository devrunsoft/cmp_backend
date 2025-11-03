using ScoutDirect.Core.Entities.Base;

namespace CMPNatural.Core.Entities
{
    public partial class Company : IIdentityObject<long> { }
    public partial class LocationCompany : IIdentityObject<long> { }
    public partial class DocumentSubmission : IIdentityObject<long> { }
    public partial class BillingInformation : IIdentityObject<long> { }
    public partial class OperationalAddress : IIdentityObject<long> { }
    public partial class BusinessType : IIdentityObject<long> { }
    public partial class BaseServiceAppointment : IIdentityObject<long> { }
    public partial class ServiceAppointment : IIdentityObject<long> { }
    public partial class ServiceAppointmentEmergency : IIdentityObject<long> { }
    public partial class Product : IIdentityObject<long> { }
    public partial class ProductPrice : IIdentityObject<long> { }
    public partial class InvoiceProduct : IIdentityObject<long> { }
    public partial class Invoice : IIdentityObject<long> { }
    public partial class ShoppingCard : IIdentityObject<long> { }
    public partial class ServiceAppointmentLocation : IIdentityObject<long> { }
    public partial class AdminEntity : IIdentityObject<long> { }
    public partial class Person : IIdentityObject<Guid> { }
    public partial class Provider : IIdentityObject<long> { }
    public partial class ProviderServiceAssignment : IIdentityObject<long> { }
    public partial class Driver : IIdentityObject<long> { }
    public partial class DriverManifest : IIdentityObject<long> { }
    public partial class Vehicle : IIdentityObject<long> { }
    public partial class VehicleCompartment : IIdentityObject<long> { }
    public partial class VehicleService : IIdentityObject<long> { }
    public partial class Capacity : IIdentityObject<long> { }
    public partial class ProviderService : IIdentityObject<long> { }
    public partial class Menu : IIdentityObject<long> { }
    public partial class AdminMenuAccess : IIdentityObject<long> { }
    public partial class AppInformation : IIdentityObject<long> { }
    public partial class Config : IIdentityObject<long> { }
    public partial class Contract : IIdentityObject<long> { }
    public partial class InvoiceSource : IIdentityObject<long> { }
    public partial class CompanyContract : IIdentityObject<long> { }
    public partial class TermsConditions : IIdentityObject<long> { }
    public partial class Manifest : IIdentityObject<long> { }
    public partial class BillingInformationProvider : IIdentityObject<long> { }
    public partial class ServiceArea : IIdentityObject<long> { }
    public partial class Payment : IIdentityObject<long> { }
    public partial class AppLog : IIdentityObject<long> { }
    public partial class GoHighLevel : IIdentityObject<long> { }
    public partial class RequestTerminate : IIdentityObject<long> { }
    public partial class LocationDateTime : IIdentityObject<long> { }
    public partial class AmendmentCompanyContract : IIdentityObject<long> { }

    public partial class ChatParticipant : IIdentityObject<long> { }
    public partial class ChatMention : IIdentityObject<long> { }
    public partial class ChatMessage : IIdentityObject<long> { }
    public partial class ChatMessageNote : IIdentityObject<long> { }
    public partial class ChatNotification : IIdentityObject<long> { }
    public partial class ChatSession : IIdentityObject<long> { }
    public partial class ChatClientSession : IIdentityObject<long> { }
    public partial class ChatMessageManualNote : IIdentityObject<long> { }
    public partial class Route : IIdentityObject<long> { }
    public partial class RouteServiceAppointmentLocation : IIdentityObject<long> { }
    public partial class ServiceAppointmentLocationFile : IIdentityObject<long> { }
    public partial class RequestEntity : IIdentityObject<long> { }
}
