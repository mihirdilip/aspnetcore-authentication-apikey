// Copyright (c) Mihir Dilip. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for license information.

using AspNetCore.Authentication.ApiKey.Tests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace AspNetCore.Authentication.ApiKey.Tests
{
    public class ApiKeyPostConfigureOptionsTests
    {
		static string KeyName = "X-API-KEY";

		[Fact]
		public async Task PostConfigure_no_option_set_throws_exception()
		{
			await Assert.ThrowsAsync<InvalidOperationException>(() => RunAuthInitAsync(_ => { }));
		}

		[Fact]
		public async Task PostConfigure_Realm_or_SuppressWWWAuthenticateHeader_not_set_throws_exception()
		{
			var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
				RunAuthInitWithProviderAsync(options =>
				{
					options.KeyName = KeyName;
				})
			);

			Assert.Contains($"{nameof(ApiKeyOptions.Realm)} must be set in {nameof(ApiKeyOptions)} when setting up the authentication.", exception.Message);
		}

		[Fact]
		public async Task PostConfigure_Realm_not_set_but_SuppressWWWAuthenticateHeader_set_no_exception_thrown()
		{
			await RunAuthInitWithProviderAsync(options =>
			{
				options.SuppressWWWAuthenticateHeader = true;
				options.KeyName = KeyName;
			});
		}

		[Fact]
		public async Task PostConfigure_Realm_set_but_SuppressWWWAuthenticateHeader_not_set_no_exception_thrown()
		{
			await RunAuthInitWithProviderAsync(options =>
			{
				options.Realm = "Test";
				options.KeyName = KeyName;
			});
		}

		[Fact]
		public async Task PostConfigure_KeyName_not_set_throws_exception()
		{
			var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
				RunAuthInitWithProviderAsync(options =>
				{
					options.SuppressWWWAuthenticateHeader = true;
				})
			);

			Assert.Contains($"{nameof(ApiKeyOptions.KeyName)} must be set in {nameof(ApiKeyOptions)} when setting up the authentication.", exception.Message);
		}

		[Fact]
		public async Task PostConfigure_KeyName_set_no_exception_thrown()
		{
			await RunAuthInitWithProviderAsync(options =>
			{
				options.SuppressWWWAuthenticateHeader = true;
				options.KeyName = KeyName;
			});
		}

		[Fact]
		public async Task PostConfigure_Events_OnValidateKey_or_IApiKeyProvider_or_IApiKeyProviderFactory_not_set_throws_exception()
		{
			var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
				RunAuthInitAsync(options =>
				{
					options.SuppressWWWAuthenticateHeader = true;
					options.KeyName = KeyName;
				})
			);

			Assert.Contains($"Either {nameof(ApiKeyOptions.Events.OnValidateKey)} delegate on configure options {nameof(ApiKeyOptions.Events)} should be set or use an extension method with type parameter of type {nameof(IApiKeyProvider)} or register an implementation of type {nameof(IApiKeyProviderFactory)} in the service collection.", exception.Message);
		}

		[Fact]
		public async Task PostConfigure_Events_OnValidateKey_set_but_IApiKeyProvider_not_set_no_exception_thrown()
		{
			await RunAuthInitAsync(options =>
			{
				options.Events.OnValidateKey = _ => Task.CompletedTask;
				options.SuppressWWWAuthenticateHeader = true;
				options.KeyName = KeyName;
			});
		}

		[Fact]
		public async Task PostConfigure_Events_OnValidateKey_not_set_but_IApiKeyProvider_set_no_exception_thrown()
		{
			await RunAuthInitWithProviderAsync(options =>
			{
				options.SuppressWWWAuthenticateHeader = true;
				options.KeyName = KeyName;
			});
		}

		[Fact]
		public async Task PostConfigure_Events_OnValidateKey_not_set_and_IApiKeyProvider_not_set_but_IApiKeyProviderFactory_registered_no_exception_thrown()
		{
			await RunAuthInitWithServiceFactoryAsync(options =>
			{
				options.SuppressWWWAuthenticateHeader = true;
				options.KeyName = KeyName;
			});
		}

		private async Task RunAuthInitAsync(Action<ApiKeyOptions> configureOptions)
		{
			var server = TestServerBuilder.BuildInHeaderOrQueryParamsServer(configureOptions);
			await server.CreateClient().GetAsync(TestServerBuilder.BaseUrl);
		}

		private async Task RunAuthInitWithProviderAsync(Action<ApiKeyOptions> configureOptions)
		{
			var server = TestServerBuilder.BuildInHeaderOrQueryParamsServerWithProvider(configureOptions);
			await server.CreateClient().GetAsync(TestServerBuilder.BaseUrl);
		}

		private async Task RunAuthInitWithServiceFactoryAsync(Action<ApiKeyOptions> configureOptions)
		{
			var server = TestServerBuilder.BuildInHeaderOrQueryParamsServerWithProviderFactory(configureOptions);
			await server.CreateClient().GetAsync(TestServerBuilder.BaseUrl);
		}
	}
}
