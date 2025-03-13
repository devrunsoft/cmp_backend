using System;
using AutoMapper.Execution;

namespace CMPNatural.Application.Responses.Report
{
	public class ReportResponse
	{
		public string AllInvoiceAmount { get; set; }
        public string ActiveInvoiceAmount { get; set; }
        public string CompleteInvoiceAmount { get; set; }
        public MounthReportEntity ThisMountInvoiceAmount { get; set; }
        public MounthReportEntity LastMountInvoiceAmount { get; set; }
    }

    public class MounthReportEntity
    {
        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public bool CurrentMount { get; set; }

    }
}
