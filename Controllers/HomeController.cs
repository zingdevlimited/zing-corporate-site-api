using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using corporate_site_api.Models;
using corporate_site_api.Services;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace corporate_site_api.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHubspotService _hubspotService;
        private readonly IRecaptchaService _recaptchaService;
        private readonly ILogger _logger;
        private readonly RecaptchaOptions _options;
    
        public HomeController(IHubspotService hubspotService, 
            IRecaptchaService recaptchaService, 
            IOptionsMonitor<RecaptchaOptions> options,
            ILogger<HomeController> logger
            ) : base()
        {
            _hubspotService = hubspotService;
            _recaptchaService = recaptchaService;
            _options = options.CurrentValue;
            _logger = logger;
        }

        #region Post Endpoints
        [Route("contacts")]
        [HttpPost]
        public async Task<IActionResult> PostContact() {
            using(StreamReader reader = new StreamReader(Request.Body)) {
                string body = reader.ReadToEnd();
                Submission submission = JsonConvert.DeserializeObject<Submission>(body);

                if (string.IsNullOrWhiteSpace(submission.Token) || submission.Contact == null)
                {
                    return new StatusCodeResult(StatusCodes.Status400BadRequest);
                }
                HttpStatusCode captchaResult;
                if((captchaResult = await VerifyCaptcha(submission.Token))!=HttpStatusCode.OK) {
                    return new StatusCodeResult((int)captchaResult);
                }

                long contactId;
                if((contactId = await ProcessContact(submission))==-1)
                {
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }

                HttpResponseMessage noteResult = await _hubspotService.AddNoteAsync(submission, contactId);
                if(!noteResult.IsSuccessStatusCode) {
                    _logger.LogError(
                        "Unexpected result from Hubspot Engagement API: {0}", 
                        await noteResult.Content.ReadAsStringAsync()
                    );
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }       
            }
            return new StatusCodeResult(StatusCodes.Status200OK);
        }

        [Route("subscriptions")]
        [HttpPost]
        public async Task<IActionResult> SubscribeContact()
        {
            using (StreamReader reader = new StreamReader(Request.Body))
            {
                string body = reader.ReadToEnd();
                Submission submission = JsonConvert.DeserializeObject<Submission>(body);

                if(string.IsNullOrWhiteSpace(submission.Token) || submission.Contact==null)
                {
                    return new StatusCodeResult(StatusCodes.Status400BadRequest);
                }
                HttpStatusCode captchaResult;
                if ((captchaResult = await VerifyCaptcha(submission.Token)) != HttpStatusCode.OK)
                {
                    return new StatusCodeResult((int)captchaResult);
                }

                long contactId;
                if ((contactId = await ProcessContact(submission)) == -1)
                {
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }

                HttpResponseMessage subscriptionResult = 
                    await _hubspotService.AddSubscriptionAsync(submission, contactId);

                if(!subscriptionResult.IsSuccessStatusCode)
                {
                    _logger.LogError(
                        "Unexpected result from Hubspot Lists API: {0}",
                        await subscriptionResult.Content.ReadAsStringAsync()
                    );
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }
            return new StatusCodeResult(StatusCodes.Status200OK);
        }
        #endregion

        #region internal Methods
        private async Task<HttpStatusCode> VerifyCaptcha(string token)
        {
            HttpResponseMessage verifyHTTPResponse = await _recaptchaService.VerifyTokenASync(token);
            string verifyBody = await verifyHTTPResponse.Content.ReadAsStringAsync();
            VerificationResponse verifyResponse = JsonConvert.DeserializeObject<VerificationResponse>(verifyBody);

            double threshold = _options.ScoreThreshold;
            string actionName = _options.ActionName;
            if (!verifyResponse.Success)
            {
                _logger.LogWarning("Recaptcha was not successful: {0}", JsonConvert.SerializeObject(verifyResponse));
                return HttpStatusCode.Unauthorized;
            }
            if (verifyResponse.Score < threshold || verifyResponse.Action != actionName)
            {
                _logger.LogWarning("Recaptcha could not verify humanity: {0}", JsonConvert.SerializeObject(verifyResponse));
                return HttpStatusCode.Forbidden;
            }
            return HttpStatusCode.OK;
        }

        private async Task<long> ProcessContact(Submission submission)
        {
            HttpResponseMessage contactResult = await _hubspotService.AddContactAsync(submission);
            string res = await contactResult.Content.ReadAsStringAsync();
            JObject resultBody = JObject.Parse(res);
            switch (contactResult.StatusCode)
            {
                case HttpStatusCode.Conflict:
                    return (long)resultBody["identityProfile"]["vid"];
                case HttpStatusCode.OK:
                    return (long)resultBody["vid"];
                default:
                    _logger.LogError("Unexpected result from Hubspot Contact API: {0}", res);
                    return -1;
            }
        }
        #endregion

    }
}
