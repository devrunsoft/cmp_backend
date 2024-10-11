using System;
using CMPNatural.Application.Responses.Service;
using CmpNatural.CrmManagment.Model;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using ScoutDirect.Application.Responses;
using System.Net;

namespace CmpNatural.CrmManagment.Invoice
{
    public class InvoiceApi
    {
        public HighLevelSettings _highLevelSetting { get; set; }

        public InvoiceApi(IOptions<HighLevelSettings> highLevelSetting)
        {
            _highLevelSetting = highLevelSetting.Value;
        }

        public CommandResponse<List<ServiceResponse>> call()
        {
            try
            {
                var result = "-1";
                var webAddr = $"{_highLevelSetting.RestApi}/products/?locationId={_highLevelSetting.LocationId}";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Headers.Add($"Authorization: {_highLevelSetting.Authorization}");
                httpWebRequest.Headers.Add($"Version: {_highLevelSetting.Version}");
                httpWebRequest.Method = "GET";


                //using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                //{
                //    streamWriter.Flush();
                //}

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                JObject jsonObject = JObject.Parse(result);
                JArray productsArray = (JArray)jsonObject["products"];

                List<ServiceResponse> response = productsArray.ToObject<List<ServiceResponse>>();

                return new Success<List<ServiceResponse>>() { Data = response };

            }
            catch (Exception ex)
            {
                return new HasError<List<ServiceResponse>>() { };
            }
        }
    }
}

