using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading.Tasks;
using FaceOneApp.Models;
using System.IO;
using System.Net.Http.Headers;

namespace FaceOneWebAPI.Tests
{
    [TestClass]
    public class UnitTestMegVii
    {
        [TestMethod]
        public void TestGetFaceSetsShouldReturnOnlyOneFaceSet()
        {
            HttpClient client = new HttpClient();

            StringContent content = new StringContent("api_key=oxzC5V_7DvpM7uNQITr2ICdBKs1S1f2V&api_secret=IElJ3_FXpUrOxeFMYzcUziNLQq-WLX3W", System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");

            HttpResponseMessage response = Task.Run<HttpResponseMessage>(() => client.PostAsync("https://api.megvii.com/facepp/v3/faceset/getfacesets", content)).Result;
            Assert.IsTrue(response.IsSuccessStatusCode);
            
            DtoGetFaceSets result = JsonConvert.DeserializeObject<DtoGetFaceSets>(response.Content.ReadAsStringAsync().Result);

            Assert.AreEqual(1, result.facesets.Length);
            FaceSet f = result.facesets[0];
            Assert.AreEqual("Face One", f.displayname);
            Assert.AreEqual("Face One", f.outer_id);
        }

        [TestMethod]
        public void TestCreateFaceSetShouldFailWithFaceOne()
        {
            HttpClient client = new HttpClient();

            StringContent content = new StringContent("api_key=oxzC5V_7DvpM7uNQITr2ICdBKs1S1f2V&api_secret=IElJ3_FXpUrOxeFMYzcUziNLQq-WLX3W&display_name=Face+One&outer_id=Face+One&=", System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");

            HttpResponseMessage response = Task.Run<HttpResponseMessage>(() => client.PostAsync("https://api.megvii.com/facepp/v3/faceset/create", content)).Result;

            Assert.IsFalse(response.IsSuccessStatusCode);
            MegviiErrorMessage error = JsonConvert.DeserializeObject<MegviiErrorMessage>(response.Content.ReadAsStringAsync().Result);

            Assert.AreEqual("FACESET_EXIST", error.errorMessage);
            
        }

        [TestMethod]
        public void TestUploadOfAngelinaJolieFile()
        {
            HttpClient client = new HttpClient();
            MultipartFormDataContent content = new MultipartFormDataContent(Guid.NewGuid().ToString());

            StringContent api_key = new StringContent("oxzC5V_7DvpM7uNQITr2ICdBKs1S1f2V");
            content.Add(api_key, "api_key");

            StringContent api_secret = new StringContent("IElJ3_FXpUrOxeFMYzcUziNLQq-WLX3W");
            content.Add(api_secret, "api_secret");

            byte[] file = File.ReadAllBytes(@"C:\Users\bfillon\documents\visual studio 2015\Projects\FaceOne\FaceOneWebAPI.Tests\AngelinaJolie.jpg");
            ByteArrayContent f = new ByteArrayContent(file);
            f.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");

            content.Add(f, "image_file", "image.jpg");

            String s = Task.Run<String>(() => content.ReadAsStringAsync()).Result;
            Console.WriteLine(s);

            HttpResponseMessage response = Task.Run<HttpResponseMessage>(() => client.PostAsync("https://api.megvii.com/facepp/v3/detect", content)).Result;
            Assert.IsTrue(response.IsSuccessStatusCode);

            DtoDetect result = JsonConvert.DeserializeObject<DtoDetect>(response.Content.ReadAsStringAsync().Result);
            Assert.AreEqual("ZTfbOT4xiozVHhIUmEUoyA==", result.image_id); // c'est toujours la même image envoyée
            Assert.AreEqual(1, result.faces.Length); // une seule personne sur la photo
            Assert.IsNotNull(result.faces[0].face_token);

            // Set user id
            string angelinaToken = result.faces[0].face_token;
            StringContent sContent = new StringContent("api_key=oxzC5V_7DvpM7uNQITr2ICdBKs1S1f2V&api_secret=IElJ3_FXpUrOxeFMYzcUziNLQq-WLX3W&user_id=Angelina+Jolie&face_token=" + angelinaToken, System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");
            response = Task.Run<HttpResponseMessage>(() => client.PostAsync("https://api.megvii.com/facepp/v3/face/setuserid", sContent)).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);



            // Get face_token into Faceset
            sContent = new StringContent("api_key=oxzC5V_7DvpM7uNQITr2ICdBKs1S1f2V&api_secret=IElJ3_FXpUrOxeFMYzcUziNLQq-WLX3W", System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");

            response = Task.Run<HttpResponseMessage>(() => client.PostAsync("https://api.megvii.com/facepp/v3/faceset/getfacesets", sContent)).Result;
            Assert.IsTrue(response.IsSuccessStatusCode);

            DtoGetFaceSets fsresult = JsonConvert.DeserializeObject<DtoGetFaceSets>(response.Content.ReadAsStringAsync().Result);
            string faceset_token = fsresult.facesets[0].faceset_token;

            // Set face_token for Angelina
            sContent = new StringContent("api_key=oxzC5V_7DvpM7uNQITr2ICdBKs1S1f2V&api_secret=IElJ3_FXpUrOxeFMYzcUziNLQq-WLX3W&faceset_token=" + faceset_token + "&face_tokens=" + angelinaToken, System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");

            response = Task.Run<HttpResponseMessage>(() => client.PostAsync("https://api.megvii.com/facepp/v3/faceset/addface", sContent)).Result;
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void TestUploadOfBenoitFillonFile()
        {
            HttpClient client = new HttpClient();
            MultipartFormDataContent content = new MultipartFormDataContent(Guid.NewGuid().ToString());

            StringContent api_key = new StringContent("oxzC5V_7DvpM7uNQITr2ICdBKs1S1f2V");
            content.Add(api_key, "api_key");

            StringContent api_secret = new StringContent("IElJ3_FXpUrOxeFMYzcUziNLQq-WLX3W");
            content.Add(api_secret, "api_secret");

            byte[] file = File.ReadAllBytes(@"C:\Users\bfillon\documents\visual studio 2015\Projects\FaceOne\FaceOneWebAPI.Tests\BenoitFillon.jpg");
            ByteArrayContent f = new ByteArrayContent(file);
            f.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");

            content.Add(f, "image_file", "image.jpg");

            HttpResponseMessage response = Task.Run<HttpResponseMessage>(() => client.PostAsync("https://api.megvii.com/facepp/v3/detect", content)).Result;
            Assert.IsTrue(response.IsSuccessStatusCode);

            DtoDetect result = JsonConvert.DeserializeObject<DtoDetect>(response.Content.ReadAsStringAsync().Result);
            //Assert.AreEqual("ZTfbOT4xiozVHhIUmEUoyA==", result.image_id); // c'est toujours la même image envoyée
            Assert.AreEqual(1, result.faces.Length); // une seule personne sur la photo
            Assert.IsNotNull(result.faces[0].face_token);

            // Set user id
            string benoitToken = result.faces[0].face_token;
            StringContent sContent = new StringContent("api_key=oxzC5V_7DvpM7uNQITr2ICdBKs1S1f2V&api_secret=IElJ3_FXpUrOxeFMYzcUziNLQq-WLX3W&user_id=Benoit+Fillon&face_token=" + benoitToken, System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");
            response = Task.Run<HttpResponseMessage>(() => client.PostAsync("https://api.megvii.com/facepp/v3/face/setuserid", sContent)).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);



            // Get face_token into Faceset
            sContent = new StringContent("api_key=oxzC5V_7DvpM7uNQITr2ICdBKs1S1f2V&api_secret=IElJ3_FXpUrOxeFMYzcUziNLQq-WLX3W", System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");

            response = Task.Run<HttpResponseMessage>(() => client.PostAsync("https://api.megvii.com/facepp/v3/faceset/getfacesets", sContent)).Result;
            Assert.IsTrue(response.IsSuccessStatusCode);

            DtoGetFaceSets fsresult = JsonConvert.DeserializeObject<DtoGetFaceSets>(response.Content.ReadAsStringAsync().Result);
            string faceset_token = fsresult.facesets[0].faceset_token;

            // Set face_token for Benoit
            sContent = new StringContent("api_key=oxzC5V_7DvpM7uNQITr2ICdBKs1S1f2V&api_secret=IElJ3_FXpUrOxeFMYzcUziNLQq-WLX3W&faceset_token=" + faceset_token + "&face_tokens=" + benoitToken, System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");

            response = Task.Run<HttpResponseMessage>(() => client.PostAsync("https://api.megvii.com/facepp/v3/faceset/addface", sContent)).Result;
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void TestUploadAndSearchAngelinaJolie()
        {
            HttpClient client = new HttpClient();
            MultipartFormDataContent content = new MultipartFormDataContent(Guid.NewGuid().ToString());

            StringContent api_key = new StringContent("oxzC5V_7DvpM7uNQITr2ICdBKs1S1f2V");
            content.Add(api_key, "api_key");

            StringContent api_secret = new StringContent("IElJ3_FXpUrOxeFMYzcUziNLQq-WLX3W");
            content.Add(api_secret, "api_secret");

            byte[] file = File.ReadAllBytes(@"C:\Users\bfillon\documents\visual studio 2015\Projects\FaceOne\FaceOneWebAPI.Tests\AngelinaJolie2.jpg");
            ByteArrayContent f = new ByteArrayContent(file);
            f.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");

            content.Add(f, "image_file", "image.jpg");

            HttpResponseMessage response = Task.Run<HttpResponseMessage>(() => client.PostAsync("https://api.megvii.com/facepp/v3/detect", content)).Result;
            Assert.IsTrue(response.IsSuccessStatusCode);

            DtoDetect result = JsonConvert.DeserializeObject<DtoDetect>(response.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(1, result.faces.Length); // une seule personne sur la photo
            Assert.IsNotNull(result.faces[0].face_token);

            
            string angelinaToken = result.faces[0].face_token;

            // Get face_token into Faceset
            StringContent sContent = new StringContent("api_key=oxzC5V_7DvpM7uNQITr2ICdBKs1S1f2V&api_secret=IElJ3_FXpUrOxeFMYzcUziNLQq-WLX3W", System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");

            response = Task.Run<HttpResponseMessage>(() => client.PostAsync("https://api.megvii.com/facepp/v3/faceset/getfacesets", sContent)).Result;
            Assert.IsTrue(response.IsSuccessStatusCode);

            DtoGetFaceSets fsresult = JsonConvert.DeserializeObject<DtoGetFaceSets>(response.Content.ReadAsStringAsync().Result);
            string faceset_token = fsresult.facesets[0].faceset_token;

            // Search for Angelina
            sContent = new StringContent("api_key=oxzC5V_7DvpM7uNQITr2ICdBKs1S1f2V&api_secret=IElJ3_FXpUrOxeFMYzcUziNLQq-WLX3W&face_token=" + angelinaToken + "&faceset_token=" + faceset_token, System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");

            response = Task.Run<HttpResponseMessage>(() => client.PostAsync("https://api.megvii.com/facepp/v3/search", sContent)).Result;
            Assert.IsTrue(response.IsSuccessStatusCode);

            DtoSearch searchresult = JsonConvert.DeserializeObject<DtoSearch>(response.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(1, searchresult.results.Length);
            Assert.IsNotNull(searchresult.results[0].face_token);
            Assert.AreEqual("Angelina Jolie", searchresult.results[0].user_id);

            Console.WriteLine(searchresult.results[0].confidence);



        }
    }
}
