// Copyright (c) Mihir Dilip. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for license information.

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;

namespace AspNetCore.Authentication.ApiKey
{
	/// <summary>
	/// Extension methods for api key authentication.
	/// </summary>
	public static class ApiKeyExtensions
	{
		#region API Key - In Header

		/// <summary>
		/// Adds API Key - In Header authentication scheme to the project.
		/// <see cref="ApiKeyEvents.OnValidateKey"/> delegate must be set on the <see cref="ApiKeyOptions.Events"/>.
		/// </summary>
		/// <param name="builder"></param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
		public static AuthenticationBuilder AddApiKeyInHeader(this AuthenticationBuilder builder)
			=> builder.AddApiKeyInHeader(ApiKeyDefaults.AuthenticationScheme);

		/// <summary>
		/// Adds API Key - In Header authentication scheme to the project.
		/// <see cref="ApiKeyEvents.OnValidateKey"/> delegate must be set on the <see cref="ApiKeyOptions.Events"/>.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="authenticationScheme">The authentication scheme.</param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
		public static AuthenticationBuilder AddApiKeyInHeader(this AuthenticationBuilder builder, string authenticationScheme)
			=> builder.AddApiKeyInHeader(authenticationScheme, configureOptions: null);

		/// <summary>
		/// Adds API Key - In Header authentication scheme to the project.
		/// <see cref="ApiKeyEvents.OnValidateKey"/> delegate must be set on the Events property on <paramref name="configureOptions"/>.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="configureOptions">The configure options.</param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
		public static AuthenticationBuilder AddApiKeyInHeader(this AuthenticationBuilder builder, Action<ApiKeyOptions>? configureOptions)
			=> builder.AddApiKeyInHeader(ApiKeyDefaults.AuthenticationScheme, configureOptions);

		/// <summary>
		/// Adds API Key - In Header authentication scheme to the project.
		/// <see cref="ApiKeyEvents.OnValidateKey"/> delegate must be set on the Events property on <paramref name="configureOptions"/>.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="authenticationScheme">The authentication scheme.</param>
		/// <param name="configureOptions">The configure options.</param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
		public static AuthenticationBuilder AddApiKeyInHeader(this AuthenticationBuilder builder, string authenticationScheme, Action<ApiKeyOptions>? configureOptions)
			=> builder.AddApiKeyInHeader(authenticationScheme, displayName: null, configureOptions: configureOptions);

		/// <summary>
		/// Adds API Key - In Header authentication scheme to the project.
		/// <see cref="ApiKeyEvents.OnValidateKey"/> delegate must be set on the Events property on <paramref name="configureOptions"/>.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="authenticationScheme">The authentication scheme.</param>
		/// <param name="displayName">The display name.</param>
		/// <param name="configureOptions">The configure options.</param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
		public static AuthenticationBuilder AddApiKeyInHeader(this AuthenticationBuilder builder, string authenticationScheme, string? displayName, Action<ApiKeyOptions>? configureOptions)
			=> builder.AddApiKey<ApiKeyInHeaderHandler>(authenticationScheme, displayName, configureOptions);





		/// <summary>
		/// Adds API Key - In Header authentication scheme to the project. It takes a implementation of <see cref="IApiKeyProvider"/> as type parameter.
		/// If <see cref="ApiKeyEvents.OnValidateKey"/> delegate is set on the <see cref="ApiKeyOptions.Events"/> then it will be used instead of implementation of <see cref="IApiKeyProvider"/>.
		/// </summary>
		/// <typeparam name="TApiKeyProvider"></typeparam>
		/// <param name="builder"></param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
#if NET5_0_OR_GREATER
		public static AuthenticationBuilder AddApiKeyInHeader<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApiKeyProvider>(this AuthenticationBuilder builder) where TApiKeyProvider : class, IApiKeyProvider
			=> builder.AddApiKeyInHeader<TApiKeyProvider>(ApiKeyDefaults.AuthenticationScheme);
#else
		public static AuthenticationBuilder AddApiKeyInHeader<TApiKeyProvider>(this AuthenticationBuilder builder) where TApiKeyProvider : class, IApiKeyProvider
			=> builder.AddApiKeyInHeader<TApiKeyProvider>(ApiKeyDefaults.AuthenticationScheme);
#endif

		/// <summary>
		/// Adds API Key - In Header authentication scheme to the project. It takes a implementation of <see cref="IApiKeyProvider"/> as type parameter.
		/// If <see cref="ApiKeyEvents.OnValidateKey"/> delegate is set on the <see cref="ApiKeyOptions.Events"/> then it will be used instead of implementation of <see cref="IApiKeyProvider"/>.
		/// </summary>
		/// <typeparam name="TApiKeyProvider"></typeparam>
		/// <param name="builder"></param>
		/// <param name="authenticationScheme">The authentication scheme.</param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
#if NET5_0_OR_GREATER
		public static AuthenticationBuilder AddApiKeyInHeader<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApiKeyProvider>(this AuthenticationBuilder builder, string authenticationScheme) where TApiKeyProvider : class, IApiKeyProvider
			=> builder.AddApiKeyInHeader<TApiKeyProvider>(authenticationScheme, configureOptions: null);
#else
		public static AuthenticationBuilder AddApiKeyInHeader<TApiKeyProvider>(this AuthenticationBuilder builder, string authenticationScheme) where TApiKeyProvider : class, IApiKeyProvider
			=> builder.AddApiKeyInHeader<TApiKeyProvider>(authenticationScheme, configureOptions: null);
#endif

		/// <summary>
		/// Adds API Key - In Header authentication scheme to the project. It takes a implementation of <see cref="IApiKeyProvider"/> as type parameter.
		/// If <see cref="ApiKeyEvents.OnValidateKey"/> delegate is set on the Events property on <paramref name="configureOptions"/> then it will be used instead of implementation of <see cref="IApiKeyProvider"/>.
		/// </summary>
		/// <typeparam name="TApiKeyProvider"></typeparam>
		/// <param name="builder"></param>
		/// <param name="configureOptions">The <see cref="ApiKeyOptions"/>.</param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
#if NET5_0_OR_GREATER
		public static AuthenticationBuilder AddApiKeyInHeader<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApiKeyProvider>(this AuthenticationBuilder builder, Action<ApiKeyOptions>? configureOptions) where TApiKeyProvider : class, IApiKeyProvider
			=> builder.AddApiKeyInHeader<TApiKeyProvider>(ApiKeyDefaults.AuthenticationScheme, configureOptions);
#else
		public static AuthenticationBuilder AddApiKeyInHeader<TApiKeyProvider>(this AuthenticationBuilder builder, Action<ApiKeyOptions>? configureOptions) where TApiKeyProvider : class, IApiKeyProvider
			=> builder.AddApiKeyInHeader<TApiKeyProvider>(ApiKeyDefaults.AuthenticationScheme, configureOptions);
#endif

