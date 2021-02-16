// Copyright (c) Mihir Dilip. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for license information.

using System;
using System.Security.Claims;

namespace AspNetCore.Authentication.ApiKey.Tests.Infrastructure
{
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
