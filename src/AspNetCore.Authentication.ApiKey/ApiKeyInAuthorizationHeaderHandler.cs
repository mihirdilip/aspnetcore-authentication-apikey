// Copyright (c) Mihir Dilip. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for license information.

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;
using System.Text.Encodings.Web;

namespace AspNetCore.Authentication.ApiKey
{
	public class ApiKeyInAuthorizationHeaderHandler : ApiKeyHandlerBase
	{
		private const string WwwAuthenticateInParameter = "authorization_header";
		protected override string GetWwwAuthenticateInParameter() => WwwAuthenticateInParameter;

#if NET8_0_OR_GREATER
		protected ApiKeyInAuthorizationHeaderHandler(IOptionsMonitor<ApiKeyOptions> options, ILoggerFactory logger, UrlEncoder encoder)
			: base(options, logger, encoder)
		{
		}

		[Obsolete("ISystemClock is obsolete, use TimeProvider on AuthenticationSchemeOptions instead.")]
#endif
		public ApiKeyInAuthorizationHeaderHandler(IOptionsMonitor<ApiKeyOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
			: base(options, logger, encoder, clock)
		{
		}

		protected override Task<string> ParseApiKeyAsync()
		{
			if (Request.Headers.TryGetValue(HeaderNames.Authorization, out StringValues value) 
					&& AuthenticationHeaderValue.TryParse(value, out var headerValue)
					&& (headerValue.Scheme.Equals(Scheme.Name, StringComparison.OrdinalIgnoreCase) 
						|| headerValue.Scheme.Equals(Options.KeyName, StringComparison.OrdinalIgnoreCase)
					)
			)
			{
				return Task.FromResult(headerValue.Parameter ?? string.Empty);
			}

			// No ApiKey found
			return Task.FromResult(string.Empty);
		}
	}
}