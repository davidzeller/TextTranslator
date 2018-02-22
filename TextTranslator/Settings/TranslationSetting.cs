using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace TranslateApp.Settings
{
    public class TranslationSetting
    {
        public string Identifier { get; set; }

        public string From { get; set; }
        public string To { get; set; }
        public string SubscriptionKey { get; set; }
        public string Category { get; set; }
        
        public TranslationSetting(string identifier, string from, string to, string subscriptionKey, string category)
        {
            if(String.IsNullOrEmpty(from))
            {
                throw new ArgumentException(string.Format("Das Element \"from\" ist bei der Sprache {0} nicht gesetzt", identifier));
            }
            if (String.IsNullOrEmpty(to))
            {
                throw new ArgumentException(string.Format("Das Element \"to\" ist bei der Sprache {0} nicht gesetzt", identifier));
            }
            if (String.IsNullOrEmpty(to))
            {
                throw new ArgumentException(string.Format("Das Element \"subscriptionKey\" ist bei der Sprache {0} nicht gesetzt", identifier));
            }

            this.From = from;
            this.To = to;
            this.SubscriptionKey = subscriptionKey;
            this.Category = category;
        }

        public static List<TranslationSetting> ReadFromConfig()
        {
            var translationSettings = new List<TranslationSetting>();

            int i = 1;
            var from = ConfigurationManager.AppSettings["Lang" + i.ToString() + "From"];
            while (!String.IsNullOrEmpty(from))
            {
                var to = ConfigurationManager.AppSettings["Lang" + i.ToString() + "To"].ToString();
                var subscriptionkey = ConfigurationManager.AppSettings["Lang" + i.ToString() + "SubcriptionKey"].ToString(); 
                var category = ConfigurationManager.AppSettings["Lang" + i.ToString() + "Category"].ToString();
                translationSettings.Add(new TranslationSetting("Lang" + i.ToString(), from, to, subscriptionkey, category));

                i++;
                from = ConfigurationManager.AppSettings["Lang" + i.ToString() + "From"];
            }

            return translationSettings;
        }
    }
}
