// Copyright (c) Mihir Dilip. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for license information.

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Net.Http.Headers;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace AspNetCore.Authentication.ApiKey
{
	internal class ApiKeyInAuthorizationHeaderHandler : ApiKeyHandlerBase
	{
		public ApiKeyInAuthorizationHeaderHandler(IOptionsMonitor<ApiKeyOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
			: base(options, logger, encoder, clock)
		{
		}

		protected override Task<string> ParseApiKeyAsync()
		{
			if (Request.Headers.ContainsKey(HeaderNames.Authorization)
					&& AuthenticationHeaderValue.TryParse(Request.Headers[HeaderNames.Authorization], out var headerValue)
					&& headerValue.Scheme.Equals(Options.KeyName, StringComparison.OrdinalIgnoreCase)
			)
			{
				return Task.FromResult(headerValue.Parameter);
			}

			// No ApiKey found
			return Task.FromResult(string.Empty);
		}
	}
}