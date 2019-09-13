namespace corporate_site_api.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Options;

    public class RecaptchaService : IRecaptchaService
    {

        private const string VERIFY_URL = "https://www.google.com/recaptcha/api/siteverify";

        private readonly IHttpClientFactory clientFactory;
        private readonly RecaptchaOptions options;

        public RecaptchaService(IHttpClientFactory clientFactory, IOptionsMonitor<RecaptchaOptions> options)
        {
            this.options = options.CurrentValue;
            this.clientFactory = clientFactory;
        }

        public async Task<HttpResponseMessage> VerifyTokenASync(string token)
        {
            try {
                HttpClient client = clientFactory.CreateClient();
                var request = new HttpRequestMessage(HttpMethod.Post,VERIFY_URL);
                var paraaa = new List<KeyValuePair<string,string>> {
                    new KeyValuePair<string,string>("secret",options.SharedKey),
                    new KeyValuePair<string,string>("response",token)
                };
                request.Content = new FormUrlEncodedContent(paraaa);
                return await client.SendAsync(request);
            }
            catch(Exception ex) {
                Console.Error.WriteLine(ex.Message);
                throw ex;
            }            
        }

    }

    
}