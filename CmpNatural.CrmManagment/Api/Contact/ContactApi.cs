using System;
using CMPNatural.Application.Responses.Service;
using CmpNatural.CrmManagment.Model;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using ScoutDirect.Application.Responses;
using System.Net;

namespace CmpNatural.CrmManagment.Contact
{

    public class ContactApi
    {
        public HighLevelSettings _highLevelSetting { get; set; }

        public ContactApi(IOptions<HighLevelSettings> highLevelSetting)
        {
            _highLevelSetting = highLevelSetting.Value;
        }

        public CommandResponse<List<ContactResponse>> getAlllContact(string? query)
        {
            try
            {
                var result = "-1";
                var webAddr = $"{_highLevelSetting.RestApi}/contacts/?locationId={_highLevelSetting.LocationId}&query={query}";

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
                JArray productsArray = (JArray)jsonObject["contacts"];

                List<ContactResponse> response = productsArray.ToObject<List<ContactResponse>>();

                return new Success<List<ContactResponse>>() { Data = response };

            }
            catch (Exception ex)
            {
                return new HasError<List<ContactResponse>>() { };
            }
        }
    }
}

