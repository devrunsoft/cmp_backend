using ScoutDirect.Core.Entities.Base;

namespace CMPNatural.Core.Entities
{
    public partial class Company : IIdentityObject<long> { }
    public partial class LocationCompany : IIdentityObject<long> { }
    public partial class DocumentSubmission : IIdentityObject<long> { }
    public partial class BillingInformation : IIdentityObject<long> { }
    public partial class OperationalAddress : IIdentityObject<long> { }
    public partial class BusinessType : IIdentityObject<long> { }

}
