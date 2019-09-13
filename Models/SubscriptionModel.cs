using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace corporate_site_api.Models
{
    public class HubspotSubscription {

        public HubspotSubscription(string email, long contactId)
        {
            Emails = new string[] { email };
            Ids = new long[] { contactId };
        }

        [JsonProperty("vids")]
        public long[] Ids { get; set; }
        [JsonProperty("emails")]
        public string[] Emails { get; set; }
    }

}