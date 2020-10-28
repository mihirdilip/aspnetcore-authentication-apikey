// Copyright (c) Mihir Dilip. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for license information.

using System;
using Microsoft.Extensions.Options;

namespace AspNetCore.Authentication.ApiKey
{
	/// <summary>
	/// This post configure options checks whether the required option properties are set or not on <see cref="ApiKeyOptions"/>.
	/// </summary>
	class ApiKeyPostConfigureOptions : IPostConfigureOptions<ApiKeyOptions>
	{
		public void PostConfigure(string name, ApiKeyOptions options)
		{
			if (!options.SuppressWWWAuthenticateHeader && string.IsNullOrWhiteSpace(options.Realm))
			{
				throw new InvalidOperationException($"{nameof(ApiKeyOptions.Realm)} must be set in {typeof(ApiKeyOptions).Name} when setting up the authentication.");
			}

			if (string.IsNullOrWhiteSpace(options.KeyName))
			{
				throw new InvalidOperationException($"{nameof(ApiKeyOptions.KeyName)} must be set in {typeof(ApiKeyOptions).Name} when setting up the authentication.");
			}

			if (options.Events?.OnValidateKey == null && options.EventsType == null && options.ApiKeyProviderType == null)
			{
				throw new InvalidOperationException($"Either {nameof(ApiKeyOptions.Events.OnValidateKey)} delegate on configure options {nameof(ApiKeyOptions.Events)} should be set or use an extention method with type parameter of type {nameof(IApiKeyProvider)}.");
			}
		}
	}
}
