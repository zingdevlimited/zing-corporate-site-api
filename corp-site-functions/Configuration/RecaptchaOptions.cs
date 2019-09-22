// <copyright file="RecaptchaOptions.cs" company="ProspectSoft Ltd T/A Zing">
// Copyright (c) ProspectSoft Ltd T/A Zing. All rights reserved.
// </copyright>

namespace CorpSiteFunctions.Configuration
{
    /// <summary>
    /// PoCo defining the RecaptchaOptions from configuration.
    /// </summary>
    public class RecaptchaOptions
    {
        /// <summary>
        /// Gets or sets the SharedKey.
        /// </summary>
        public string SharedKey { get; set; }

        /// <summary>
        /// Gets or sets the BaseUrl.
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Gets or sets the ScoreThreshold.
        /// </summary>
        public double ScoreThreshold { get; set; }

        /// <summary>
        /// Gets or sets the ActionName.
        /// </summary>
        public string ActionName { get; set; }
    }
}