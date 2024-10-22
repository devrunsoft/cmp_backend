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
    public partial class Invoice : IIdentityObject<long> { }
    public partial class ShoppingCard : IIdentityObject<long> { }

}
