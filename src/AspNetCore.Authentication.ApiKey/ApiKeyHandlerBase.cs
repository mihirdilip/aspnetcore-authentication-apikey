// Copyright (c) Mihir Dilip. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace AspNetCore.Authentication.ApiKey
{
	/// <summary>
	/// Inherited from <see cref="AuthenticationHandler{TOptions}"/> for api key authentication.
	/// </summary>
	internal abstract class ApiKeyHandlerBase : AuthenticationHandler<ApiKeyOptions>
	{
		private readonly IApiKeyProvider _apiKeyValidationService;

		protected ApiKeyHandlerBase(IOptionsMonitor<ApiKeyOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IApiKeyProvider apiKeyValidationService) 
			: base(options, logger, encoder, clock)
		{
			_apiKeyValidationService = apiKeyValidationService;
		}

		protected abstract string AuthenticationScheme { get; }

		private string Challenge => $"{AuthenticationScheme} realm=\"{Options.Realm}\", charset=\"UTF-8\"";

		protected async Task<AuthenticateResult> HandleAuthenticateAsync(string key)
		{
			if (string.IsNullOrWhiteSpace(key))
			{
				return AuthenticateResult.Fail("Invalid ApiKey authentication header.");
			}

			// Validate key by using the implementation of IApiKeyValidationService.
			var validatedKey = await _apiKeyValidationService.ProvideAsync(key).ConfigureAwait(false);
			if (validatedKey == null)
			{
				return AuthenticateResult.Fail("Invalid API Key provided.");
			}

			// Create 'AuthenticationTicket' and return as success if the above validation was successful.
			var claims = new List<Claim>();
			if (validatedKey.Claims != null)
			{
				claims.AddRange(validatedKey.Claims);
			}

			if (!string.IsNullOrWhiteSpace(validatedKey.OwnerName) && !claims.Exists(c => c.Type.Equals(ClaimTypes.Name, StringComparison.OrdinalIgnoreCase)))
			{
				claims.Add(new Claim(ClaimTypes.Name, validatedKey.OwnerName));
			}

			var identity = new ClaimsIdentity(claims, Scheme.Name);
			var principal = new ClaimsPrincipal(identity);
			var ticket = new AuthenticationTicket(principal, Scheme.Name);
			return AuthenticateResult.Success(ticket);
		}

		/// <summary>
		/// Handles the un-authenticated requests. 
		/// Returns 401 status code in response.
		/// Adds 'WWW-Authenticate' with 'Basic' authentication scheme and 'Realm' in the response header 
		/// to let the client know that 'Basic' authentication scheme is being used by the system.
		/// </summary>
		/// <param name="properties"></param>
		/// <returns></returns>
		protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
		{
			Response.Headers[HeaderNames.WWWAuthenticate] = Challenge;
			await base.HandleChallengeAsync(properties);
		}
	}
}
