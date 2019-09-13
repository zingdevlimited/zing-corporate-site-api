using Newtonsoft.Json;

namespace corporate_site_api.Models
{
    public class VerificationResponse {
        [JsonProperty("success")]
        public bool Success {get;set;}
        [JsonProperty("score")]
        public double Score {get;set;}
        [JsonProperty("action")]
        public string Action {get;set;}
        [JsonProperty("challenge_ts")]
        public string Challenge_ts {get;set;}
        [JsonProperty("hostname")]
        public string Hostname {get;set;}
    }
}