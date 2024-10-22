using System;
using CMPNatural.Application.Responses.Service;
using CmpNatural.CrmManagment.Model;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using ScoutDirect.Application.Responses;
using System.Net;
using CmpNatural.CrmManagment.Command;
using Newtonsoft.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CmpNatural.CrmManagment.Invoice
{
    public class InvoiceApi
    {
        public HighLevelSettings _highLevelSetting { get; set; }

        public InvoiceApi(IOptions<HighLevelSettings> highLevelSetting)
        {
            _highLevelSetting = highLevelSetting.Value;
        }

        public CommandResponse<InvoiceApiResponse> GetInvoice(string invoiceId)
        {
            try
            {
                var result = "-1";
                var webAddr = $"{_highLevelSetting.RestApi}/invoices/{invoiceId}?altId={_highLevelSetting.LocationId}&altType=location";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Headers.Add($"Authorization: {_highLevelSetting.Authorization}");
                httpWebRequest.Headers.Add($"Version: {_highLevelSetting.Version}");
                httpWebRequest.Method = "GET";

                //var postbody = JsonConvert.SerializeObject(command);

                //using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                //{
                //    streamWriter.Write(postbody);
                //    streamWriter.Flush();
                //}

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                JObject jsonObject = JObject.Parse(result);

                InvoiceApiResponse response = jsonObject.ToObject<InvoiceApiResponse>();

                return new Success<InvoiceApiResponse>() { Data = response };

            }
            catch (Exception ex)
            {
                return new HasError<InvoiceApiResponse>() { };
            }
        }

        public CommandResponse<InvoiceApiResponse> CreateInvoice(CreateInvoiceApiCommand command)
        {
            try
            {
                var result = "-1";
                var webAddr = $"{_highLevelSetting.RestApi}/invoices/";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Headers.Add($"Authorization: {_highLevelSetting.Authorization}");
                httpWebRequest.Headers.Add($"Version: {_highLevelSetting.Version}");
                httpWebRequest.Method = "POST";

                var postbody = JsonConvert.SerializeObject(command);

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(postbody);
                    streamWriter.Flush();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                JObject jsonObject = JObject.Parse(result);

                InvoiceApiResponse response = jsonObject.ToObject<InvoiceApiResponse>();

                return new Success<InvoiceApiResponse>() { Data = response };

            }
            catch (Exception ex)
            {
                return new HasError<InvoiceApiResponse>() { };
            }
        }
        public CommandResponse<InvoiceApiResponse> SendInvoice(string invoiceId, SendInvoiceCommand command)
        {
            try
            {
                var result = "-1";
                var webAddr = $"{_highLevelSetting.RestApi}/invoices/{invoiceId}/send";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Headers.Add($"Authorization: {_highLevelSetting.Authorization}");
                httpWebRequest.Headers.Add($"Version: {_highLevelSetting.Version}");
                httpWebRequest.Method = "POST";

                //var command = new Dictionary<string, string>
                //{
                //{ "altId", _highLevelSetting.LocationId },
                //{ "altType", "location" },
                //{ "userId",  userId },
                //{ "action", "sms_and_email" },
                //{ "liveMode", "true"}
                //};

                var postbody = JsonConvert.SerializeObject(command);

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(postbody);
                    streamWriter.Flush();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                JObject jsonObject = JObject.Parse(result);

                InvoiceApiResponse response = jsonObject.ToObject<InvoiceApiResponse>();

                return new Success<InvoiceApiResponse>() { Data = response };

            }
            catch (Exception ex)
            {
                return new HasError<InvoiceApiResponse>() { };
            }
        }
    }
}

