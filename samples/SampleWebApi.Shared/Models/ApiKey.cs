#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
using AspNetCore.Authentication.ApiKey;
using System.Collections.Generic;
using System.Security.Claims;

namespace SampleWebApi.Models
{
	class ApiKey : IApiKey
	{
        public ApiKey(string key, string owner, List<Claim> claims = null)
        {
			Key = key;
			OwnerName = owner;
			Claims = claims ?? new List<Claim>();
		}

		public string Key { get; }
		public string OwnerName { get; }
		public IReadOnlyCollection<Claim> Claims { get; }
	}
}
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
