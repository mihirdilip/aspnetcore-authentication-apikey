// Copyright (c) Mihir Dilip. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace AspNetCore.Authentication.ApiKey
{
	internal class ApiKeyInQueryParamsHandler : ApiKeyHandlerBase
	{
		public ApiKeyInQueryParamsHandler(IOptionsMonitor<ApiKeyOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IApiKeyProvider apiKeyValidationService)
			: base(options, logger, encoder, clock, apiKeyValidationService)
		{
		}

		protected override string AuthenticationScheme { get; } = ApiKeyDefaults.InQueryParamsAuthenticationScheme;

		/// <summary>
		/// Searches the query parameters for 'ApiKey' value of which is validated using implementation of <see cref="IApiKeyProvider"/> passed as type parameter when setting up ApiKey authentication in the Startup.cs 
		/// </summary>
		/// <returns></returns>
		protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
		{
			if (!Request.Query.TryGetValue(Options.KeyName, out var value))
			{
				// No ApiKey query parameter found
				return AuthenticateResult.NoResult();
			}

			var key = value.FirstOrDefault();
			return await HandleAuthenticateAsync(key).ConfigureAwait(false);
		}
	}
}