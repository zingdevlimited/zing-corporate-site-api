// <copyright file="VerificationResponse.cs" company="ProspectSoft Ltd T/A Zing">
// Copyright (c) ProspectSoft Ltd T/A Zing. All rights reserved.
// </copyright>

using Newtonsoft.Json;

namespace CorpSiteFunctions.Models
{
    /// <summary>
    /// PoCo defining a VerificationResponse.
    /// </summary>
    public class VerificationResponse
    {
        /// <summary>
        /// Gets or sets a value indicating whether the verification was successfull.
        /// </summary>
        [JsonProperty("success")]
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the Score.
        /// </summary>
        [JsonProperty("score")]
        public double Score { get; set; }

        /// <summary>
        /// Gets or sets the Action.
        /// </summary>
        [JsonProperty("action")]
        public string Action { get; set; }

        /// <summary>
        /// Gets or sets the challenge timestamp.
        /// </summary>
        [JsonProperty("challenge_ts")]
        public string Challenge_ts { get; set; }

        /// <summary>
        /// Gets or sets the Hostname.
        /// </summary>
        [JsonProperty("hostname")]
        public string Hostname { get; set; }
    }
}