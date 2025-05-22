// Copyright (c) Mihir Dilip. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for license information.

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

namespace AspNetCore.Authentication.ApiKey
{
	public class ApiKeyInHeaderHandler : ApiKeyHandlerBase
	{
		private const string WwwAuthenticateInParameter = "header";
		protected override string GetWwwAuthenticateInParameter() => WwwAuthenticateInParameter;

#if NET8_0_OR_GREATER
		protected ApiKeyInHeaderHandler(IOptionsMonitor<ApiKeyOptions> options, ILoggerFactory logger, UrlEncoder encoder)
			: base(options, logger, encoder)
		{
		}

		[Obsolete("ISystemClock is obsolete, use TimeProvider on AuthenticationSchemeOptions instead.")]
#endif
		public ApiKeyInHeaderHandler(IOptionsMonitor<ApiKeyOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
			: base(options, logger, encoder, clock)
		{
		}

		protected override Task<string> ParseApiKeyAsync()
		{
			if (Request.Headers.TryGetValue(Options.KeyName, out var value))
			{
				return Task.FromResult(value.FirstOrDefault() ?? string.Empty);
			}

			// No ApiKey found
			return Task.FromResult(string.Empty);
		}
	}
}