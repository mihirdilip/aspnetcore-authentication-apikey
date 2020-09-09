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
    internal class ApiKeyInHeaderHandler : ApiKeyHandlerBase
	{
		public ApiKeyInHeaderHandler(IOptionsMonitor<ApiKeyOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) 
			: base(options, logger, encoder, clock)
		{
		}

        protected override Task<string> ParseApiKeyAsync()
        {
			if (Request.Headers.TryGetValue(Options.KeyName, out var value))
			{
				return Task.FromResult(value.FirstOrDefault());
			}

			// No ApiKey found
			return Task.FromResult(string.Empty);
		}
    }
}