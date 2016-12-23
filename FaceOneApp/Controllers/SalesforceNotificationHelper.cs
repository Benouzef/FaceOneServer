using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace FaceOneApp.Controllers
{
    public class SalesforceNotificationHelper
    {
        public static void PerformNotification(string serviceUrl, string userid, string tokentype, string oauthToken)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var client = new RestClient(serviceUrl + "/services/apexrest/Contacts/" + userid);
            var request = new RestRequest(Method.POST);

            //request.AddHeader("postman-token", "8c17ab06-0796-7be4-f470-a00910c361e8");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("authorization", tokentype + " " + oauthToken);
            IRestResponse response = client.Execute(request);
        }
    }
}