using System;
using Newtonsoft.Json;
using System.Net;

namespace CmpNatural.CrmManagment.Webhook
{
	public class UpdateContactTokenApi
	{
        public dynamic? send(string email, string token)
        {
            try
            {
                var result = "-1";
                var webAddr = "https://services.leadconnectorhq.com/hooks/porKMjTM2U71w2EOlVrb/webhook-trigger/846c0bbf-8bef-4f1d-a77a-38d18985855a";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                httpWebRequest.ContentType = "application/json";
                //httpWebRequest.Headers.Add("Authorization:key=" + _serverKey);
                httpWebRequest.Method = "POST";

                string postbody = JsonConvert.SerializeObject(new { email = email, token = token });

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

                //ActivationLinkResponse response = JsonConvert.DeserializeObject<ActivationLinkResponse>(result);


                return result;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }


}

