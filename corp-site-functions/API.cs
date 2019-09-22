// <copyright file="API.cs" company="ProspectSoft Ltd T/A Zing">
// Copyright (c) ProspectSoft Ltd T/A Zing. All rights reserved.
// </copyright>

using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CorpSiteFunctions.Configuration;
using CorpSiteFunctions.Models;
using CorpSiteFunctions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CorpSiteFunctions
{
    /// <summary>
    /// Function class containing the endpoints called by the Zing Coporate Site.
    /// </summary>
    public class API
    {
        private IHubspotService hubspotService;
        private IRecaptchaService recaptchaService;
        private RecaptchaOptions recaptchaOptions;
        private ILogger<APILoggingCategory> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="API"/> class.
        /// </summary>
        /// <param name="hubspotService">DI for <see cref="HubspotService"/>.</param>
        /// <param name="recaptchaService">DI for <see cref="RecaptchaService"/>.</param>
        /// <param name="recaptchaOptions">DI for <see cref="RecaptchaOptions"/>.</param>
        /// <param name="logger">DI for <see cref="ILogger"/>.</param>
        public API(
            IHubspotService hubspotService,
            IRecaptchaService recaptchaService,
            IOptionsMonitor<RecaptchaOptions> recaptchaOptions,
            ILogger<APILoggingCategory> logger)
        {
            this.hubspotService = hubspotService;
            this.recaptchaService = recaptchaService;
            this.recaptchaOptions = recaptchaOptions.CurrentValue;
            this.logger = logger;
        }

        /// <summary>
        /// Posts a contact into Hubspot.
        /// </summary>
        /// <param name="req">The <see cref="HttpRequest"/> that triggered this function call.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [FunctionName("PostContact")]
        public async Task<IActionResult> PostContact(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req)
        {
            using (StreamReader reader = new StreamReader(req.Body))
            {
                string body = reader.ReadToEnd();
                Submission submission = JsonConvert.DeserializeObject<Submission>(body);

                if (submission == null || string.IsNullOrWhiteSpace(submission.Token) || submission.Contact == null)
                {
                    return new StatusCodeResult(StatusCodes.Status400BadRequest);
                }

                HttpStatusCode captchaResult;
                if ((captchaResult = await this.VerifyCaptcha(submission.Token)) != HttpStatusCode.OK)
                {
                    return new StatusCodeResult((int)captchaResult);
                }

                long contactId;
                if ((contactId = await this.ProcessContact(submission)) == -1)
                {
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }

                HttpResponseMessage noteResult = await this.hubspotService.AddNoteAsync(submission, contactId);
                if (!noteResult.IsSuccessStatusCode)
                {
                    this.logger.LogError(
                        "Unexpected result from Hubspot Engagement API: {0}",
                        await noteResult.Content.ReadAsStringAsync());
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }

            return new StatusCodeResult(StatusCodes.Status200OK);
        }

        private async Task<HttpStatusCode> VerifyCaptcha(string token)
        {
            HttpResponseMessage verifyHTTPResponse = await this.recaptchaService.VerifyTokenASync(token);
            string verifyBody = await verifyHTTPResponse.Content.ReadAsStringAsync();
            VerificationResponse verifyResponse = JsonConvert.DeserializeObject<VerificationResponse>(verifyBody);

            double threshold = this.recaptchaOptions.ScoreThreshold;
            string actionName = this.recaptchaOptions.ActionName;
            if (!verifyResponse.Success)
            {
                this.logger.LogWarning("Recaptcha was not successful: {0}", JsonConvert.SerializeObject(verifyResponse));
                return HttpStatusCode.Unauthorized;
            }

            if (verifyResponse.Score < threshold || verifyResponse.Action != actionName)
            {
                this.logger.LogWarning("Recaptcha could not verify humanity: {0}", JsonConvert.SerializeObject(verifyResponse));
                return HttpStatusCode.Forbidden;
            }

            return HttpStatusCode.OK;
        }

        private async Task<long> ProcessContact(Submission submission)
        {
            HttpResponseMessage contactResult = await this.hubspotService.AddContactAsync(submission);
            string res = await contactResult.Content.ReadAsStringAsync();
            JObject resultBody = JObject.Parse(res);
            switch (contactResult.StatusCode)
            {
                case HttpStatusCode.Conflict:
                    return (long)resultBody["identityProfile"]["vid"];
                case HttpStatusCode.OK:
                    return (long)resultBody["vid"];
                default:
                    this.logger.LogError("Unexpected result from Hubspot Contact API: {0}", res);
                    return -1;
            }
        }
    }
}
