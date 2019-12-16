// Copyright (c) Mihir Dilip. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace AspNetCore.Authentication.ApiKey
{
	/// <summary>
	/// Default values used by api key authentication.
	/// </summary>
	public static class ApiKeyDefaults
	{
		/// <summary>
		/// Default value for AuthenticationScheme 
		/// </summary>
		public const string AuthenticationScheme = "ApiKey";

		internal const string InHeaderAuthenticationScheme = AuthenticationScheme + "-InHeader";
		internal const string InQueryParamsAuthenticationScheme = AuthenticationScheme + "-InQueryParams";
		internal const string InHeaderOrQueryParamsAuthenticationScheme = AuthenticationScheme + "-InHeaderOrQueryParams";
	}
}