		/// <summary>
		/// Adds API Key - In Header authentication scheme to the project. It takes a implementation of <see cref="IApiKeyProvider"/> as type parameter.
		/// If <see cref="ApiKeyEvents.OnValidateKey"/> delegate is set on the Events property on <paramref name="configureOptions"/> then it will be used instead of implementation of <see cref="IApiKeyProvider"/>.
		/// </summary>
		/// <typeparam name="TApiKeyProvider"></typeparam>
		/// <param name="builder"></param>
		/// <param name="authenticationScheme">The authentication scheme.</param>
		/// <param name="configureOptions">The <see cref="ApiKeyOptions"/>.</param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
#if NET5_0_OR_GREATER
		public static AuthenticationBuilder AddApiKeyInHeader<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApiKeyProvider>(this AuthenticationBuilder builder, string authenticationScheme, Action<ApiKeyOptions>? configureOptions) where TApiKeyProvider : class, IApiKeyProvider
			=> builder.AddApiKeyInHeader<TApiKeyProvider>(authenticationScheme, displayName: null, configureOptions: configureOptions);
#else
		public static AuthenticationBuilder AddApiKeyInHeader<TApiKeyProvider>(this AuthenticationBuilder builder, string authenticationScheme, Action<ApiKeyOptions>? configureOptions) where TApiKeyProvider : class, IApiKeyProvider
			=> builder.AddApiKeyInHeader<TApiKeyProvider>(authenticationScheme, displayName: null, configureOptions: configureOptions);
#endif

		/// <summary>
		/// Adds API Key - In Header authentication scheme to the project. It takes a implementation of <see cref="IApiKeyProvider"/> as type parameter.
		/// If <see cref="ApiKeyEvents.OnValidateKey"/> delegate is set on the Events property on <paramref name="configureOptions"/> then it will be used instead of implementation of <see cref="IApiKeyProvider"/>.
		/// </summary>
		/// <typeparam name="TApiKeyProvider"></typeparam>
		/// <param name="builder"></param>
		/// <param name="authenticationScheme">The authentication scheme.</param>
		/// <param name="displayName">The display name.</param>
		/// <param name="configureOptions">The <see cref="ApiKeyOptions"/>.</param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
#if NET5_0_OR_GREATER
		public static AuthenticationBuilder AddApiKeyInHeader<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApiKeyProvider>(this AuthenticationBuilder builder, string authenticationScheme, string? displayName, Action<ApiKeyOptions>? configureOptions) where TApiKeyProvider : class, IApiKeyProvider
			=> builder.AddApiKey<TApiKeyProvider, ApiKeyInHeaderHandler>(authenticationScheme, displayName, configureOptions);
#else
		public static AuthenticationBuilder AddApiKeyInHeader<TApiKeyProvider>(this AuthenticationBuilder builder, string authenticationScheme, string? displayName, Action<ApiKeyOptions>? configureOptions) where TApiKeyProvider : class, IApiKeyProvider
			=> builder.AddApiKey<TApiKeyProvider, ApiKeyInHeaderHandler>(authenticationScheme, displayName, configureOptions);
#endif

#endregion // API Key - In Header

		#region API Key - In Authorization Header

		/// <summary>
		/// Adds API Key - In Authorization Header authentication scheme to the project.
		/// <see cref="ApiKeyEvents.OnValidateKey"/> delegate must be set on the <see cref="ApiKeyOptions.Events"/>.
		/// </summary>
		/// <param name="builder"></param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
		public static AuthenticationBuilder AddApiKeyInAuthorizationHeader(this AuthenticationBuilder builder)
			=> builder.AddApiKeyInAuthorizationHeader(ApiKeyDefaults.AuthenticationScheme);

		/// <summary>
		/// Adds API Key - In Authorization Header authentication scheme to the project.
		/// <see cref="ApiKeyEvents.OnValidateKey"/> delegate must be set on the <see cref="ApiKeyOptions.Events"/>.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="authenticationScheme">The authentication scheme.</param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
		public static AuthenticationBuilder AddApiKeyInAuthorizationHeader(this AuthenticationBuilder builder, string authenticationScheme)
			=> builder.AddApiKeyInAuthorizationHeader(authenticationScheme, configureOptions: null);

		/// <summary>
		/// Adds API Key - In Authorization Header authentication scheme to the project.
		/// <see cref="ApiKeyEvents.OnValidateKey"/> delegate must be set on the Events property on <paramref name="configureOptions"/>.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="configureOptions">The configure options.</param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
		public static AuthenticationBuilder AddApiKeyInAuthorizationHeader(this AuthenticationBuilder builder, Action<ApiKeyOptions>? configureOptions)
			=> builder.AddApiKeyInAuthorizationHeader(ApiKeyDefaults.AuthenticationScheme, configureOptions);

		/// <summary>
		/// Adds API Key - In Authorization Header authentication scheme to the project.
		/// <see cref="ApiKeyEvents.OnValidateKey"/> delegate must be set on the Events property on <paramref name="configureOptions"/>.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="authenticationScheme">The authentication scheme.</param>
		/// <param name="configureOptions">The configure options.</param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
		public static AuthenticationBuilder AddApiKeyInAuthorizationHeader(this AuthenticationBuilder builder, string authenticationScheme, Action<ApiKeyOptions>? configureOptions)
			=> builder.AddApiKeyInAuthorizationHeader(authenticationScheme, displayName: null, configureOptions: configureOptions);

		/// <summary>
		/// Adds API Key - In Authorization Header authentication scheme to the project.
		/// <see cref="ApiKeyEvents.OnValidateKey"/> delegate must be set on the Events property on <paramref name="configureOptions"/>.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="authenticationScheme">The authentication scheme.</param>
		/// <param name="displayName">The display name.</param>
		/// <param name="configureOptions">The configure options.</param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
		public static AuthenticationBuilder AddApiKeyInAuthorizationHeader(this AuthenticationBuilder builder, string authenticationScheme, string? displayName, Action<ApiKeyOptions>? configureOptions)
			=> builder.AddApiKey<ApiKeyInAuthorizationHeaderHandler>(authenticationScheme, displayName, configureOptions);





