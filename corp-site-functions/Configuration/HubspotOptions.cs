// <copyright file="HubspotOptions.cs" company="ProspectSoft Ltd T/A Zing">
// Copyright (c) ProspectSoft Ltd T/A Zing. All rights reserved.
// </copyright>

namespace CorpSiteFunctions.Configuration
{
    /// <summary>
    /// PoCo defining the HubspotOptions from configuration.
    /// </summary>
    public class HubspotOptions
    {
        /// <summary>
        /// Gets or sets the APIKey.
        /// </summary>
        public string APIKey { get; set; }

        /// <summary>
        /// Gets or sets the BaseUrl.
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Gets or sets the AssignedUser.
        /// </summary>
        public long AssignedUser { get; set; }

        /// <summary>
        /// Gets or sets the SubscriptionListId.
        /// </summary>
        public long SubscriptionListId { get; set; }
    }
}