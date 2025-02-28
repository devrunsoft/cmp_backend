using System;
namespace CMPNatural.Application
{
	public class NameAndValue<T>
	{
		public NameAndValue()
		{
		}

		public string name { get; set; }
		public T value { get; set; }
	}
}

