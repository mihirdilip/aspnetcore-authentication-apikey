// Copyright (c) Mihir Dilip. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AspNetCore.Authentication.ApiKey.Tests.Infrastructure
{
    class FakeApiKeyProvider : IApiKeyProvider
    {
        public Task<IApiKey> ProvideAsync(string key)
        {
            return Task.FromResult(FakeApiKeys.Keys.FirstOrDefault(k => k.Key.Equals(key, StringComparison.OrdinalIgnoreCase)));
        }
    }

    class FakeApiKey : IApiKey
    {
        public FakeApiKey(string key, string ownerName, IReadOnlyCollection<Claim> claims = null)
        {
            Key = key;
            OwnerName = ownerName;
            Claims = claims;
        }

        public string Key { get; }

        public string OwnerName { get; }

        public IReadOnlyCollection<Claim> Claims { get; }
    }

    class FakeApiKeys
    {
        internal const string KeyName = "X-API-KEY";

        internal static string FakeInvalidKey = "<invalid-key>";
        internal static string FakeKey = "myrandomfakekey";
        internal static string FakeKeyOwner = "Fake Key";
        internal static Claim FakeNameClaim = new Claim(ClaimTypes.Name, "FakeNameClaim", ClaimValueTypes.String);
        internal static Claim FakeNameIdentifierClaim = new Claim(ClaimTypes.NameIdentifier, "FakeNameIdentifierClaim", ClaimValueTypes.String);
        internal static Claim FakeRoleClaim = new Claim(ClaimTypes.Role, "FakeRoleClaim", ClaimValueTypes.String);

        internal static List<IApiKey> Keys => new List<IApiKey>
        {
            new FakeApiKey(FakeKey, FakeKeyOwner, new List<Claim> { FakeNameClaim, FakeNameIdentifierClaim, FakeRoleClaim })
        };
    }
}
