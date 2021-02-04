using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace AspNetCore.Authentication.ApiKey.Tests
{
    public class ApiKeyPostConfigureOptionsTests
    {
		[Fact]
		public async Task PostConfigure_no_option_set_throws_exception()
		{
			await Assert.ThrowsAsync<InvalidOperationException>(() => RunTestServerAsync(_ => { }));
		}

		[Fact]
		public async Task PostConfigure_Realm_or_SuppressWWWAuthenticateHeader_not_set_throws_exception()
		{
			var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
				RunTestServerWithApiKeyProviderAsync(options =>
				{
					options.KeyName = "X-API-KEY";
				})
			);

			Assert.Contains($"{nameof(ApiKeyOptions.Realm)} must be set in {typeof(ApiKeyOptions).Name} when setting up the authentication.", exception.Message);
		}

		[Fact]
		public async Task PostConfigure_Realm_not_set_but_SuppressWWWAuthenticateHeader_set_no_exception_thrown()
		{
			await RunTestServerWithApiKeyProviderAsync(options =>
			{
				options.SuppressWWWAuthenticateHeader = true;
				options.KeyName = "X-API-KEY";
			});
		}

		[Fact]
		public async Task PostConfigure_Realm_set_but_SuppressWWWAuthenticateHeader_not_set_no_exception_thrown()
		{
			await RunTestServerWithApiKeyProviderAsync(options =>
			{
				options.Realm = "Test";
				options.KeyName = "X-API-KEY";
			});
		}

		[Fact]
		public async Task PostConfigure_KeyName_not_set_throws_exception()
		{
			var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
				RunTestServerWithApiKeyProviderAsync(options =>
				{
					options.SuppressWWWAuthenticateHeader = true;
				})
			);

			Assert.Contains($"{nameof(ApiKeyOptions.KeyName)} must be set in {typeof(ApiKeyOptions).Name} when setting up the authentication.", exception.Message);
		}

		[Fact]
		public async Task PostConfigure_KeyName_set_no_exception_thrown()
		{
			await RunTestServerWithApiKeyProviderAsync(options =>
			{
				options.SuppressWWWAuthenticateHeader = true;
				options.KeyName = "X-API-KEY";
			});
		}

		[Fact]
		public async Task PostConfigure_Events_OnValidateKey_or_IApiKeyProvider_not_set_throws_exception()
		{
			var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
				RunTestServerAsync(options =>
				{
					options.SuppressWWWAuthenticateHeader = true;
					options.KeyName = "X-API-KEY";
				})
			);

			Assert.Contains($"Either {nameof(ApiKeyOptions.Events.OnValidateKey)} delegate on configure options {nameof(ApiKeyOptions.Events)} should be set or use an extention method with type parameter of type {nameof(IApiKeyProvider)}.", exception.Message);
		}

		[Fact]
		public async Task PostConfigure_Events_OnValidateKey_set_but_IApiKeyProvider_not_set_no_exception_thrown()
		{
			await RunTestServerAsync(options =>
			{
				options.Events.OnValidateKey = _ => Task.CompletedTask;
				options.SuppressWWWAuthenticateHeader = true;
				options.KeyName = "X-API-KEY";
			});
		}

		[Fact]
		public async Task PostConfigure_Events_OnValidateKey_not_set_but_IApiKeyProvider_set_no_exception_thrown()
		{
			await RunTestServerWithApiKeyProviderAsync(options =>
			{
				options.SuppressWWWAuthenticateHeader = true;
				options.KeyName = "X-API-KEY";
			});
		}




		private async Task RunTestServerWithApiKeyProviderAsync(Action<ApiKeyOptions> configureOptions)
		{
			var server = new TestServer(
				new WebHostBuilder()
					.ConfigureServices(services =>
					{
						services.AddAuthentication(ApiKeyDefaults.AuthenticationScheme)
							.AddApiKeyInHeaderOrQueryParams<MockApiKeyProvider>(configureOptions);
					})
					.Configure(app =>
					{
						app.UseAuthentication();
					})
			);
			await server.CreateClient().GetAsync("http://localhost/");
		}

		private async Task RunTestServerAsync(Action<ApiKeyOptions> configureOptions)
		{
			var server = new TestServer(
				new WebHostBuilder()
					.ConfigureServices(services =>
					{
						services.AddAuthentication(ApiKeyDefaults.AuthenticationScheme)
							.AddApiKeyInHeaderOrQueryParams(configureOptions);
					})
					.Configure(app =>
					{
						app.UseAuthentication();
					})
			);
			await server.CreateClient().GetAsync("http://localhost/");
		}

		private class MockApiKeyProvider : IApiKeyProvider
		{
			public Task<IApiKey> ProvideAsync(string key)
			{
				throw new System.NotImplementedException();
			}
		}
	}
}