		/// <summary>
		/// Adds API Key - In Authorization Header authentication scheme to the project. It takes a implementation of <see cref="IApiKeyProvider"/> as type parameter.
		/// If <see cref="ApiKeyEvents.OnValidateKey"/> delegate is set on the <see cref="ApiKeyOptions.Events"/> then it will be used instead of implementation of <see cref="IApiKeyProvider"/>.
		/// </summary>
		/// <typeparam name="TApiKeyProvider"></typeparam>
		/// <param name="builder"></param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
#if NET5_0_OR_GREATER
		public static AuthenticationBuilder AddApiKeyInAuthorizationHeader<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApiKeyProvider>(this AuthenticationBuilder builder) where TApiKeyProvider : class, IApiKeyProvider
			=> builder.AddApiKeyInAuthorizationHeader<TApiKeyProvider>(ApiKeyDefaults.AuthenticationScheme);
#else
		public static AuthenticationBuilder AddApiKeyInAuthorizationHeader<TApiKeyProvider>(this AuthenticationBuilder builder) where TApiKeyProvider : class, IApiKeyProvider
			=> builder.AddApiKeyInAuthorizationHeader<TApiKeyProvider>(ApiKeyDefaults.AuthenticationScheme);
#endif

		/// <summary>
		/// Adds API Key - In Authorization Header authentication scheme to the project. It takes a implementation of <see cref="IApiKeyProvider"/> as type parameter.
		/// If <see cref="ApiKeyEvents.OnValidateKey"/> delegate is set on the <see cref="ApiKeyOptions.Events"/> then it will be used instead of implementation of <see cref="IApiKeyProvider"/>.
		/// </summary>
		/// <typeparam name="TApiKeyProvider"></typeparam>
		/// <param name="builder"></param>
		/// <param name="authenticationScheme">The authentication scheme.</param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
#if NET5_0_OR_GREATER
		public static AuthenticationBuilder AddApiKeyInAuthorizationHeader<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApiKeyProvider>(this AuthenticationBuilder builder, string authenticationScheme) where TApiKeyProvider : class, IApiKeyProvider
			=> builder.AddApiKeyInAuthorizationHeader<TApiKeyProvider>(authenticationScheme, configureOptions: null);
#else
		public static AuthenticationBuilder AddApiKeyInAuthorizationHeader<TApiKeyProvider>(this AuthenticationBuilder builder, string authenticationScheme) where TApiKeyProvider : class, IApiKeyProvider
			=> builder.AddApiKeyInAuthorizationHeader<TApiKeyProvider>(authenticationScheme, configureOptions: null);
#endif

		/// <summary>
		/// Adds API Key - In Authorization Header authentication scheme to the project. It takes a implementation of <see cref="IApiKeyProvider"/> as type parameter.
		/// If <see cref="ApiKeyEvents.OnValidateKey"/> delegate is set on the Events property on <paramref name="configureOptions"/> then it will be used instead of implementation of <see cref="IApiKeyProvider"/>.
		/// </summary>
		/// <typeparam name="TApiKeyProvider"></typeparam>
		/// <param name="builder"></param>
		/// <param name="configureOptions">The <see cref="ApiKeyOptions"/>.</param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
#if NET5_0_OR_GREATER
		public static AuthenticationBuilder AddApiKeyInAuthorizationHeader<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApiKeyProvider>(this AuthenticationBuilder builder, Action<ApiKeyOptions>? configureOptions) where TApiKeyProvider : class, IApiKeyProvider
			=> builder.AddApiKeyInAuthorizationHeader<TApiKeyProvider>(ApiKeyDefaults.AuthenticationScheme, configureOptions);
#else
		public static AuthenticationBuilder AddApiKeyInAuthorizationHeader<TApiKeyProvider>(this AuthenticationBuilder builder, Action<ApiKeyOptions>? configureOptions) where TApiKeyProvider : class, IApiKeyProvider
			=> builder.AddApiKeyInAuthorizationHeader<TApiKeyProvider>(ApiKeyDefaults.AuthenticationScheme, configureOptions);
#endif

		/// <summary>
		/// Adds API Key - In Authorization Header authentication scheme to the project. It takes a implementation of <see cref="IApiKeyProvider"/> as type parameter.
		/// If <see cref="ApiKeyEvents.OnValidateKey"/> delegate is set on the Events property on <paramref name="configureOptions"/> then it will be used instead of implementation of <see cref="IApiKeyProvider"/>.
		/// </summary>
		/// <typeparam name="TApiKeyProvider"></typeparam>
		/// <param name="builder"></param>
		/// <param name="authenticationScheme">The authentication scheme.</param>
		/// <param name="configureOptions">The <see cref="ApiKeyOptions"/>.</param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
#if NET5_0_OR_GREATER
		public static AuthenticationBuilder AddApiKeyInAuthorizationHeader<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApiKeyProvider>(this AuthenticationBuilder builder, string authenticationScheme, Action<ApiKeyOptions>? configureOptions) where TApiKeyProvider : class, IApiKeyProvider
			=> builder.AddApiKeyInAuthorizationHeader<TApiKeyProvider>(authenticationScheme, displayName: null, configureOptions: configureOptions);
#else
		public static AuthenticationBuilder AddApiKeyInAuthorizationHeader<TApiKeyProvider>(this AuthenticationBuilder builder, string authenticationScheme, Action<ApiKeyOptions>? configureOptions) where TApiKeyProvider : class, IApiKeyProvider
			=> builder.AddApiKeyInAuthorizationHeader<TApiKeyProvider>(authenticationScheme, displayName: null, configureOptions: configureOptions);
#endif

		/// <summary>
		/// Adds API Key - In Authorization Header authentication scheme to the project. It takes a implementation of <see cref="IApiKeyProvider"/> as type parameter.
		/// If <see cref="ApiKeyEvents.OnValidateKey"/> delegate is set on the Events property on <paramref name="configureOptions"/> then it will be used instead of implementation of <see cref="IApiKeyProvider"/>.
		/// </summary>
		/// <typeparam name="TApiKeyProvider"></typeparam>
		/// <param name="builder"></param>
		/// <param name="authenticationScheme">The authentication scheme.</param>
		/// <param name="displayName">The display name.</param>
		/// <param name="configureOptions">The <see cref="ApiKeyOptions"/>.</param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
#if NET5_0_OR_GREATER
		public static AuthenticationBuilder AddApiKeyInAuthorizationHeader<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApiKeyProvider>(this AuthenticationBuilder builder, string authenticationScheme, string? displayName, Action<ApiKeyOptions>? configureOptions) where TApiKeyProvider : class, IApiKeyProvider
			=> builder.AddApiKey<TApiKeyProvider, ApiKeyInAuthorizationHeaderHandler>(authenticationScheme, displayName, configureOptions);
#else
		public static AuthenticationBuilder AddApiKeyInAuthorizationHeader<TApiKeyProvider>(this AuthenticationBuilder builder, string authenticationScheme, string? displayName, Action<ApiKeyOptions>? configureOptions) where TApiKeyProvider : class, IApiKeyProvider
			=> builder.AddApiKey<TApiKeyProvider, ApiKeyInAuthorizationHeaderHandler>(authenticationScheme, displayName, configureOptions);
#endif

