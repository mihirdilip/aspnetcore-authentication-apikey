// Copyright (c) Mihir Dilip. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for license information.

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace AspNetCore.Authentication.ApiKey
{
	public class ApiKeyInQueryParamsHandler : ApiKeyHandlerBase
	{
#if NET8_0_OR_GREATER
        protected ApiKeyInQueryParamsHandler(IOptionsMonitor<ApiKeyOptions> options, ILoggerFactory logger, UrlEncoder encoder)
            : base(options, logger, encoder)
        {
        }

        [Obsolete("ISystemClock is obsolete, use TimeProvider on AuthenticationSchemeOptions instead.")]
#endif
        public ApiKeyInQueryParamsHandler(IOptionsMonitor<ApiKeyOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
			: base(options, logger, encoder, clock)
		{
		}

		protected override Task<string> ParseApiKeyAsync()
		{
			if (Request.Query.TryGetValue(Options.KeyName, out var value))
			{
				return Task.FromResult(value.FirstOrDefault());
			}

			// No ApiKey query parameter found
			return Task.FromResult(string.Empty);
		}
	}
}