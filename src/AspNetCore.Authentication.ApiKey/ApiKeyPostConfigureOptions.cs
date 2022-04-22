// Copyright (c) Mihir Dilip. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for license information.

using System;
using Microsoft.Extensions.Options;

namespace AspNetCore.Authentication.ApiKey
{
	using Microsoft.Extensions.DependencyInjection;

	/// <summary>
	/// This post configure options checks whether the required option properties are set or not on <see cref="ApiKeyOptions"/>.
	/// </summary>
	class ApiKeyPostConfigureOptions : IPostConfigureOptions<ApiKeyOptions>
	{
		private readonly IServiceProvider serviceProvider;

		public ApiKeyPostConfigureOptions(IServiceProvider serviceProvider)
		{
			this.serviceProvider = serviceProvider;
		}

		public void PostConfigure(string name, ApiKeyOptions options)
		{
			if (!options.SuppressWWWAuthenticateHeader && string.IsNullOrWhiteSpace(options.Realm))
			{
				throw new InvalidOperationException($"{nameof(ApiKeyOptions.Realm)} must be set in {nameof(ApiKeyOptions)} when setting up the authentication.");
			}

			if (string.IsNullOrWhiteSpace(options.KeyName))
			{
				throw new InvalidOperationException($"{nameof(ApiKeyOptions.KeyName)} must be set in {nameof(ApiKeyOptions)} when setting up the authentication.");
			}

			var apiKeyProviderFactory = this.serviceProvider.GetService<IApiKeyProviderFactory>();
			if (options.Events?.OnValidateKey == null && options.EventsType == null && options.ApiKeyProviderType == null && apiKeyProviderFactory == null)
			{
				throw new InvalidOperationException($"Either {nameof(ApiKeyOptions.Events.OnValidateKey)} delegate on configure options {nameof(ApiKeyOptions.Events)} should be set or use an extension method with type parameter of type {nameof(IApiKeyProvider)} or register an implementation of type {nameof(IApiKeyProviderFactory)} in the service collection.");
			}
		}
	}
}
