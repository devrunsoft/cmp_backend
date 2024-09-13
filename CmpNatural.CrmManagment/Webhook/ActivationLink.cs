using System;
using System.Net;
using Newtonsoft.Json;

namespace CmpNatural.CrmManagment.Webhook
{
    public class ActivationLink
    {
        public ActivationLinkResponse? send(string email, string activationLink)
        {
            try
            {
                var result = "-1";
                var webAddr = "https://services.leadconnectorhq.com/hooks/porKMjTM2U71w2EOlVrb/webhook-trigger/ZkPR87YxPwgZ1zzoQYrm";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                httpWebRequest.ContentType = "application/json";
                //httpWebRequest.Headers.Add("Authorization:key=" + _serverKey);
                httpWebRequest.Method = "POST";

                string postbody = JsonConvert.SerializeObject(new { email = email , activationlink = activationLink });

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

                ActivationLinkResponse response = JsonConvert.DeserializeObject<ActivationLinkResponse>(result);


                return response;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

}