		#endregion // API Key - In Authorization Header

		#region API Key - In Query Parameters

		/// <summary>
		/// Adds API Key - In Query Parameters authentication scheme to the project.
		/// <see cref="ApiKeyEvents.OnValidateKey"/> delegate must be set on the <see cref="ApiKeyOptions.Events"/>.
		/// </summary>
		/// <param name="builder"></param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
		public static AuthenticationBuilder AddApiKeyInQueryParams(this AuthenticationBuilder builder)
			=> builder.AddApiKeyInQueryParams(ApiKeyDefaults.AuthenticationScheme);

		/// <summary>
		/// Adds API Key - In Query Parameters authentication scheme to the project.
		/// <see cref="ApiKeyEvents.OnValidateKey"/> delegate must be set on the <see cref="ApiKeyOptions.Events"/>.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="authenticationScheme">The authentication scheme.</param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
		public static AuthenticationBuilder AddApiKeyInQueryParams(this AuthenticationBuilder builder, string authenticationScheme)
			=> builder.AddApiKeyInQueryParams(authenticationScheme, configureOptions: null);

		/// <summary>
		/// Adds API Key - In Query Parameters authentication scheme to the project.
		/// <see cref="ApiKeyEvents.OnValidateKey"/> delegate must be set on the Events property on <paramref name="configureOptions"/>.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="configureOptions">The configure options.</param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
		public static AuthenticationBuilder AddApiKeyInQueryParams(this AuthenticationBuilder builder, Action<ApiKeyOptions>? configureOptions)
			=> builder.AddApiKeyInQueryParams(ApiKeyDefaults.AuthenticationScheme, configureOptions);

		/// <summary>
		/// Adds API Key - In Query Parameters authentication scheme to the project.
		/// <see cref="ApiKeyEvents.OnValidateKey"/> delegate must be set on the Events property on <paramref name="configureOptions"/>.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="authenticationScheme">The authentication scheme.</param>
		/// <param name="configureOptions">The configure options.</param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
		public static AuthenticationBuilder AddApiKeyInQueryParams(this AuthenticationBuilder builder, string authenticationScheme, Action<ApiKeyOptions>? configureOptions)
			=> builder.AddApiKeyInQueryParams(authenticationScheme, displayName: null, configureOptions: configureOptions);

		/// <summary>
		/// Adds API Key - In Query Parameters authentication scheme to the project.
		/// <see cref="ApiKeyEvents.OnValidateKey"/> delegate must be set on the Events property on <paramref name="configureOptions"/>.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="authenticationScheme">The authentication scheme.</param>
		/// <param name="displayName">The display name.</param>
		/// <param name="configureOptions">The configure options.</param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
		public static AuthenticationBuilder AddApiKeyInQueryParams(this AuthenticationBuilder builder, string authenticationScheme, string? displayName, Action<ApiKeyOptions>? configureOptions)
			=> builder.AddApiKey<ApiKeyInQueryParamsHandler>(authenticationScheme, displayName, configureOptions);





		/// <summary>
		/// Adds API Key - In Query Parameters authentication scheme to the project. It takes a implementation of <see cref="IApiKeyProvider"/> as type parameter.
		/// If <see cref="ApiKeyEvents.OnValidateKey"/> delegate is set on the <see cref="ApiKeyOptions.Events"/> then it will be used instead of implementation of <see cref="IApiKeyProvider"/>.
		/// </summary>
		/// <typeparam name="TApiKeyProvider"></typeparam>
		/// <param name="builder"></param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
#if NET5_0_OR_GREATER
		public static AuthenticationBuilder AddApiKeyInQueryParams<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApiKeyProvider>(this AuthenticationBuilder builder) where TApiKeyProvider : class, IApiKeyProvider
			=> builder.AddApiKeyInQueryParams<TApiKeyProvider>(ApiKeyDefaults.AuthenticationScheme);
#else
		public static AuthenticationBuilder AddApiKeyInQueryParams<TApiKeyProvider>(this AuthenticationBuilder builder) where TApiKeyProvider : class, IApiKeyProvider
			=> builder.AddApiKeyInQueryParams<TApiKeyProvider>(ApiKeyDefaults.AuthenticationScheme);
#endif

		/// <summary>
		/// Adds API Key - In Query Parameters authentication scheme to the project. It takes a implementation of <see cref="IApiKeyProvider"/> as type parameter.
		/// If <see cref="ApiKeyEvents.OnValidateKey"/> delegate is set on the <see cref="ApiKeyOptions.Events"/> then it will be used instead of implementation of <see cref="IApiKeyProvider"/>.
		/// </summary>
		/// <typeparam name="TApiKeyProvider"></typeparam>
		/// <param name="builder"></param>
		/// <param name="authenticationScheme">The authentication scheme.</param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
#if NET5_0_OR_GREATER
		public static AuthenticationBuilder AddApiKeyInQueryParams<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApiKeyProvider>(this AuthenticationBuilder builder, string authenticationScheme) where TApiKeyProvider : class, IApiKeyProvider
			=> builder.AddApiKeyInQueryParams<TApiKeyProvider>(authenticationScheme, configureOptions: null);
#else
		public static AuthenticationBuilder AddApiKeyInQueryParams<TApiKeyProvider>(this AuthenticationBuilder builder, string authenticationScheme) where TApiKeyProvider : class, IApiKeyProvider
			=> builder.AddApiKeyInQueryParams<TApiKeyProvider>(authenticationScheme, configureOptions: null);
#endif

		/// <summary>
		/// Adds API Key - In Query Parameters authentication scheme to the project. It takes a implementation of <see cref="IApiKeyProvider"/> as type parameter.
		/// If <see cref="ApiKeyEvents.OnValidateKey"/> delegate is set on the Events property on <paramref name="configureOptions"/> then it will be used instead of implementation of <see cref="IApiKeyProvider"/>.
		/// </summary>
		/// <typeparam name="TApiKeyProvider"></typeparam>
		/// <param name="builder"></param>
		/// <param name="configureOptions">The <see cref="ApiKeyOptions"/>.</param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
#if NET5_0_OR_GREATER
		public static AuthenticationBuilder AddApiKeyInQueryParams<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApiKeyProvider>(this AuthenticationBuilder builder, Action<ApiKeyOptions>? configureOptions) where TApiKeyProvider : class, IApiKeyProvider
			=> builder.AddApiKeyInQueryParams<TApiKeyProvider>(ApiKeyDefaults.AuthenticationScheme, configureOptions);
#else
		public static AuthenticationBuilder AddApiKeyInQueryParams<TApiKeyProvider>(this AuthenticationBuilder builder, Action<ApiKeyOptions>? configureOptions) where TApiKeyProvider : class, IApiKeyProvider
			=> builder.AddApiKeyInQueryParams<TApiKeyProvider>(ApiKeyDefaults.AuthenticationScheme, configureOptions);
#endif

