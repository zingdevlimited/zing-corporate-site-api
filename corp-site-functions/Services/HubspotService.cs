// <copyright file="HubspotService.cs" company="ProspectSoft Ltd T/A Zing">
// Copyright (c) ProspectSoft Ltd T/A Zing. All rights reserved.
// </copyright>

using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CorpSiteFunctions.Configuration;
using CorpSiteFunctions.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CorpSiteFunctions.Services
{
    /// <summary>
    /// Implementation of <see cref="IHubspotService"/>.
    /// </summary>
    public class HubspotService : IHubspotService
    {
        private const string ContactAddUrl = "contacts/v1/contact/?hapikey=";
        private const string NoteAddUrl = "engagements/v1/engagements?hapikey=";
        private const string ListAddUrl = "contacts/v1/lists";

        private readonly IHttpClientFactory clientFactory;
        private readonly ILogger logger;
        private readonly HubspotOptions options;

        /// <summary>
        /// Initializes a new instance of the <see cref="HubspotService"/> class.
        /// </summary>
        /// <param name="options">DI for <see cref="HubspotOptions"/>.</param>
        /// <param name="logger">DI for <see cref="ILogger"/>.</param>
        /// <param name="clientFactory">DI for <see cref="IHttpClientFactory"/>.</param>
        public HubspotService(
            IOptionsMonitor<HubspotOptions> options,
            ILogger<HubspotService> logger,
            IHttpClientFactory clientFactory)
        {
            this.options = options.CurrentValue;
            this.logger = logger;
            this.clientFactory = clientFactory;
        }

        /// <inheritdoc/>
        public async Task<HttpResponseMessage> AddContactAsync(Submission data)
        {
            HubspotContact contact;
            if (string.IsNullOrEmpty(data.Contact.LastName) || string.IsNullOrEmpty(data.Contact.FirstName))
            {
                contact = data.CreateContact(this.options.AssignedUser, true);
            }
            else
            {
                contact = data.CreateContact(this.options.AssignedUser);
            }

            try
            {
                HttpClient client = this.clientFactory.CreateClient();
                var response = await client.PostAsJsonAsync(this.options.BaseUrl + ContactAddUrl + this.options.APIKey, contact);
                return response;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        /// <inheritdoc/>
        public async Task<HttpResponseMessage> AddNoteAsync(Submission data, long contactId)
        {
            HubspotNote note = data.CreateNote(contactId);
            try
            {
                HttpClient client = this.clientFactory.CreateClient();
                var response = await client.PostAsJsonAsync(this.options.BaseUrl + NoteAddUrl + this.options.APIKey, note);
                return response;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        /// <inheritdoc/>
        public async Task<HttpResponseMessage> AddSubscriptionAsync(Submission data, long contactId)
        {
            HubspotSubscription subscription = new HubspotSubscription(data.Contact.Email, contactId);
            try
            {
                HttpClient client = this.clientFactory.CreateClient();
                var response = await client.PostAsJsonAsync(
                    string.Format("{0}/{1}/add?hapikey={2}", this.options.BaseUrl + ListAddUrl, this.options.SubscriptionListId, this.options.APIKey),
                    subscription);
                return response;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}