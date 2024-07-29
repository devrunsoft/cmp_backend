namespace ScoutDirect.Core.Models
{
    public class AppVersionModel
    {
        public int shop_number { get; set; }
        public bool shop_force { get; set; }
        public int customer_number { get; set; }
        public bool customer_force { get; set; }

        public int shop_number_web { get; set; }
        public bool shop_force_web { get; set; }
        public int customer_number_web { get; set; }
        public bool customer_force_web { get; set; }
    }
}