		/// <summary>
		/// Adds API Key - In Query Parameters authentication scheme to the project. It takes a implementation of <see cref="IApiKeyProvider"/> as type parameter.
		/// If <see cref="ApiKeyEvents.OnValidateKey"/> delegate is set on the Events property on <paramref name="configureOptions"/> then it will be used instead of implementation of <see cref="IApiKeyProvider"/>.
		/// </summary>
		/// <typeparam name="TApiKeyProvider"></typeparam>
		/// <param name="builder"></param>
		/// <param name="authenticationScheme">The authentication scheme.</param>
		/// <param name="configureOptions">The <see cref="ApiKeyOptions"/>.</param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
#if NET5_0_OR_GREATER
		public static AuthenticationBuilder AddApiKeyInQueryParams<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApiKeyProvider>(this AuthenticationBuilder builder, string authenticationScheme, Action<ApiKeyOptions>? configureOptions) where TApiKeyProvider : class, IApiKeyProvider
			=> builder.AddApiKeyInQueryParams<TApiKeyProvider>(authenticationScheme, displayName: null, configureOptions: configureOptions);
#else
		public static AuthenticationBuilder AddApiKeyInQueryParams<TApiKeyProvider>(this AuthenticationBuilder builder, string authenticationScheme, Action<ApiKeyOptions>? configureOptions) where TApiKeyProvider : class, IApiKeyProvider
			=> builder.AddApiKeyInQueryParams<TApiKeyProvider>(authenticationScheme, displayName: null, configureOptions: configureOptions);
#endif

		/// <summary>
		/// Adds API Key - In Query Parameters authentication scheme to the project. It takes a implementation of <see cref="IApiKeyProvider"/> as type parameter.
		/// If <see cref="ApiKeyEvents.OnValidateKey"/> delegate is set on the Events property on <paramref name="configureOptions"/> then it will be used instead of implementation of <see cref="IApiKeyProvider"/>.
		/// </summary>
		/// <typeparam name="TApiKeyProvider"></typeparam>
		/// <param name="builder"></param>
		/// <param name="authenticationScheme">The authentication scheme.</param>
		/// <param name="displayName">The display name.</param>
		/// <param name="configureOptions">The <see cref="ApiKeyOptions"/>.</param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
#if NET5_0_OR_GREATER
		public static AuthenticationBuilder AddApiKeyInQueryParams<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApiKeyProvider>(this AuthenticationBuilder builder, string authenticationScheme, string? displayName, Action<ApiKeyOptions>? configureOptions) where TApiKeyProvider : class, IApiKeyProvider
			=> builder.AddApiKey<TApiKeyProvider, ApiKeyInQueryParamsHandler>(authenticationScheme, displayName, configureOptions);
#else
		public static AuthenticationBuilder AddApiKeyInQueryParams<TApiKeyProvider>(this AuthenticationBuilder builder, string authenticationScheme, string? displayName, Action<ApiKeyOptions>? configureOptions) where TApiKeyProvider : class, IApiKeyProvider
			=> builder.AddApiKey<TApiKeyProvider, ApiKeyInQueryParamsHandler>(authenticationScheme, displayName, configureOptions);
#endif

		#endregion // API Key - In Query Parameters

		#region API Key - In Header Or Query Parameters

		/// <summary>
		/// Adds API Key - In Header Or Query Parameters authentication scheme to the project.
		/// <see cref="ApiKeyEvents.OnValidateKey"/> delegate must be set on the <see cref="ApiKeyOptions.Events"/>.
		/// </summary>
		/// <param name="builder"></param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
		public static AuthenticationBuilder AddApiKeyInHeaderOrQueryParams(this AuthenticationBuilder builder)
			=> builder.AddApiKeyInHeaderOrQueryParams(ApiKeyDefaults.AuthenticationScheme);

		/// <summary>
		/// Adds API Key - In Header Or Query Parameters authentication scheme to the project.
		/// <see cref="ApiKeyEvents.OnValidateKey"/> delegate must be set on the <see cref="ApiKeyOptions.Events"/>.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="authenticationScheme">The authentication scheme.</param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
		public static AuthenticationBuilder AddApiKeyInHeaderOrQueryParams(this AuthenticationBuilder builder, string authenticationScheme)
			=> builder.AddApiKeyInHeaderOrQueryParams(authenticationScheme, configureOptions: null);

		/// <summary>
		/// Adds API Key - In Header Or Query Parameters authentication scheme to the project.
		/// <see cref="ApiKeyEvents.OnValidateKey"/> delegate must be set on the Events property on <paramref name="configureOptions"/>.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="configureOptions">The configure options.</param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
		public static AuthenticationBuilder AddApiKeyInHeaderOrQueryParams(this AuthenticationBuilder builder, Action<ApiKeyOptions>? configureOptions)
			=> builder.AddApiKeyInHeaderOrQueryParams(ApiKeyDefaults.AuthenticationScheme, configureOptions);

		/// <summary>
		/// Adds API Key - In Header Or Query Parameters authentication scheme to the project.
		/// <see cref="ApiKeyEvents.OnValidateKey"/> delegate must be set on the Events property on <paramref name="configureOptions"/>.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="authenticationScheme">The authentication scheme.</param>
		/// <param name="configureOptions">The configure options.</param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
		public static AuthenticationBuilder AddApiKeyInHeaderOrQueryParams(this AuthenticationBuilder builder, string authenticationScheme, Action<ApiKeyOptions>? configureOptions)
			=> builder.AddApiKeyInHeaderOrQueryParams(authenticationScheme, displayName: null, configureOptions: configureOptions);

		/// <summary>
		/// Adds API Key - In Header Or Query Parameters authentication scheme to the project.
		/// <see cref="ApiKeyEvents.OnValidateKey"/> delegate must be set on the Events property on <paramref name="configureOptions"/>.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="authenticationScheme">The authentication scheme.</param>
		/// <param name="displayName">The display name.</param>
		/// <param name="configureOptions">The configure options.</param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
		public static AuthenticationBuilder AddApiKeyInHeaderOrQueryParams(this AuthenticationBuilder builder, string authenticationScheme, string? displayName, Action<ApiKeyOptions>? configureOptions)
			=> builder.AddApiKey<ApiKeyInHeaderOrQueryParamsHandler>(authenticationScheme, displayName, configureOptions);





