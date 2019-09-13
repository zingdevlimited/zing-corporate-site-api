namespace corporate_site_api.Services
{
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface IRecaptchaService {
        Task<HttpResponseMessage> VerifyTokenASync(string token);
    }



}