namespace corporate_site_api.Services
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using corporate_site_api.Models;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using System.Net;

    public class HubspotService : IHubspotService
    {
        private const string CONTACT_ADD_URL = "https://api.hubapi.com/contacts/v1/contact/?hapikey=";
        private const string NOTE_ADD_URL = "https://api.hubapi.com/engagements/v1/engagements?hapikey=";
        private const string LIST_ADD_URL = "https://api.hubapi.com/contacts/v1/lists";

        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger _logger;
        private readonly HubspotOptions _options;

        public HubspotService(IOptionsMonitor<HubspotOptions> options,
            ILogger<HubspotService> logger,
        IHttpClientFactory clientFactory)
        {
            _options = options.CurrentValue;
            _logger = logger;
            _clientFactory = clientFactory;
        }

        public async Task<HttpResponseMessage> AddContactAsync(Submission data){
            HubspotContact contact = data.CreateContact(_options.AssignedUser);
            try {
                HttpClient client = _clientFactory.CreateClient();
                var response = await client.PostAsJsonAsync(CONTACT_ADD_URL+_options.APIkey,contact);
                return response;
            }
            catch(Exception ex) {
                _logger.LogError(ex.Message);
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<HttpResponseMessage> AddNoteAsync(Submission data, long contactId) {
            HubspotNote note = data.CreateNote(contactId);
            try {
                HttpClient client = _clientFactory.CreateClient();
                var response = await client.PostAsJsonAsync(NOTE_ADD_URL+_options.APIkey,note);
                return response;
            }
            catch(Exception ex) {
                _logger.LogError(ex.Message);
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }            
        }

        public async Task<HttpResponseMessage> AddSubscriptionAsync(Submission data, long contactId)
        {
            HubspotSubscription subscription = new HubspotSubscription(data.Contact.Email, contactId);
            try
            {
                HttpClient client = _clientFactory.CreateClient();
                var response = await client.PostAsJsonAsync(
                    string.Format("{0}/{1}/add?hapikey={2}",LIST_ADD_URL,_options.SubscriptionListId,_options.APIkey),
                    subscription
                );
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

    }

    
}