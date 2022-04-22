// Copyright (c) Matthias Gernand. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for license information.

namespace AspNetCore.Authentication.ApiKey
{
	/// <summary>
	/// Implementation of this interface will be used by the 'ApiKey' authentication handler to get a schema specific <see cref="IApiKeyProvider"/>.
	/// </summary>
	public interface IApiKeyProviderFactory
	{
		/// <summary>
		/// Implementation of the service creation logic.
		/// </summary>
		/// <param name="authenticationSchemaName"></param>
		/// <returns></returns>
		IApiKeyProvider CreateApiKeyProvider(string authenticationSchemaName);
	}
}
