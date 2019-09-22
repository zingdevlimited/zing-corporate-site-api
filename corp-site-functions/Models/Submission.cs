// <copyright file="Submission.cs" company="ProspectSoft Ltd T/A Zing">
// Copyright (c) ProspectSoft Ltd T/A Zing. All rights reserved.
// </copyright>

namespace CorpSiteFunctions.Models
{
    /// <summary>
    /// PoCo defining a submission to the functions API.
    /// </summary>
    public class Submission
    {
        /// <summary>
        /// Gets or sets the SubmissionContact.
        /// </summary>
        public SubmissionContact Contact { get; set; }

        /// <summary>
        /// Gets or sets the SubmissionMessage.
        /// </summary>
        public SubmissionMessage Message { get; set; }

        /// <summary>
        /// Gets or sets the Token.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Creates a contact using the AssignedUser.
        /// </summary>
        /// <param name="assignedUser">The ID of the AssignedUser.</param>
        /// <returns>A <see cref="HubspotContact"/>.</returns>
        public HubspotContact CreateContact(long assignedUser)
        {
            return new HubspotContact(this.Contact, assignedUser);
        }

        /// <summary>
        /// Creates a partial contact parsing the email to get extra details
        /// </summary>
        /// <param name="assignedUser">The ID of the AssignedUser.</param>
        /// <param name="partial">True if the contact is a partial model.</param>
        /// <returns>A <see cref="HubspotContact"/>.</returns>
        public HubspotContact CreateContact(long assignedUser, bool partial)
        {
            return new HubspotContact(this.Contact, assignedUser, partial);
        }

        /// <summary>
        /// Creates a note for the contact ID.
        /// </summary>
        /// <param name="contactId">The contact ID to create the note for.</param>
        /// <returns>A <see cref="HubspotNote"/>.</returns>
        public HubspotNote CreateNote(long contactId)
        {
            return new HubspotNote(this.Message, contactId);
        }
    }
}