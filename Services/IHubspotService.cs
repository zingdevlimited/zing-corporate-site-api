namespace corporate_site_api.Services
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using corporate_site_api.Models;

    public interface IHubspotService {
        Task<HttpResponseMessage> AddContactAsync(Submission data);
        Task<HttpResponseMessage> AddNoteAsync(Submission data, long contactId);
        Task<HttpResponseMessage> AddSubscriptionAsync(Submission data, long contactId);
    }



}