		/// <summary>
		/// Adds API Key - In Header Or Query Parameters authentication scheme to the project. It takes a implementation of <see cref="IApiKeyProvider"/> as type parameter.
		/// If <see cref="ApiKeyEvents.OnValidateKey"/> delegate is set on the <see cref="ApiKeyOptions.Events"/> then it will be used instead of implementation of <see cref="IApiKeyProvider"/>.
		/// </summary>
		/// <typeparam name="TApiKeyProvider"></typeparam>
		/// <param name="builder"></param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
#if NET5_0_OR_GREATER
		public static AuthenticationBuilder AddApiKeyInHeaderOrQueryParams<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApiKeyProvider>(this AuthenticationBuilder builder) where TApiKeyProvider : class, IApiKeyProvider
			=> builder.AddApiKeyInHeaderOrQueryParams<TApiKeyProvider>(ApiKeyDefaults.AuthenticationScheme);
#else
		public static AuthenticationBuilder AddApiKeyInHeaderOrQueryParams<TApiKeyProvider>(this AuthenticationBuilder builder) where TApiKeyProvider : class, IApiKeyProvider
			=> builder.AddApiKeyInHeaderOrQueryParams<TApiKeyProvider>(ApiKeyDefaults.AuthenticationScheme);
#endif

		/// <summary>
		/// Adds API Key - In Header Or Query Parameters authentication scheme to the project. It takes a implementation of <see cref="IApiKeyProvider"/> as type parameter.
		/// If <see cref="ApiKeyEvents.OnValidateKey"/> delegate is set on the <see cref="ApiKeyOptions.Events"/> then it will be used instead of implementation of <see cref="IApiKeyProvider"/>.
		/// </summary>
		/// <typeparam name="TApiKeyProvider"></typeparam>
		/// <param name="builder"></param>
		/// <param name="authenticationScheme">The authentication scheme.</param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
#if NET5_0_OR_GREATER
		public static AuthenticationBuilder AddApiKeyInHeaderOrQueryParams<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApiKeyProvider>(this AuthenticationBuilder builder, string authenticationScheme) where TApiKeyProvider : class, IApiKeyProvider
			=> builder.AddApiKeyInHeaderOrQueryParams<TApiKeyProvider>(authenticationScheme, configureOptions: null);
#else
		public static AuthenticationBuilder AddApiKeyInHeaderOrQueryParams<TApiKeyProvider>(this AuthenticationBuilder builder, string authenticationScheme) where TApiKeyProvider : class, IApiKeyProvider
			=> builder.AddApiKeyInHeaderOrQueryParams<TApiKeyProvider>(authenticationScheme, configureOptions: null);
#endif

		/// <summary>
		/// Adds API Key - In Header Or Query Parameters authentication scheme to the project. It takes a implementation of <see cref="IApiKeyProvider"/> as type parameter.
		/// If <see cref="ApiKeyEvents.OnValidateKey"/> delegate is set on the Events property on <paramref name="configureOptions"/> then it will be used instead of implementation of <see cref="IApiKeyProvider"/>.
		/// </summary>
		/// <typeparam name="TApiKeyProvider"></typeparam>
		/// <param name="builder"></param>
		/// <param name="configureOptions">The <see cref="ApiKeyOptions"/>.</param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
#if NET5_0_OR_GREATER
		public static AuthenticationBuilder AddApiKeyInHeaderOrQueryParams<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApiKeyProvider>(this AuthenticationBuilder builder, Action<ApiKeyOptions>? configureOptions) where TApiKeyProvider : class, IApiKeyProvider
			=> builder.AddApiKeyInHeaderOrQueryParams<TApiKeyProvider>(ApiKeyDefaults.AuthenticationScheme, configureOptions);
#else
		public static AuthenticationBuilder AddApiKeyInHeaderOrQueryParams<TApiKeyProvider>(this AuthenticationBuilder builder, Action<ApiKeyOptions>? configureOptions) where TApiKeyProvider : class, IApiKeyProvider
			=> builder.AddApiKeyInHeaderOrQueryParams<TApiKeyProvider>(ApiKeyDefaults.AuthenticationScheme, configureOptions);
#endif

		/// <summary>
		/// Adds API Key - In Header Or Query Parameters authentication scheme to the project. It takes a implementation of <see cref="IApiKeyProvider"/> as type parameter.
		/// If <see cref="ApiKeyEvents.OnValidateKey"/> delegate is set on the Events property on <paramref name="configureOptions"/> then it will be used instead of implementation of <see cref="IApiKeyProvider"/>.
		/// </summary>
		/// <typeparam name="TApiKeyProvider"></typeparam>
		/// <param name="builder"></param>
		/// <param name="authenticationScheme">The authentication scheme.</param>
		/// <param name="configureOptions">The <see cref="ApiKeyOptions"/>.</param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
#if NET5_0_OR_GREATER
		public static AuthenticationBuilder AddApiKeyInHeaderOrQueryParams<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApiKeyProvider>(this AuthenticationBuilder builder, string authenticationScheme, Action<ApiKeyOptions>? configureOptions) where TApiKeyProvider : class, IApiKeyProvider
			=> builder.AddApiKeyInHeaderOrQueryParams<TApiKeyProvider>(authenticationScheme, displayName: null, configureOptions: configureOptions);
#else
		public static AuthenticationBuilder AddApiKeyInHeaderOrQueryParams<TApiKeyProvider>(this AuthenticationBuilder builder, string authenticationScheme, Action<ApiKeyOptions>? configureOptions) where TApiKeyProvider : class, IApiKeyProvider
			=> builder.AddApiKeyInHeaderOrQueryParams<TApiKeyProvider>(authenticationScheme, displayName: null, configureOptions: configureOptions);
#endif

		/// <summary>
		/// Adds API Key - In Header Or Query Parameters authentication scheme to the project. It takes a implementation of <see cref="IApiKeyProvider"/> as type parameter.
		/// If <see cref="ApiKeyEvents.OnValidateKey"/> delegate is set on the Events property on <paramref name="configureOptions"/> then it will be used instead of implementation of <see cref="IApiKeyProvider"/>.
		/// </summary>
		/// <typeparam name="TApiKeyProvider"></typeparam>
		/// <param name="builder"></param>
		/// <param name="authenticationScheme">The authentication scheme.</param>
		/// <param name="displayName">The display name.</param>
		/// <param name="configureOptions">The <see cref="ApiKeyOptions"/>.</param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
#if NET5_0_OR_GREATER
		public static AuthenticationBuilder AddApiKeyInHeaderOrQueryParams<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApiKeyProvider>(this AuthenticationBuilder builder, string authenticationScheme, string? displayName, Action<ApiKeyOptions>? configureOptions) where TApiKeyProvider : class, IApiKeyProvider
			=> builder.AddApiKey<TApiKeyProvider, ApiKeyInHeaderOrQueryParamsHandler>(authenticationScheme, displayName, configureOptions);
#else
		public static AuthenticationBuilder AddApiKeyInHeaderOrQueryParams<TApiKeyProvider>(this AuthenticationBuilder builder, string authenticationScheme, string? displayName, Action<ApiKeyOptions>? configureOptions) where TApiKeyProvider : class, IApiKeyProvider
			=> builder.AddApiKey<TApiKeyProvider, ApiKeyInHeaderOrQueryParamsHandler>(authenticationScheme, displayName, configureOptions);
#endif

