using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Barbara.Application.Services
{
    public class IdPayPaymentResponse
    {
        public string id { get; set; } // کلید منحصر بفرد تراکنش
        public string link { get; set; }    // لینک پرداخت برای انتقال خریدار به درگاه پرداخت

        public bool Success { get; set; }
    }

    public class IdPayPaymentVerifyResponse
    {
        public string Id { get; set; } // کلید منحصر بفرد تراکنش
        public bool Success { get; set; }
    }

    public class IDPayService
    {
        private static HttpClient getHttpClient()
        {
            // Client   
            var client = new HttpClient();

            // Credentials
            //client.DefaultRequestHeaders.Add("Content-Type", "application/json");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("X-API-KEY", "0fa6da4e-2dd6-4ad4-acb2-9fac5deddf71");

#if DEBUG
            client.DefaultRequestHeaders.Add("X-SANDBOX", "1");
#else
                 client.DefaultRequestHeaders.Add("X-SANDBOX", "0");
#endif

            return client;
        }
        private static HttpContent getHttpContent(object data)
        {
            //Data
            var myContent = JsonConvert.SerializeObject(data);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return byteContent;
        }

        public static async Task<IdPayPaymentResponse> Payment(object data)
        {
            using var client = getHttpClient();
            var postData = getHttpContent(data);
            var result = await client.PostAsync("https://api.idpay.ir/v1.1/payment", postData);

            if (result.IsSuccessStatusCode)
            {
                //result.EnsureSuccessStatusCode();
                string responseBody = await result.Content.ReadAsStringAsync();
                var resultObject = JsonConvert.DeserializeObject<IdPayPaymentResponse>(responseBody);
                resultObject.Success = true;
                return resultObject;
            }
            else
                return new IdPayPaymentResponse() { Success = false };
        }

        public static async Task<IdPayPaymentVerifyResponse> PaymentVerify(string id, string order_id)
        {
            using var client = getHttpClient();
            var postData = getHttpContent(new { id = id, order_id = order_id });
            var result = await client.PostAsync("https://api.idpay.ir/v1.1/payment/verify", postData);


            if (result.IsSuccessStatusCode)
            {
                //result.EnsureSuccessStatusCode();
                //string responseBody = await result.Content.ReadAsStringAsync();
                //var resultObject = JsonConvert.DeserializeObject<IdPayPaymentResponse>(responseBody);
                //resultObject.Success = true;
                return new IdPayPaymentVerifyResponse() { Success = true, Id = id }; ;
            }
            else
                return new IdPayPaymentVerifyResponse() { Success = false };


        }
    }
}



//var content = new FormUrlEncodedContent(new[]
//{
//     new KeyValuePair<string, string>("order_id", "101"),
//     new KeyValuePair<string, string>("amount", "10000"),
//     new KeyValuePair<string, string>("name", "قاسم رادمان"),
//     new KeyValuePair<string, string>("phone", "09382198592"),
//     new KeyValuePair<string, string>("mail", "my@site.com"),
//     new KeyValuePair<string, string>("desc", "توضیحات پرداخت کننده"),
//     new KeyValuePair<string, string>("callback", "https://example.com/callback")
//}); 