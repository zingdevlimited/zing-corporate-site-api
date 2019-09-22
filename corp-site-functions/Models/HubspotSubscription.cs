// <copyright file="HubspotSubscription.cs" company="ProspectSoft Ltd T/A Zing">
// Copyright (c) ProspectSoft Ltd T/A Zing. All rights reserved.
// </copyright>

using Newtonsoft.Json;

namespace CorpSiteFunctions.Models
{
    /// <summary>
    /// PoCo defining a HubspotSubscription model.
    /// </summary>
    public class HubspotSubscription
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HubspotSubscription"/> class.
        /// </summary>
        /// <param name="email">The email to subscribe with.</param>
        /// <param name="contactId">The contact ID who is subscribing.</param>
        public HubspotSubscription(string email, long contactId)
        {
            this.Emails = new string[] { email };
            this.Ids = new long[] { contactId };
        }

        /// <summary>
        /// Gets or sets the IDs to subscribe to.
        /// </summary>
        [JsonProperty("ids")]
        public long[] Ids { get; set; }

        /// <summary>
        /// Gets or sets the Emails to subscribe.
        /// </summary>
        [JsonProperty("emails")]
        public string[] Emails { get; set; }
    }
}