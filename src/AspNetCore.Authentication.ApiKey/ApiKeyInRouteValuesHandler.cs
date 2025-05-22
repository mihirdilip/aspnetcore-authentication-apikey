﻿// Copyright (c) Mihir Dilip. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for license information.
#if NETCOREAPP3_0_OR_GREATER

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

namespace AspNetCore.Authentication.ApiKey
{
	public class ApiKeyInRouteValuesHandler : ApiKeyHandlerBase
	{
		private const string WwwAuthenticateInParameter = "route_values";
		protected override string GetWwwAuthenticateInParameter() => WwwAuthenticateInParameter;

#if NET8_0_OR_GREATER
		protected ApiKeyInRouteValuesHandler(IOptionsMonitor<ApiKeyOptions> options, ILoggerFactory logger, UrlEncoder encoder)
			: base(options, logger, encoder)
		{
		}

		[Obsolete("ISystemClock is obsolete, use TimeProvider on AuthenticationSchemeOptions instead.")]
#endif
		public ApiKeyInRouteValuesHandler(IOptionsMonitor<ApiKeyOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
			: base(options, logger, encoder, clock)
		{
		}

		protected override Task<string> ParseApiKeyAsync()
		{
			if (Request.RouteValues.TryGetValue(Options.KeyName, out var value) && value != null && value.GetType() == typeof(string))
			{
				return Task.FromResult(value.ToString() ?? string.Empty);
			}

			// No ApiKey query parameter found
			return Task.FromResult(string.Empty);
		}
	}
}
#endif