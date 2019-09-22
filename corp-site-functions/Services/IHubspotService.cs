// <copyright file="IHubspotService.cs" company="ProspectSoft Ltd T/A Zing">
// Copyright (c) ProspectSoft Ltd T/A Zing. All rights reserved.
// </copyright>

using System.Net.Http;
using System.Threading.Tasks;
using CorpSiteFunctions.Models;

namespace CorpSiteFunctions.Services
{
    /// <summary>
    /// Defines the service methods to interact with Hubspost.
    /// </summary>
    public interface IHubspotService
    {
        /// <summary>
        /// Adds a contact into Hubspot.
        /// </summary>
        /// <param name="data">The <see cref="Submission"/> to post to Hubspot.</param>
        /// <returns><see cref="HttpResponseMessage"/>.</returns>
        Task<HttpResponseMessage> AddContactAsync(Submission data);

        /// <summary>
        /// Adds a note to a contact in Hubspot.
        /// </summary>
        /// <param name="data">The <see cref="Submission"/> to post to Hubspot.</param>
        /// <param name="contactId">The id of the contact in Hubspot.</param>
        /// <returns><see cref="HttpResponseMessage"/>.</returns>
        Task<HttpResponseMessage> AddNoteAsync(Submission data, long contactId);

        /// <summary>
        /// Adds a subscription to a contact in Hubspot.
        /// </summary>
        /// <param name="data">The <see cref="Submission"/> to post to Hubspot.</param>
        /// <param name="contactId">The id of the contact in Hubspot.</param>
        /// <returns><see cref="HttpResponseMessage"/>.</returns>
        Task<HttpResponseMessage> AddSubscriptionAsync(Submission data, long contactId);
    }
}