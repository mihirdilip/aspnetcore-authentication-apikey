// Copyright (c) Mihir Dilip. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for license information.

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace AspNetCore.Authentication.ApiKey
{
    internal class ApiKeyInHeaderOrQueryParamsHandler : ApiKeyHandlerBase
	{
		public ApiKeyInHeaderOrQueryParamsHandler(IOptionsMonitor<ApiKeyOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) 
			: base(options, logger, encoder, clock)
		{
		}

        protected override Task<string> ParseApiKeyAsync()
        {
			// Try query parameter
			if (Request.Query.TryGetValue(Options.KeyName, out var value))
			{
				return Task.FromResult(value.FirstOrDefault());
			}

			// No ApiKey query parameter found try headers
			if (Request.Headers.TryGetValue(Options.KeyName, out var headerValue))
			{
				return Task.FromResult(headerValue.FirstOrDefault());
			}

			// No ApiKey found
			return Task.FromResult(string.Empty);
		}
    }
}