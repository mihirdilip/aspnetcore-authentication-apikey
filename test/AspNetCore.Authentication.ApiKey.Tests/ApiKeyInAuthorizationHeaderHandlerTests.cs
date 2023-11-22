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
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;

namespace AspNetCore.Authentication.ApiKey.Tests
{
    public class ApiKeyInAuthorizationHeaderHandlerTests : IDisposable
    {
		private readonly TestServer _server;
        private readonly HttpClient _client;
        private readonly TestServer _serverWithProvider;
        private readonly HttpClient _clientWithProvider;
        private bool _disposedValue;

        public ApiKeyInAuthorizationHeaderHandlerTests()
        {
			_server = TestServerBuilder.BuildInAuthorizationHeaderServer();
			_client = _server.CreateClient();

			_serverWithProvider = TestServerBuilder.BuildInAuthorizationHeaderServerWithProvider();
			_clientWithProvider = _serverWithProvider.CreateClient();
		}

		[Fact]
		public async Task Verify_Handler()
		{
			var services = _server.Host.Services;
			var schemeProvider = services.GetRequiredService<IAuthenticationSchemeProvider>();
			Assert.NotNull(schemeProvider);
			
			var scheme = await schemeProvider.GetDefaultAuthenticateSchemeAsync();
			Assert.NotNull(scheme);
			Assert.Equal(typeof(ApiKeyInAuthorizationHeaderHandler), scheme.HandlerType);

			var apiKeyOptionsSnapshot = services.GetService<IOptionsSnapshot<ApiKeyOptions>>();
			var apiKeyOptions = apiKeyOptionsSnapshot?.Get(scheme.Name);
			Assert.NotNull(apiKeyOptions);
			Assert.NotNull(apiKeyOptions.Events?.OnValidateKey);
			Assert.Null(apiKeyOptions.ApiKeyProviderType);

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
			Assert.Equal(typeof(ApiKeyInAuthorizationHeaderHandler), scheme.HandlerType);

			var apiKeyOptionsSnapshot = services.GetService<IOptionsSnapshot<ApiKeyOptions>>();
			var apiKeyOptions = apiKeyOptionsSnapshot?.Get(scheme.Name);
			Assert.NotNull(apiKeyOptions);
			Assert.Null(apiKeyOptions.Events?.OnValidateKey);
			Assert.NotNull(apiKeyOptions.ApiKeyProviderType);
			Assert.Equal(typeof(FakeApiKeyProvider), apiKeyOptions.ApiKeyProviderType);

			var apiKeyProvider = services.GetService<IApiKeyProvider>();
			Assert.NotNull(apiKeyProvider);
			Assert.Equal(typeof(FakeApiKeyProvider), apiKeyProvider.GetType());
		}

		[Fact]
		public async Task Verify_challenge_www_authenticate_header()
		{
			using var response = await _client.GetAsync(TestServerBuilder.BaseUrl);
			Assert.False(response.IsSuccessStatusCode);

			var wwwAuthenticateHeader = response.Headers.WwwAuthenticate;
			Assert.NotEmpty(wwwAuthenticateHeader);

			var wwwAuthenticateHeaderToMatch = Assert.Single(wwwAuthenticateHeader);
			Assert.NotNull(wwwAuthenticateHeaderToMatch);
			Assert.Equal(ApiKeyDefaults.AuthenticationScheme, wwwAuthenticateHeaderToMatch.Scheme);
			Assert.Equal($"realm=\"{TestServerBuilder.Realm}\", charset=\"UTF-8\", in=\"authorization_header\", key_name=\"{FakeApiKeys.KeyName}\"", wwwAuthenticateHeaderToMatch.Parameter);
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
			request.Headers.Authorization = new AuthenticationHeaderValue(ApiKeyDefaults.AuthenticationScheme, FakeApiKeys.FakeKey);
			using var response = await _client.SendAsync(request);
			Assert.True(response.IsSuccessStatusCode);
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		}

