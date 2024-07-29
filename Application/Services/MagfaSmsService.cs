//using ScoutDirect.Application.Responses.Base;
//using System.Net.Http;
//using System.Threading.Tasks;

//namespace Barbara.Application.Services
//{
//    public class MagfaSmsService
//    {
//        public static async Task<SendSmsResponse> SendSMS(string mobileNumber, string body)
//        { 
//            // Credentials
//            string username = "farabordbazar_41860";
//            string password = "OLuMOVSXY8PyBXCJ";
//            string domain = "magfa";
  
//            // Client
//            using var client = new HttpClient();

//            // Call
//            var content = await client.GetStringAsync("https://sms.magfa.com/api/http/sms/v1?service=enqueue&username=" +
//                username + "&password=" + password + "&domain=" + domain + "&from=98300041860&to=" + mobileNumber + "&text=" + body);
 
//            return new SendSmsResponse() { Success = true, Status = "Success", Message = "" };
//        }
//    }
//}
