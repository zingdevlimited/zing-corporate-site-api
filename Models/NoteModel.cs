using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace corporate_site_api.Models
{
    public class HubspotNote {

        public HubspotNote(SubmissionMessage message, long contactId) {
            long milis = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            Engagement = new JObject(
                new JProperty("active", true),
                new JProperty("type", "NOTE"),
                new JProperty("timestamp", milis)
            );

            Associations = new JObject(
                new JProperty("contactIds", new JArray(contactId))
            );

            Metadata = new JObject(
                new JProperty("body", $"<h1>{message.Subject}</h1>{message.Message}")
            );
        }

        [JsonProperty("engagement")]
        public JObject Engagement { get; set; }
        [JsonProperty("associations")]
        public JObject Associations { get; set; }
        [JsonProperty("metadata")]
        public JObject Metadata {get;set;}
   }

}