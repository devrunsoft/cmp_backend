//using ScoutDirect.Application.Responses.Base;
//using System.Net.Http;
//using System.Threading.Tasks;

//namespace Barbara.Application.Services
//{
//    public class KavehNegarService
//    {
//        public static async Task<SendSmsResponse> Call(string phoneNumber, string body)
//        {
//            // Credentials
//            string apiKey = "6F5635714F734B3841366F686336416B7247454E4F5454573746546E6743442F7932633969777277384B493D";

//            // Client
//            using var client = new HttpClient();

//            // Call
//            var content = await client.GetStringAsync("https://api.kavenegar.com/v1/" + apiKey + "/call/maketts.json?receptor=" + phoneNumber + "&message=" + body);

//            return new SendSmsResponse() { Success = true, Status = "Success", Message = "" };
//        }
//    }
//}
