// <copyright file="HubspotContact.cs" company="ProspectSoft Ltd T/A Zing">
// Copyright (c) ProspectSoft Ltd T/A Zing. All rights reserved.
// </copyright>

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CorpSiteFunctions.Models
{
    /// <summary>
    /// PoCo defining the Hubspot contact object structure.
    /// </summary>
    public class HubspotContact
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HubspotContact"/> class.
        /// </summary>
        /// <param name="contact">The <see cref="SubmissionContact"/> to construct a <see cref="HubspotContact"/> from.</param>
        /// <param name="assignedUser">The ID of the user who is carrying out this call.</param>
        /// <param name="incompleteContact">True if the contact is an incomplete model.</param>
        public HubspotContact(SubmissionContact contact, long assignedUser, bool incompleteContact = false)
        {
            if (incompleteContact)
            {
                string firstHalf = contact.Email.Split("@")[0];
                string[] tokens;
                if ((tokens = firstHalf.Split(".")).Length > 1)
                {
                    contact.LastName = tokens[tokens.Length - 1];
                }

                contact.FirstName = tokens[0];
            }

            this.Properties = new JArray(
                this.WrapProperty("firstname", contact.FirstName),
                this.WrapProperty("lastname", contact.LastName),
                this.WrapProperty("email", contact.Email),
                this.WrapProperty("hubspot_owner_id", assignedUser));
        }

        /// <summary>
        /// Gets or sets the JArray of the properties making up this Hubspot contact.
        /// </summary>
        [JsonProperty("properties")]
        public JArray Properties { get; set; }

        private JObject WrapProperty(string property, object value)
        {
            return new JObject(
                new JProperty("property", property),
                new JProperty("value", value));
        }
    }
}