// Copyright (c) Mihir Dilip. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for license information.

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace AspNetCore.Authentication.ApiKey
{
    /// <summary>
    /// Context used when handling forbidden response.
    /// </summary>
    public class ApiKeyHandleForbiddenContext : PropertiesContext<ApiKeyOptions>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scheme"></param>
        /// <param name="options"></param>
        /// <param name="properties"></param>
        public ApiKeyHandleForbiddenContext(HttpContext context, AuthenticationScheme scheme, ApiKeyOptions options, AuthenticationProperties properties)
            : base(context, scheme, options, properties)
        {
        }

        /// <summary>
        /// Gets IsHandled property. 
        /// True means response is handled and any default logic will be skipped.
        /// </summary>
        public bool IsHandled { get; private set; }

        /// <summary>
        /// Marks as response handled and any default logic will be skipped.
        /// </summary>
        public void Handled() => IsHandled = true;
    }
}
