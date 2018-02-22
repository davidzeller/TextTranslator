using System;
using System.Net;
using System.IO;
using System.Runtime.Serialization;
using System.Web;
using System.Threading.Tasks;
using TranslateApp.Settings;
using System.Net.Http;
using System.Configuration;

namespace TextTranslator.TranslationHubIntegration
{
    public class TranslateApi
    {
        public static string Translate(string authToken, string text, string from, string to, string category)
        {
            string translation = "";
            string uri = ConfigurationManager.AppSettings["TranslatorUri"].ToString() + "/Translate?text=" + HttpUtility.UrlEncode(text) + "&from=" + from + "&to=" + to;

            if(!String.IsNullOrEmpty(category))
            {
                uri += "&category=" + category;
            }

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            httpWebRequest.Headers.Add("Authorization", authToken);
            using (WebResponse response = httpWebRequest.GetResponse())
            using (Stream stream = response.GetResponseStream())
            {
                DataContractSerializer dcs = new DataContractSerializer(Type.GetType("System.String"));
                translation = (string)dcs.ReadObject(stream);
            }

            return translation;
        }
    }
}
