using System;
using System.Net;
using CMPEmail.Email;
using Newtonsoft.Json;

namespace CmpNatural.CrmManagment.Webhook
{
    public class EmailWebHook
    {
        public static void send(MailModel model)
        {
            try
            {
                var result = "-1";
                var webAddr = "https://services.leadconnectorhq.com/hooks/porKMjTM2U71w2EOlVrb/webhook-trigger/4wKrrLJRp3RgBTOqHOrq";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                httpWebRequest.ContentType = "application/json";
                //httpWebRequest.Headers.Add("Authorization:key=" + _serverKey);
                httpWebRequest.Method = "POST";

                string postbody = JsonConvert.SerializeObject(new { email = model.toEmail, link = model.Link,
                    subject = model.Subject,
                    body = model.Body,
                    buttonText = model.buttonText

                });

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

                var response = result;


                //return response;

            }
            catch (Exception ex)
            {
                //return null;
            }
        }
    }

}