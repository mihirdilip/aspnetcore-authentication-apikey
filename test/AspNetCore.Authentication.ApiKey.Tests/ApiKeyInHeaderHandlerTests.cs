// Copyright (c) Mihir Dilip. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for license information.

using AspNetCore.Authentication.ApiKey.Tests.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace AspNetCore.Authentication.ApiKey.Tests
{
    public class ApiKeyInHeaderHandlerTests : IDisposable
    {
		private readonly TestServer _server;
        private readonly HttpClient _client;
        private readonly TestServer _serverWithProvider;
        private readonly HttpClient _clientWithProvider;

        public ApiKeyInHeaderHandlerTests()
        {
			_server = TestServerBuilder.BuildInHeaderServer();
			_client = _server.CreateClient();

			_serverWithProvider = TestServerBuilder.BuildInHeaderServerWithProvider();
			_clientWithProvider = _serverWithProvider.CreateClient();
		}

		public void Dispose()
		{
			_client?.Dispose();
			_server?.Dispose();

			_clientWithProvider?.Dispose();
			_serverWithProvider?.Dispose();
		}

		[Fact]
		public async Task Verify_Handler()
		{
			var services = _server.Host.Services;
			var schemeProvider = services.GetRequiredService<IAuthenticationSchemeProvider>();
			Assert.NotNull(schemeProvider);
			
			var scheme = await schemeProvider.GetDefaultAuthenticateSchemeAsync();
			Assert.NotNull(scheme);
			Assert.Equal(typeof(ApiKeyInHeaderHandler), scheme.HandlerType);

            var apiKeyOptions = services.GetService<IOptionsSnapshot<ApiKeyOptions>>();
            Assert.NotNull(apiKeyOptions.Get(scheme.Name)?.Events?.OnValidateKey);

            var apiKeyProvider = services.GetService<IApiKeyProvider>();
			Assert.Null(apiKeyProvider);
		}

		[Fact]
		public async Task TApiKeyProvider_Verify_Handler()
		{
			var services = _serverWithProvider.Host.Services;
			var schemeProvider = services.GetRequiredService<IAuthenticationSchemeProvider>();
			Assert.NotNull(schemeProvider);

			var scheme = await schemeProvider.GetDefaultAuthenticateSchemeAsync();
			Assert.NotNull(scheme);
			Assert.Equal(typeof(ApiKeyInHeaderHandler), scheme.HandlerType);

            var apiKeyOptions = services.GetService<IOptionsSnapshot<ApiKeyOptions>>();
            Assert.Null(apiKeyOptions.Get(scheme.Name)?.Events?.OnValidateKey);

            var apiKeyProvider = services.GetService<IApiKeyProvider>();
			Assert.NotNull(apiKeyProvider);
		}

		[Fact]
        public async Task Unauthorized()
        {
			using var request = new HttpRequestMessage(HttpMethod.Get, TestServerBuilder.BaseUrl);
			using var response = await _client.SendAsync(request);
			Assert.False(response.IsSuccessStatusCode);
			Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

		[Fact]
		public async Task Success()
		{
			using var request = new HttpRequestMessage(HttpMethod.Get, TestServerBuilder.BaseUrl);
			request.Headers.Add(FakeApiKeys.KeyName, FakeApiKeys.FakeKey);
			using var response = await _client.SendAsync(request);
			Assert.True(response.IsSuccessStatusCode);
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		}

		[Fact]
		public async Task Invalid_key_unauthorized()
		{
			using var request = new HttpRequestMessage(HttpMethod.Get, TestServerBuilder.BaseUrl);
			request.Headers.Add(FakeApiKeys.KeyName, FakeApiKeys.FakeInvalidKey);
			using var response = await _client.SendAsync(request);
			Assert.False(response.IsSuccessStatusCode);
			Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
		}


		[Fact]
		public async Task TApiKeyProvider_Unauthorized()
		{
			using var request = new HttpRequestMessage(HttpMethod.Get, TestServerBuilder.BaseUrl);
			using var response = await _clientWithProvider.SendAsync(request);
			Assert.False(response.IsSuccessStatusCode);
			Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public async Task TApiKeyProvider_success()
		{
			using var request = new HttpRequestMessage(HttpMethod.Get, TestServerBuilder.BaseUrl);
			request.Headers.Add(FakeApiKeys.KeyName, FakeApiKeys.FakeKey);
			using var response = await _clientWithProvider.SendAsync(request);
			Assert.True(response.IsSuccessStatusCode);
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		}

		[Fact]
		public async Task TApiKeyProvider_invalid_key_unauthotized()
		{
			using var request = new HttpRequestMessage(HttpMethod.Get, TestServerBuilder.BaseUrl);
			request.Headers.Add(FakeApiKeys.KeyName, FakeApiKeys.FakeInvalidKey);
			using var response = await _clientWithProvider.SendAsync(request);
			Assert.False(response.IsSuccessStatusCode);
			Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
		}
	}
}
