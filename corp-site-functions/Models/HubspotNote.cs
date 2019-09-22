// <copyright file="HubspotNote.cs" company="ProspectSoft Ltd T/A Zing">
// Copyright (c) ProspectSoft Ltd T/A Zing. All rights reserved.
// </copyright>

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CorpSiteFunctions.Models
{
    /// <summary>
    /// PoCo defining the Hubspot note object structure.
    /// </summary>
    public class HubspotNote
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HubspotNote"/> class.
        /// </summary>
        /// <param name="message">The <see cref="SubmissionMessage"/> to add to the Hubspot contact.</param>
        /// <param name="contactId">The ID of the contact we are adding the note to.</param>
        public HubspotNote(SubmissionMessage message, long contactId)
        {
            long milis = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            this.Engagement = new JObject(
                new JProperty("active", true),
                new JProperty("type", "NOTE"),
                new JProperty("timestamp", milis));

            this.Associations = new JObject(
                new JProperty("contactIds", new JArray(contactId)));

            this.Metadata = new JObject(
                new JProperty("body", $"<h1>{message.Subject}</h1>{message.Message}"));
        }

        /// <summary>
        /// Gets or sets the Engagement JObject.
        /// </summary>
        [JsonProperty("engagement")]
        public JObject Engagement { get; set; }

        /// <summary>
        /// Gets or sets the Associations JObject.
        /// </summary>
        [JsonProperty("associations")]
        public JObject Associations { get; set; }

        /// <summary>
        /// Gets or sets the Metadata JObject.
        /// </summary>
        [JsonProperty("metadata")]
        public JObject Metadata { get; set; }
   }
}