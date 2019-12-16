// Copyright (c) Mihir Dilip. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Authentication;

namespace AspNetCore.Authentication.ApiKey
{
	/// <summary>
	/// Inherited from <see cref="AuthenticationSchemeOptions"/> to allow extra option properties for 'ApiKey' authentication.
	/// </summary>
	public class ApiKeyOptions : AuthenticationSchemeOptions
	{
		/// <summary>
		/// This is required property. It is used when challenging un-authenticated requests.
		/// </summary>
		public string Realm { get; set; }

		/// <summary>
		/// This is required property. It is the name of the header or query parameter of the API Key.
		/// </summary>
		public string KeyName { get; set; }
	}
}