		#endregion // API Key - In Header Or Query Parameters

		#region API Key - In Route Values
#if NETCOREAPP3_0_OR_GREATER
		/// <summary>
		/// Adds API Key - In Route Values authentication scheme to the project.
		/// <see cref="ApiKeyEvents.OnValidateKey"/> delegate must be set on the <see cref="ApiKeyOptions.Events"/>.
		/// </summary>
		/// <param name="builder"></param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
		public static AuthenticationBuilder AddApiKeyInRouteValues(this AuthenticationBuilder builder)
			=> builder.AddApiKeyInRouteValues(ApiKeyDefaults.AuthenticationScheme);

		/// <summary>
		/// Adds API Key - In Route Values authentication scheme to the project.
		/// <see cref="ApiKeyEvents.OnValidateKey"/> delegate must be set on the <see cref="ApiKeyOptions.Events"/>.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="authenticationScheme">The authentication scheme.</param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
		public static AuthenticationBuilder AddApiKeyInRouteValues(this AuthenticationBuilder builder, string authenticationScheme)
			=> builder.AddApiKeyInRouteValues(authenticationScheme, configureOptions: null);

		/// <summary>
		/// Adds API Key - In Route Values authentication scheme to the project.
		/// <see cref="ApiKeyEvents.OnValidateKey"/> delegate must be set on the Events property on <paramref name="configureOptions"/>.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="configureOptions">The configure options.</param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
		public static AuthenticationBuilder AddApiKeyInRouteValues(this AuthenticationBuilder builder, Action<ApiKeyOptions>? configureOptions)
			=> builder.AddApiKeyInRouteValues(ApiKeyDefaults.AuthenticationScheme, configureOptions);

		/// <summary>
		/// Adds API Key - In Route Values authentication scheme to the project.
		/// <see cref="ApiKeyEvents.OnValidateKey"/> delegate must be set on the Events property on <paramref name="configureOptions"/>.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="authenticationScheme">The authentication scheme.</param>
		/// <param name="configureOptions">The configure options.</param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
		public static AuthenticationBuilder AddApiKeyInRouteValues(this AuthenticationBuilder builder, string authenticationScheme, Action<ApiKeyOptions>? configureOptions)
			=> builder.AddApiKeyInRouteValues(authenticationScheme, displayName: null, configureOptions: configureOptions);

		/// <summary>
		/// Adds API Key - In Route Values authentication scheme to the project.
		/// <see cref="ApiKeyEvents.OnValidateKey"/> delegate must be set on the Events property on <paramref name="configureOptions"/>.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="authenticationScheme">The authentication scheme.</param>
		/// <param name="displayName">The display name.</param>
		/// <param name="configureOptions">The configure options.</param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
		public static AuthenticationBuilder AddApiKeyInRouteValues(this AuthenticationBuilder builder, string authenticationScheme, string? displayName, Action<ApiKeyOptions>? configureOptions)
			=> builder.AddApiKey<ApiKeyInRouteValuesHandler>(authenticationScheme, displayName, configureOptions);





		/// <summary>
		/// Adds API Key - In Route Values authentication scheme to the project. It takes a implementation of <see cref="IApiKeyProvider"/> as type parameter.
		/// If <see cref="ApiKeyEvents.OnValidateKey"/> delegate is set on the <see cref="ApiKeyOptions.Events"/> then it will be used instead of implementation of <see cref="IApiKeyProvider"/>.
		/// </summary>
		/// <typeparam name="TApiKeyProvider"></typeparam>
		/// <param name="builder"></param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
#if NET5_0_OR_GREATER
		public static AuthenticationBuilder AddApiKeyInRouteValues<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApiKeyProvider>(this AuthenticationBuilder builder) where TApiKeyProvider : class, IApiKeyProvider
			=> builder.AddApiKeyInRouteValues<TApiKeyProvider>(ApiKeyDefaults.AuthenticationScheme);
#else
		public static AuthenticationBuilder AddApiKeyInRouteValues<TApiKeyProvider>(this AuthenticationBuilder builder) where TApiKeyProvider : class, IApiKeyProvider
			=> builder.AddApiKeyInRouteValues<TApiKeyProvider>(ApiKeyDefaults.AuthenticationScheme);
#endif

		/// <summary>
		/// Adds API Key - In Route Values authentication scheme to the project. It takes a implementation of <see cref="IApiKeyProvider"/> as type parameter.
		/// If <see cref="ApiKeyEvents.OnValidateKey"/> delegate is set on the <see cref="ApiKeyOptions.Events"/> then it will be used instead of implementation of <see cref="IApiKeyProvider"/>.
		/// </summary>
		/// <typeparam name="TApiKeyProvider"></typeparam>
		/// <param name="builder"></param>
		/// <param name="authenticationScheme">The authentication scheme.</param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
#if NET5_0_OR_GREATER
		public static AuthenticationBuilder AddApiKeyInRouteValues<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApiKeyProvider>(this AuthenticationBuilder builder, string authenticationScheme) where TApiKeyProvider : class, IApiKeyProvider
			=> builder.AddApiKeyInRouteValues<TApiKeyProvider>(authenticationScheme, configureOptions: null);
#else
		public static AuthenticationBuilder AddApiKeyInRouteValues<TApiKeyProvider>(this AuthenticationBuilder builder, string authenticationScheme) where TApiKeyProvider : class, IApiKeyProvider
			=> builder.AddApiKeyInRouteValues<TApiKeyProvider>(authenticationScheme, configureOptions: null);
#endif

