using System;
using System.ComponentModel;
using System.Text.Json.Serialization;
using CMPNatural.Core.Entities;

namespace CMPNatural.Application.Responses.ClientServiceAppointment
{
	public class ClientServiceAppointment
	{
		public ClientServiceAppointment()
		{
		}
        public BaseServiceAppointment? Draft { get; set; }
        public BaseServiceAppointment? Current { get; set; }
		public BaseServiceAppointment? Next { get; set; }
        public long ServiceId { get; set; }
        //public bool CanTerminate { get; set; }
        public long RequestId { get; set; }
        public TerminateStatusEnum TerminateStatus { get; set; }
    }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TerminateStatusEnum
{
    [Description("None")]
    None,

    [Description("CanTerminate")]
    CanTerminate,

    [Description("Requested")]
    Requested,

    [Description("Terminated")]
    Terminated,

    [Description("Updated")]
    Updated,

    [Description("Done")]
    Done

}