// Copyright (c) Mihir Dilip. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace AspNetCore.Authentication.ApiKey
{
	/// <summary>
	/// Extension methods for api key authentication.
	/// </summary>
	public static class ApiKeyExtensions
	{
		/// <summary>
		/// Adds API Key - In Header authentication scheme to the project. It takes a implementation of <see cref="IApiKeyProvider"/> as type parameter.
		/// </summary>
		/// <typeparam name="TApiKeyProvider"></typeparam>
		/// <param name="builder"></param>
		/// <param name="configureOptions">Sets the <see cref="ApiKeyOptions"/>. Realm option property must be set.</param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
		public static AuthenticationBuilder AddApiKeyInHeader<TApiKeyProvider>(this AuthenticationBuilder builder, Action<ApiKeyOptions> configureOptions)
			where TApiKeyProvider : class, IApiKeyProvider
		{
			return builder.AddApiKey<TApiKeyProvider, ApiKeyInHeaderHandler>(configureOptions);
		}

		/// <summary>
		/// Adds API Key - In Query Parameters authentication scheme to the project. It takes a implementation of <see cref="IApiKeyProvider"/> as type parameter.
		/// </summary>
		/// <typeparam name="TApiKeyProvider"></typeparam>
		/// <param name="builder"></param>
		/// <param name="configureOptions">Sets the <see cref="ApiKeyOptions"/>. Realm option property must be set.</param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
		public static AuthenticationBuilder AddApiKeyInQueryParams<TApiKeyProvider>(this AuthenticationBuilder builder, Action<ApiKeyOptions> configureOptions)
			where TApiKeyProvider : class, IApiKeyProvider
		{
			return builder.AddApiKey<TApiKeyProvider, ApiKeyInQueryParamsHandler>(configureOptions);
		}

		/// <summary>
		/// Adds API Key - In Header Or Query Parameters authentication scheme to the project. It takes a implementation of <see cref="IApiKeyProvider"/> as type parameter.
		/// </summary>
		/// <typeparam name="TApiKeyProvider"></typeparam>
		/// <param name="builder"></param>
		/// <param name="configureOptions">Sets the <see cref="ApiKeyOptions"/>. Realm option property must be set.</param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
		public static AuthenticationBuilder AddApiKeyInHeaderOrQueryParams<TApiKeyProvider>(this AuthenticationBuilder builder, Action<ApiKeyOptions> configureOptions)
			where TApiKeyProvider : class, IApiKeyProvider
		{
			return builder.AddApiKey<TApiKeyProvider, ApiKeyInHeaderOrQueryParamsHandler>(configureOptions);
		}

		private static AuthenticationBuilder AddApiKey<TApiKeyProvider, TApiKeyHandler>(this AuthenticationBuilder builder, Action<ApiKeyOptions> configureOptions)
			where TApiKeyProvider : class, IApiKeyProvider 
			where TApiKeyHandler : AuthenticationHandler<ApiKeyOptions>
		{
			// Adds post configure options to the pipeline.
			builder.Services.AddSingleton<IPostConfigureOptions<ApiKeyOptions>, ApiKeyPostConfigureOptions>();

			// Adds implementation of IBasicUserValidationService to the dependency container.
			builder.Services.AddTransient<IApiKeyProvider, TApiKeyProvider>();

			// Adds a default validation for Api keys. 
			// If a different implementation for IApiKeyValidator is needed then that other implementation just has to be registered afterwards
			builder.Services.AddTransient<IApiKeyValidator, ApiKeyValidator>();

			// Adds basic authentication scheme to the pipeline.
			return builder.AddScheme<ApiKeyOptions, TApiKeyHandler>(ApiKeyDefaults.AuthenticationScheme, configureOptions);
		}
	}
}
