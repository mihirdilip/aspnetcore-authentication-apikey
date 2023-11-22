#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
using AspNetCore.Authentication.ApiKey;
using SampleWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApi.Repositories
{
    /// <summary>
    /// NOTE: DO NOT USE THIS IMPLEMENTATION. THIS IS FOR DEMO PURPOSE ONLY
    /// </summary>
    public class InMemoryApiKeyRepository : IApiKeyRepository
	{
        private readonly List<IApiKey> _cache = new List<IApiKey>()
        {
			new ApiKey("Key1", "Admin"),
			new ApiKey("Key2", "User"),
		};

		public Task<IApiKey> GetApiKeyAsync(string key)
		{
			var apiKey = _cache.FirstOrDefault(k => k.Key.Equals(key, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(apiKey);
        }
    }
}
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