		/// <summary>
		/// Adds API Key - In Route Values authentication scheme to the project. It takes a implementation of <see cref="IApiKeyProvider"/> as type parameter.
		/// If <see cref="ApiKeyEvents.OnValidateKey"/> delegate is set on the Events property on <paramref name="configureOptions"/> then it will be used instead of implementation of <see cref="IApiKeyProvider"/>.
		/// </summary>
		/// <typeparam name="TApiKeyProvider"></typeparam>
		/// <param name="builder"></param>
		/// <param name="configureOptions">The <see cref="ApiKeyOptions"/>.</param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
#if NET5_0_OR_GREATER
		public static AuthenticationBuilder AddApiKeyInRouteValues<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApiKeyProvider>(this AuthenticationBuilder builder, Action<ApiKeyOptions>? configureOptions) where TApiKeyProvider : class, IApiKeyProvider
			=> builder.AddApiKeyInRouteValues<TApiKeyProvider>(ApiKeyDefaults.AuthenticationScheme, configureOptions);
#else
		public static AuthenticationBuilder AddApiKeyInRouteValues<TApiKeyProvider>(this AuthenticationBuilder builder, Action<ApiKeyOptions>? configureOptions) where TApiKeyProvider : class, IApiKeyProvider
			=> builder.AddApiKeyInRouteValues<TApiKeyProvider>(ApiKeyDefaults.AuthenticationScheme, configureOptions);
#endif

		/// <summary>
		/// Adds API Key - In Route Values authentication scheme to the project. It takes a implementation of <see cref="IApiKeyProvider"/> as type parameter.
		/// If <see cref="ApiKeyEvents.OnValidateKey"/> delegate is set on the Events property on <paramref name="configureOptions"/> then it will be used instead of implementation of <see cref="IApiKeyProvider"/>.
		/// </summary>
		/// <typeparam name="TApiKeyProvider"></typeparam>
		/// <param name="builder"></param>
		/// <param name="authenticationScheme">The authentication scheme.</param>
		/// <param name="configureOptions">The <see cref="ApiKeyOptions"/>.</param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
#if NET5_0_OR_GREATER
		public static AuthenticationBuilder AddApiKeyInRouteValues<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApiKeyProvider>(this AuthenticationBuilder builder, string authenticationScheme, Action<ApiKeyOptions>? configureOptions) where TApiKeyProvider : class, IApiKeyProvider
			=> builder.AddApiKeyInRouteValues<TApiKeyProvider>(authenticationScheme, displayName: null, configureOptions: configureOptions);
#else
		public static AuthenticationBuilder AddApiKeyInRouteValues<TApiKeyProvider>(this AuthenticationBuilder builder, string authenticationScheme, Action<ApiKeyOptions>? configureOptions) where TApiKeyProvider : class, IApiKeyProvider
			=> builder.AddApiKeyInRouteValues<TApiKeyProvider>(authenticationScheme, displayName: null, configureOptions: configureOptions);
#endif

		/// <summary>
		/// Adds API Key - In Route Values authentication scheme to the project. It takes a implementation of <see cref="IApiKeyProvider"/> as type parameter.
		/// If <see cref="ApiKeyEvents.OnValidateKey"/> delegate is set on the Events property on <paramref name="configureOptions"/> then it will be used instead of implementation of <see cref="IApiKeyProvider"/>.
		/// </summary>
		/// <typeparam name="TApiKeyProvider"></typeparam>
		/// <param name="builder"></param>
		/// <param name="authenticationScheme">The authentication scheme.</param>
		/// <param name="displayName">The display name.</param>
		/// <param name="configureOptions">The <see cref="ApiKeyOptions"/>.</param>
		/// <returns>The instance of <see cref="AuthenticationBuilder"/></returns>
#if NET5_0_OR_GREATER
		public static AuthenticationBuilder AddApiKeyInRouteValues<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApiKeyProvider>(this AuthenticationBuilder builder, string authenticationScheme, string? displayName, Action<ApiKeyOptions>? configureOptions) where TApiKeyProvider : class, IApiKeyProvider
			=> builder.AddApiKey<TApiKeyProvider, ApiKeyInRouteValuesHandler>(authenticationScheme, displayName, configureOptions);
#else
		public static AuthenticationBuilder AddApiKeyInRouteValues<TApiKeyProvider>(this AuthenticationBuilder builder, string authenticationScheme, string? displayName, Action<ApiKeyOptions>? configureOptions) where TApiKeyProvider : class, IApiKeyProvider
			=> builder.AddApiKey<TApiKeyProvider, ApiKeyInRouteValuesHandler>(authenticationScheme, displayName, configureOptions);
#endif

#endif
		#endregion // API Key - In Route Values


#if NET5_0_OR_GREATER
		private static AuthenticationBuilder AddApiKey<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApiKeyHandler>(this AuthenticationBuilder builder, string authenticationScheme, string? displayName, Action<ApiKeyOptions>? configureOptions)
			where TApiKeyHandler : AuthenticationHandler<ApiKeyOptions>
#else
		private static AuthenticationBuilder AddApiKey<TApiKeyHandler>(this AuthenticationBuilder builder, string authenticationScheme, string? displayName, Action<ApiKeyOptions>? configureOptions)
			where TApiKeyHandler : AuthenticationHandler<ApiKeyOptions>
#endif
		{
			// Adds post configure options to the pipeline.
			builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<ApiKeyOptions>, ApiKeyPostConfigureOptions>());

			// Adds api key authentication scheme to the pipeline.
			return builder.AddScheme<ApiKeyOptions, TApiKeyHandler>(authenticationScheme, displayName, configureOptions);
		}

#if NET5_0_OR_GREATER
		private static AuthenticationBuilder AddApiKey<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApiKeyProvider, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApiKeyHandler>(this AuthenticationBuilder builder, string authenticationScheme, string? displayName, Action<ApiKeyOptions>? configureOptions)
			where TApiKeyProvider : class, IApiKeyProvider
			where TApiKeyHandler : AuthenticationHandler<ApiKeyOptions>
#else
		private static AuthenticationBuilder AddApiKey<TApiKeyProvider, TApiKeyHandler>(this AuthenticationBuilder builder, string authenticationScheme, string? displayName, Action<ApiKeyOptions>? configureOptions)
			where TApiKeyProvider : class, IApiKeyProvider
			where TApiKeyHandler : AuthenticationHandler<ApiKeyOptions>
#endif
		{
			// Adds implementation of IApiKeyProvider to the dependency container.
			builder.Services.AddTransient<IApiKeyProvider, TApiKeyProvider>();
			builder.Services.Configure<ApiKeyOptions>(
				authenticationScheme,
				o => o.ApiKeyProviderType = typeof(TApiKeyProvider)
			);

			// Adds post configure options to the pipeline.
			builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<ApiKeyOptions>, ApiKeyPostConfigureOptions>());

			// Adds api key authentication scheme to the pipeline.
			return builder.AddScheme<ApiKeyOptions, TApiKeyHandler>(authenticationScheme, displayName, configureOptions);
		}
	}
}
