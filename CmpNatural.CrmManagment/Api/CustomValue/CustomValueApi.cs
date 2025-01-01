using System;
using CmpNatural.CrmManagment.Model;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using ScoutDirect.Application.Responses;
using System.Net;

namespace CmpNatural.CrmManagment.Api.CustomValue
{
    public class CustomValueApi
    {
        public HighLevelSettings _highLevelSetting { get; set; }

        public CustomValueApi(IOptions<HighLevelSettings> highLevelSetting)
        {
            _highLevelSetting = highLevelSetting.Value;
        }

        public CommandResponse<List<CustomValueResponse>> getAll()
        {
            try
            {
                var result = "-1";
                var webAddr = $"{_highLevelSetting.RestApi}/locations/{_highLevelSetting.LocationId}/customValues";
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Headers.Add($"Authorization: {_highLevelSetting.AuthorizationCustomValues}");
                httpWebRequest.Headers.Add($"Version: {_highLevelSetting.Version}");
                httpWebRequest.Method = "GET";

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                JObject jsonObject = JObject.Parse(result);
                JArray productsArray = (JArray)jsonObject["customValues"];

                List<CustomValueResponse> response = productsArray.ToObject<List<CustomValueResponse>>();

                return new Success<List<CustomValueResponse>>() { Data = response };

            }
            catch (Exception ex)
            {
                return new HasError<List<CustomValueResponse>>() { };
            }
        }
    }
}