		[Fact]
		public async Task Success_with_key_name()
		{
			using var request = new HttpRequestMessage(HttpMethod.Get, TestServerBuilder.BaseUrl);
			request.Headers.Authorization = new AuthenticationHeaderValue(FakeApiKeys.KeyName, FakeApiKeys.FakeKey);
			using var response = await _client.SendAsync(request);
			Assert.True(response.IsSuccessStatusCode);
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		}

		[Fact]
		public async Task Invalid_scheme_unauthorized()
		{
			using var request = new HttpRequestMessage(HttpMethod.Get, TestServerBuilder.BaseUrl);
			request.Headers.Authorization = new AuthenticationHeaderValue("INVALID", FakeApiKeys.FakeKey);
			using var response = await _client.SendAsync(request);
			Assert.False(response.IsSuccessStatusCode);
			Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public async Task Invalid_key_unauthorized()
		{
			using var request = new HttpRequestMessage(HttpMethod.Get, TestServerBuilder.BaseUrl);
			request.Headers.Authorization = new AuthenticationHeaderValue(ApiKeyDefaults.AuthenticationScheme, FakeApiKeys.FakeInvalidKey);
			using var response = await _client.SendAsync(request);
			Assert.False(response.IsSuccessStatusCode);
			Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public async Task Invalid_key_unauthorized_with_key_name()
		{
			using var request = new HttpRequestMessage(HttpMethod.Get, TestServerBuilder.BaseUrl);
			request.Headers.Authorization = new AuthenticationHeaderValue(FakeApiKeys.KeyName, FakeApiKeys.FakeInvalidKey);
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
			request.Headers.Authorization = new AuthenticationHeaderValue(ApiKeyDefaults.AuthenticationScheme, FakeApiKeys.FakeKey);
			using var response = await _clientWithProvider.SendAsync(request);
			Assert.True(response.IsSuccessStatusCode);
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		}

		[Fact]
		public async Task TApiKeyProvider_success_with_key_name()
		{
			using var request = new HttpRequestMessage(HttpMethod.Get, TestServerBuilder.BaseUrl);
			request.Headers.Authorization = new AuthenticationHeaderValue(FakeApiKeys.KeyName, FakeApiKeys.FakeKey);
			using var response = await _clientWithProvider.SendAsync(request);
			Assert.True(response.IsSuccessStatusCode);
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		}

		[Fact]
		public async Task TApiKeyProvider_invalid_scheme_unauthorized()
		{
			using var request = new HttpRequestMessage(HttpMethod.Get, TestServerBuilder.BaseUrl);
			request.Headers.Authorization = new AuthenticationHeaderValue("INVALID", FakeApiKeys.FakeKey);
			using var response = await _clientWithProvider.SendAsync(request);
			Assert.False(response.IsSuccessStatusCode);
			Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public async Task TApiKeyProvider_invalid_key_unauthotized()
		{
			using var request = new HttpRequestMessage(HttpMethod.Get, TestServerBuilder.BaseUrl);
			request.Headers.Authorization = new AuthenticationHeaderValue(ApiKeyDefaults.AuthenticationScheme, FakeApiKeys.FakeInvalidKey);
			using var response = await _clientWithProvider.SendAsync(request);
			Assert.False(response.IsSuccessStatusCode);
			Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public async Task TApiKeyProvider_invalid_key_unauthotized_with_key_name()
		{
			using var request = new HttpRequestMessage(HttpMethod.Get, TestServerBuilder.BaseUrl);
			request.Headers.Authorization = new AuthenticationHeaderValue(FakeApiKeys.KeyName, FakeApiKeys.FakeInvalidKey);
			using var response = await _clientWithProvider.SendAsync(request);
			Assert.False(response.IsSuccessStatusCode);
			Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
		}

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)

                    _client?.Dispose();
                    _server?.Dispose();

                    _clientWithProvider?.Dispose();
                    _serverWithProvider?.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~ApiKeyInAuthorizationHeaderHandlerTests()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
