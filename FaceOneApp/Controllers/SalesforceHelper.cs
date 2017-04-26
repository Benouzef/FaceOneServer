using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FaceOneApp.Controllers
{
    public class SalesforceHelper
    {
        public static void NotifyDetection(string userid)
        {
            HttpClient authClient = new HttpClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            //set OAuth key and secret variables
            string sfdcConsumerKey = "3MVG9HxRZv05HarSNNru046ZmSfpqr8ZKwZF9A_lh2WFMbNbRhjgVsiXApDJG9sPSYbsPtGkSS49BKyIGjpWC";
            string sfdcConsumerSecret = "1949525149160328750";

            //set to Force.com user account that has API access enabled
            string sfdcUserName = "benoit.fillon@free.fr";
            string sfdcPassword = "elmagi9kFc!";
            string sfdcToken = "ERmVfCjMFbbG9tlUd8l6UMQme"; //"nbLIh9gGodn6snYmCUMzEu7Vd"; //"f4SjYKXHrVG2bI5gQFxUo6h8O";

            //create login password value
            string loginPassword = sfdcPassword + sfdcToken;

            MultipartFormDataContent content = new MultipartFormDataContent(Guid.NewGuid().ToString());

            StringContent cGrant_type = new StringContent("password");
            content.Add(cGrant_type, "grant_type");

            StringContent cClient_id = new StringContent(sfdcConsumerKey);
            content.Add(cClient_id, "client_id");

            StringContent cClient_secret = new StringContent(sfdcConsumerSecret);
            content.Add(cClient_secret, "client_secret");

            StringContent cUsername = new StringContent(sfdcUserName);
            content.Add(cUsername, "username");

            StringContent cPassword = new StringContent(loginPassword);
            content.Add(cPassword, "password");

            //StringContent scope = new StringContent("id api web full");
            //content.Add(scope, "scope");


            HttpResponseMessage response = Task.Run<HttpResponseMessage>(() => authClient.PostAsync("https://login.salesforce.com/services/oauth2/token", content)).Result;

            string responseString = response.Content.ReadAsStringAsync().Result;

            JObject obj = JObject.Parse(responseString);
            String oauthToken = (string)obj["access_token"];
            String serviceUrl = (string)obj["instance_url"];
            String tokentype = (string)obj["token_type"];


            //String oauthToken = "00D0Y000000pBVh!ARQAQH80gryjKN_w_AdKoklXjf_eHFZbSxrLeYxYaAzzlUksBkAloCyL_u_1NKFRcm6IfAZPzMzZ2LxbUoTG07Xll5PlTtZc";
            //String serviceUrl = "https://eu11.salesforce.com";


    

            SalesforceNotificationHelper.PerformNotification(serviceUrl, userid, tokentype, oauthToken);
            //PerformNotification("https://eu11.salesforce.com", "0030Y000008AjMD", "Bearer", ""); 


        }

        private static void PerformNotification(string serviceUrl, string userid, string tokentype, string oauthToken)
        {
            HttpClient queryClient = new HttpClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            //QUERY: Retrieve records of type "account"
            //string restQuery = serviceUrl + "/services/data/v25.0/sobjects/Account";
            //QUERY: retrieve a specific account
            //string restQuery = serviceUrl + "/services/data/v25.0/sobjects/Account/001E000000N1H1O";
            //QUERY: Perform a SELECT operation
            string restQuery = serviceUrl + "/services/apexrest/Contacts/" + userid;

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, restQuery);

            //add token to header
            request.Headers.Add("Authorization", tokentype + " " + oauthToken);
            //request.Headers.Add("Authorization", "Bearer 00D0Y000000pBVh!ARQAQDs_BOJ3ziRGD7bE2U0jATOW1rLdMLzOOYOm4kfTU9rYFBhAsjc2RJ8jGYjancQye9DuRwuMVrB7KmjyAF3Wgu0F_Edm");

            //return XML to the caller
            //request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            //return JSON to the caller
            //request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //call endpoint async
            try
            {
                StringContent sContent = new StringContent("", Encoding.UTF8, "application/json");
                HttpResponseMessage response = Task.Run<HttpResponseMessage>(() => queryClient.PostAsync(restQuery, sContent)).Result;

                string responseString = response.Content.ReadAsStringAsync().Result;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}