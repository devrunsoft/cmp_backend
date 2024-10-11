using System;
using CmpNatural.CrmManagment.Webhook;
using System.Net;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using CmpNatural.CrmManagment.Model;
using CMPNatural.Application.Responses.Service;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Authentication;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace CmpNatural.CrmManagment.Product
{
    public class ProductPriceApi
    {
        public HighLevelSettings _highLevelSetting { get; set; }

        public ProductPriceApi(IOptions<HighLevelSettings> highLevelSetting)
        {
            _highLevelSetting = highLevelSetting.Value;
        }

        public CommandResponse<List<ServicePriceResponse>> call(string productId)
        {
            try
            {
                var result = "-1";
                var webAddr = $"{_highLevelSetting.RestApi}/products/{productId}/price?locationId={_highLevelSetting.LocationId}";

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
                JArray productsArray = (JArray)jsonObject["prices"];

                List<ServicePriceResponse> response = productsArray.ToObject<List<ServicePriceResponse>>();

    

                return new Success<List<ServicePriceResponse>>() { Data = response };

            }
            catch (Exception ex)
            {
                return new HasError<List<ServicePriceResponse>>() { };
            }
        }
    }
}

