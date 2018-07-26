using System;
using System.Net;
using System.IO;
using System.Runtime.Serialization;
using System.Web;
using System.Threading.Tasks;
using TranslateApp.Settings;
using System.Net.Http;
using System.Configuration;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace TextTranslator.TranslationHubIntegration
{
    public class TranslateApi
    {
        public static async Task<string> TranslateAsync(string authToken, string text, string from, string to, string category)
        {
            string translation = "";
            
            //V2 API currently not used
            string uriV2 = ConfigurationManager.AppSettings["TranslatorUriV2"].ToString() + "/Translate?text=" + HttpUtility.UrlEncode(text) + "&from=" + from + "&to=" + to;

            //V3 API
            string uri = ConfigurationManager.AppSettings["TranslatorUri"].ToString() + "/translate?api-version=3.0" + "&from=" + from + "&to=" + to;


            if (!String.IsNullOrEmpty(category))
            {
                uri += "&category=" + category;
            }

            //Calling V3 API (JSON Response)
            System.Object[] body = new System.Object[] { new { Text = text } };
            var requestBody = JsonConvert.SerializeObject(body);
                                  
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(uri);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Authorization", authToken);                
                var response = await client.SendAsync(request);
                var responseBody = await response.Content.ReadAsStringAsync();
                IEnumerable<TranslationResult> oresult = JsonConvert.DeserializeObject<IEnumerable<TranslationResult>>(responseBody);

                /**
                //Taking the first translation from the first result. See API documentation for more  examples
                // https://docs.microsoft.com/en-us/azure/cognitive-services/translator/reference/v3-0-translate?tabs=curl#examples
                **/
                TranslationResult res = oresult.ToArray()[0];
                var resTranslation = res.translations.ToArray()[0];
                string resText = resTranslation.text;
                
                translation = resText;
            }

            return translation;

            // Code for V2 API (XML based response) currently not used
            //HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            //httpWebRequest.Headers.Add("Authorization", authToken);
            //using (WebResponse response = httpWebRequest.GetResponse())
            //using (Stream stream = response.GetResponseStream())
            //{
            //    DataContractSerializer dcs = new DataContractSerializer(Type.GetType("System.String"));
            //    translation = (string)dcs.ReadObject(stream);
            //}
        }
    }
}
