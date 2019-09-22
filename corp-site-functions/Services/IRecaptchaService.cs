// <copyright file="IRecaptchaService.cs" company="ProspectSoft Ltd T/A Zing">
// Copyright (c) ProspectSoft Ltd T/A Zing. All rights reserved.
// </copyright>

using System.Net.Http;
using System.Threading.Tasks;

namespace CorpSiteFunctions.Services
{
    /// <summary>
    /// Defines the service methods to interact with Recaptcha.
    /// </summary>
    public interface IRecaptchaService
    {
        /// <summary>
        /// Verifies the provided token with Google Recaptcha.
        /// </summary>
        /// <param name="token">The token to be verified.</param>
        /// <returns><see cref="HttpResponseMessage"/>.</returns>
        Task<HttpResponseMessage> VerifyTokenASync(string token);
    }
}