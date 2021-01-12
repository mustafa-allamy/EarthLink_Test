using System.Collections.Specialized;
using System.Text;

namespace EarthLink_Test.Helpers
{
    public class RequestHelper
    {
       

        public string ExecuteRequestSendFile(string url, NameValueCollection paramters, NameValueCollection headers, string filePath)
        {
            System.Net.WebClient webClient = new System.Net.WebClient();
            webClient.QueryString = paramters;
            var responseBytes = webClient.UploadFile(url, filePath);
            //string response = Encoding.ASCII.GetString(responseBytes);
            string response = Encoding.UTF8.GetString(responseBytes);
            return response;
        }
    }
}