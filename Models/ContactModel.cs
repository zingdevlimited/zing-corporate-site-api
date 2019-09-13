using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace corporate_site_api.Models
{
    public class HubspotContact {
        private JObject WrapProperty(string property, object value) {
            return new JObject(
                new JProperty("property",property),
                new JProperty("value",value)
            );
        }

        public HubspotContact(SubmissionContact contact, long assignedUser, bool incompleteContact = false) {
            if(incompleteContact) {
                string firstHalf = contact.Email.Split("@")[0];
                string[] tokens;
                if((tokens = firstHalf.Split(".")).Length > 1) {
                    contact.LastName = tokens[tokens.Length - 1];
                }
                contact.FirstName = tokens[0];
            }
            Properties = new JArray(
                WrapProperty("firstname",contact.FirstName),
                WrapProperty("lastname",contact.LastName),
                WrapProperty("email",contact.Email),
                WrapProperty("hubspot_owner_id",assignedUser)
            );
        }

        [JsonProperty("properties")]
        public JArray Properties {get;set;}
    }

}