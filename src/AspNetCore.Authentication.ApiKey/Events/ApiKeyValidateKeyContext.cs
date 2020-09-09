// Copyright (c) Mihir Dilip. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for license information.

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace AspNetCore.Authentication.ApiKey
{
    /// <summary>
    /// Context used for validating key.
    /// </summary>
    public class ApiKeyValidateKeyContext : ResultContext<ApiKeyOptions>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scheme"></param>
        /// <param name="options"></param>
        /// <param name="apiKey"></param>
        public ApiKeyValidateKeyContext(HttpContext context, AuthenticationScheme scheme, ApiKeyOptions options, string apiKey) 
            : base(context, scheme, options)
        {
            ApiKey = apiKey;
        }

        /// <summary>
        /// Gets the Api Key.
        /// </summary>
        public string ApiKey { get; }

        /// <summary>
        /// Calling this method will handle construction of authentication principal (<see cref="ClaimsPrincipal" />) 
        /// which will be assiged to the <see cref="ResultContext{TOptions}.Principal"/> property 
        /// and <see cref="ResultContext{TOptions}.Success"/> method will also be called.
        /// </summary>
        /// <param name="claims">Claims to be added to the identity.</param>
        public void ValidationSucceeded(IEnumerable<Claim> claims = null)
        {
            ValidationSucceeded(null, claims);
        }

        /// <summary>
        /// Calling this method will handle construction of authentication principal (<see cref="ClaimsPrincipal" />) 
        /// which will be assiged to the <see cref="ResultContext{TOptions}.Principal"/> property 
        /// and <see cref="ResultContext{TOptions}.Success"/> method will also be called.
        /// </summary>
        /// <param name="ownerName">The owner name to be added to claims as <see cref="ClaimTypes.Name"/> and <see cref="ClaimTypes.NameIdentifier"/> if not already added with <paramref name="claims"/>.</param>
        /// <param name="claims">Claims to be added to the identity.</param>
        public void ValidationSucceeded(string ownerName, IEnumerable<Claim> claims = null)
        {
            Principal = ApiKeyUtils.BuildClaimsPrincipal(ownerName, Scheme.Name, Options.ClaimsIssuer, claims);
            Success();
        }

        /// <summary>
        /// If parameter <paramref name="failureMessage"/> passed is empty or null then NoResult() method is called 
        /// otherwise, <see cref="ResultContext{TOptions}.Fail(string)"/> method will be called.
        /// </summary>
        /// <param name="failureMessage">(Optional) The failure message.</param>
        public void ValidationFailed(string failureMessage = null) 
        {
            if (string.IsNullOrWhiteSpace(failureMessage))
            {
                NoResult();
                return;
            }
            Fail(failureMessage);
        }

        /// <summary>
        /// Calling this method is same as calling <see cref="ResultContext{TOptions}.Fail(Exception)"/> method.
        /// </summary>
        /// <param name="failureException">The failure exception.</param>
        public void ValidationFailed(Exception failureException)
        {
            Fail(failureException);
        }
    }
}
