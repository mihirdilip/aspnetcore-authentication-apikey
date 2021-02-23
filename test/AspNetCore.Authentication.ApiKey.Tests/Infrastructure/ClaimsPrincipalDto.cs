// Copyright (c) Mihir Dilip. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace AspNetCore.Authentication.ApiKey.Tests.Infrastructure
{
    [Serializable]
    struct ClaimsPrincipalDto
    {
        public ClaimsPrincipalDto(ClaimsPrincipal user)
        {
            Identity = new ClaimsIdentityDto(user.Identity);
            Identities = user.Identities.Select(i => new ClaimsIdentityDto(i));
            Claims = user.Claims.Select(c => new ClaimDto(c));
        }

        public ClaimsIdentityDto Identity { get; set; }
        public IEnumerable<ClaimsIdentityDto> Identities { get; private set; }
        public IEnumerable<ClaimDto> Claims { get; set; }
    }

    [Serializable]
    struct ClaimsIdentityDto
    {
        public ClaimsIdentityDto(IIdentity identity)
        {
            Name = identity.Name;
            IsAuthenticated = identity.IsAuthenticated;
            AuthenticationType = identity.AuthenticationType;
        }

        public string Name { get; set; }
        public bool IsAuthenticated { get; set; }
        public string AuthenticationType { get; set; }
    }

    [Serializable]
    struct ClaimDto
    {
        public ClaimDto(Claim claim)
        {
            Type = claim.Type;
            Value = claim.Value;
            Issuer = claim.Issuer;
            OriginalIssuer = claim.OriginalIssuer;
        }

        public string Type { get; set; }
        public string Value { get; set; }
        public string Issuer { get; set; }
        public string OriginalIssuer { get; set; }
    }
}
