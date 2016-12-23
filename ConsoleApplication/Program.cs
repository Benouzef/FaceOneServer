using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var client = new RestClient("https://eu11.salesforce.com/services/apexrest/Contacts/0030Y000008AjMD");
            var request = new RestRequest(Method.POST);
            
            request.AddHeader("postman-token", "8c17ab06-0796-7be4-f470-a00910c361e8");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("authorization", "Bearer 00D0Y000000pBVh!ARQAQMtklyAwvjOPKr2_3IHl0QlUMrqcQJkDAdBiPIJ6PpUyxPGtlsNU_Ca8qHkRBmLbpdeACNp046Gi9sfYOmCx1prnQUg8");
            IRestResponse response = client.Execute(request);
        }
    }
}
