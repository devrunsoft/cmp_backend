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
	public class ProductListApi
	{
        public HighLevelSettings _highLevelSetting { get; set; }

        public ProductListApi(IOptions<HighLevelSettings>  highLevelSetting)
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

        public CommandResponse<ServiceResponse> GetById(string productId)
        {
            try
            {
                var result = "-1";
                var webAddr = $"{_highLevelSetting.RestApi}/products/{productId}/?locationId={_highLevelSetting.LocationId}";

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

                ServiceResponse response = jsonObject.ToObject<ServiceResponse>();

                return new Success<ServiceResponse>() { Data = response };

            }
            catch (Exception ex)
            {
                return new HasError<ServiceResponse>() { };
            }
        }
    }
}

