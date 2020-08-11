// Copyright (c) Mihir Dilip. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Threading.Tasks;

namespace AspNetCore.Authentication.ApiKey
{
	/// <summary>
	/// Implementation of this interface will be used by the 'ApiKey' authentication handler to validated and get details from the key.
	/// </summary>
	public interface IApiKeyProvider
	{
		/// <summary>
		/// Validates the key and provides with and instance of <see cref="IApiKey"/>.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		Task<IApiKey> ProvideAsync();
	}
}
