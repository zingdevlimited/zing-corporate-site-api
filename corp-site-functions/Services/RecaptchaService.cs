// <copyright file="RecaptchaService.cs" company="ProspectSoft Ltd T/A Zing">
// Copyright (c) ProspectSoft Ltd T/A Zing. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using CorpSiteFunctions.Configuration;
using Microsoft.Extensions.Options;

namespace CorpSiteFunctions.Services
{
    /// <summary>
    /// Implemenetation of <see cref="IRecaptchaService"/>.
    /// </summary>
    public class RecaptchaService : IRecaptchaService
    {
        private const string VerifyUrl = "siteverify";

        private readonly IHttpClientFactory clientFactory;
        private readonly RecaptchaOptions options;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecaptchaService"/> class.
        /// </summary>
        /// <param name="clientFactory">DI for <see cref="IHttpClientFactory"/>.</param>
        /// <param name="options">DI for <see cref="RecaptchaOptions"/>.</param>
        public RecaptchaService(IHttpClientFactory clientFactory, IOptionsMonitor<RecaptchaOptions> options)
        {
            this.options = options.CurrentValue;
            this.clientFactory = clientFactory;
        }

        /// <inheritdoc/>
        public async Task<HttpResponseMessage> VerifyTokenASync(string token)
        {
            try
            {
                HttpClient client = this.clientFactory.CreateClient();
                var request = new HttpRequestMessage(HttpMethod.Post, this.options.BaseUrl + VerifyUrl);
                var paraaa = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("secret", this.options.SharedKey),
                    new KeyValuePair<string, string>("response", token),
                };
                request.Content = new FormUrlEncodedContent(paraaa);
                return await client.SendAsync(request);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                throw ex;
            }
        }
    }
}