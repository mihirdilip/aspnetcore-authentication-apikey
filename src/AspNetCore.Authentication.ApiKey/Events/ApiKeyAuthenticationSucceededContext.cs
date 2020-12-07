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
    /// Context used when authentication is succeeded.
    /// </summary>
    public class ApiKeyAuthenticationSucceededContext : ResultContext<ApiKeyOptions>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scheme"></param>
        /// <param name="options"></param>
        /// <param name="principal"></param>
        public ApiKeyAuthenticationSucceededContext(HttpContext context, AuthenticationScheme scheme, ApiKeyOptions options, ClaimsPrincipal principal)
            : base(context, scheme, options)
        {
            base.Principal = principal;
        }

        /// <summary>
        /// Get the <see cref="ClaimsPrincipal"/> containing the user claims.
        /// </summary>
        public new ClaimsPrincipal Principal => base.Principal;

        /// <summary>
        /// Called to replace the claims principal. The supplied principal will replace the value of the 
        /// Principal property, which determines the identity of the authenticated request.
        /// </summary>
        /// <param name="principal">The <see cref="ClaimsPrincipal"/> to be used as the replacement.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void ReplacePrincipal(ClaimsPrincipal principal)
        {
            base.Principal = principal ?? throw new ArgumentNullException(nameof(principal));
        }

        /// <summary>
        /// Called to reject the incoming principal. This may be done if the application has determined the
        /// account is no longer active, and the request should be treated as if it was anonymous.
        /// </summary>
        public void RejectPrincipal() => base.Principal = null;

        /// <summary>
        /// Adds a claim to the current authenticated identity.
        /// </summary>
        /// <param name="claim"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void AddClaim(Claim claim)
        {
            if (claim == null) throw new ArgumentNullException(nameof(claim));
            (Principal?.Identity as ClaimsIdentity).AddClaim(claim);
        }

        /// <summary>
        /// Adds a list of claims to the current authenticated identity. 
        /// </summary>
        /// <param name="claims"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void AddClaims(IEnumerable<Claim> claims)
        {
            if (claims == null) throw new ArgumentNullException(nameof(claims));
            (Principal?.Identity as ClaimsIdentity).AddClaims(claims);
        }
    }
}
