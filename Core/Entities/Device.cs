using System;
using System.Collections.Generic;

namespace Barbara.Core.Entities
{
    public partial class Device
    {
        public long Id { get; set; }
        public Guid? PersonId { get; set; }
        public string DeviceId { get; set; } = null!;
        public string FcmToken { get; set; } = null!;

        //public virtual Person? Person { get; set; }
    }
}
