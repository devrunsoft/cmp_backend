using System;
namespace CMPNatural.Core.Entities
{
	public partial class Menu
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long? Parent { get; set; }
    }
}

