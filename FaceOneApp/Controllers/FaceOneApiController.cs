using FaceOneApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace FaceOneApp.Controllers
{
    public class FaceOneApiController : ApiController
    {
        [HttpPost]
        public string Post([FromBody] JToken base64image)
        {

            string str = base64image.ToString();
            str = str.Split(',')[1];

            try
            {
                File.WriteAllBytes(Path.GetTempPath() + @"\yourfile.png", Convert.FromBase64String(str));
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }


            HttpClient client = new HttpClient();
            MultipartFormDataContent content = new MultipartFormDataContent(Guid.NewGuid().ToString());

            StringContent api_key = new StringContent("oxzC5V_7DvpM7uNQITr2ICdBKs1S1f2V");
            content.Add(api_key, "api_key");

            StringContent api_secret = new StringContent("IElJ3_FXpUrOxeFMYzcUziNLQq-WLX3W");
            content.Add(api_secret, "api_secret");

            byte[] file = File.ReadAllBytes(Path.GetTempPath() + @"\yourfile.png");
            ByteArrayContent f = new ByteArrayContent(file);
            f.Headers.ContentType = MediaTypeHeaderValue.Parse("image/png");

            content.Add(f, "image_file", "image.png");

            String s = Task.Run<String>(() => content.ReadAsStringAsync()).Result;
            Console.WriteLine(s);
            
            HttpResponseMessage response = Task.Run<HttpResponseMessage>(() => client.PostAsync("https://api.megvii.com/facepp/v3/detect", content)).Result;

            DtoDetect result = JsonConvert.DeserializeObject<DtoDetect>(response.Content.ReadAsStringAsync().Result);
            

            string token = result.faces[0].face_token;

            // Get face_token into Faceset
            StringContent sContent = new StringContent("api_key=oxzC5V_7DvpM7uNQITr2ICdBKs1S1f2V&api_secret=IElJ3_FXpUrOxeFMYzcUziNLQq-WLX3W", System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");

            response = Task.Run<HttpResponseMessage>(() => client.PostAsync("https://api.megvii.com/facepp/v3/faceset/getfacesets", sContent)).Result;
            
            DtoGetFaceSets fsresult = JsonConvert.DeserializeObject<DtoGetFaceSets>(response.Content.ReadAsStringAsync().Result);
            string faceset_token = fsresult.facesets[0].faceset_token;

            // Search for token
            sContent = new StringContent("api_key=oxzC5V_7DvpM7uNQITr2ICdBKs1S1f2V&api_secret=IElJ3_FXpUrOxeFMYzcUziNLQq-WLX3W&face_token=" + token + "&faceset_token=" + faceset_token, System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");

            response = Task.Run<HttpResponseMessage>(() => client.PostAsync("https://api.megvii.com/facepp/v3/search", sContent)).Result;

            DtoSearch searchresult = JsonConvert.DeserializeObject<DtoSearch>(response.Content.ReadAsStringAsync().Result);
            Console.WriteLine(searchresult.results[0].user_id);

            string newUserIdComingFromSF = searchresult.results[0].user_id; // "0030Y000008AjMD";
            SalesforceHelper.NotifyDetection(newUserIdComingFromSF);


            return searchresult.results[0].user_id;
        }
    }
}
