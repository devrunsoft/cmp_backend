using ScoutDirect.Application.Responses.Base;
using FaraPayamak;
using System.Net.Http;
using System.Threading.Tasks;

namespace Barbara.Application.Services
{
    public class FaraPayamakSmsService
    {
        public static async Task<Responses.Base.SendSmsResponse> SendSMS(string mobileNumber, string body, int code)
        {
            //// Credentials
            string username = "mortezasjd";
            string password = "@1234Qwer";
            //string domain = "magfa";

            byte[] status = null;
            long[] recid = null;

            SendSoapClient client = new SendSoapClient(SendSoapClient.EndpointConfiguration.SendSoap12);

            // var _FaraPayamak = new Sms.Web.FaraPayamak.Send();
            //var SentResult = await client.SendSmsAsync(new SendSmsRequest()
            //{
            //    username = username,
            //    password = password,
            //    from = "50004000660066",
            //    to = new[] { mobileNumber },
            //    text = body,
            //    isflash = false,
            //    udh = "",
            //    recId = recid,
            //    status = status
            //});

            var SentResult = await client.SendByBaseNumber2Async(username,password, body, mobileNumber, code);

            //// Credentials
            //string username = "farabordbazar_41860";
            //string password = "OLuMOVSXY8PyBXCJ";
            //string domain = "magfa";

            //// Client
            //using var client = new HttpClient();

            //// Call
            //var content = await client.GetStringAsync("https://sms.magfa.com/api/http/sms/v1?service=enqueue&username=" +
            //    username + "&password=" + password + "&domain=" + domain + "&from=98300041860&to=" + mobileNumber + "&text=" + body);


            return new Responses.Base.SendSmsResponse() { Success =( SentResult.Length>15 && long.TryParse(SentResult, out _)) ? true:false, Status = "Success", Message = "" };
        }
    }
}
