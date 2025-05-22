// Copyright (c) Mihir Dilip. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for license information.

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace AspNetCore.Authentication.ApiKey
{
    /// <summary>
    /// Context used when authentication is failed.
    /// </summary>
    public class ApiKeyAuthenticationFailedContext : ResultContext<ApiKeyOptions>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scheme"></param>
        /// <param name="options"></param>
        /// <param name="exception"></param>
        public ApiKeyAuthenticationFailedContext(HttpContext context, AuthenticationScheme scheme, ApiKeyOptions options, Exception exception)
            : base(context, scheme, options)
        {
            Exception = exception;
        }

        /// <summary>
        /// The Exception thrown when authenticating.
        /// </summary>
        public Exception Exception { get; }
    }
}
