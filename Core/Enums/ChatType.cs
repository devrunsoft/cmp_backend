using System.ComponentModel;

namespace Bazaro.Core.Enums
{ 
    public enum ChatType
    {
        [Description("پیام متنی")]
        Text = 1,

        [Description("پیام تصویری")]
        Image = 2,

        [Description("پیام صوتی")]
        Voice = 3,

        [Description("فاکتور سفارش")]
        Invoice = 10, 
    }
}
