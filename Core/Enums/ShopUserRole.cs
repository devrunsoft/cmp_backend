using System.ComponentModel;

namespace Bazaro.Core.Enums
{
    public enum ShopUserRole
    {
        [Description("مدیر")]
        Manager = 1,
        [Description("دستیار")]
        Assistant = 2
    }
}
