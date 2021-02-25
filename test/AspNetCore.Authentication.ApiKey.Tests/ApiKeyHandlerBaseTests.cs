// Copyright (c) Mihir Dilip. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for license information.

using AspNetCore.Authentication.ApiKey.Tests.Infrastructure;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace AspNetCore.Authentication.ApiKey.Tests
{
    public class ApiKeyHandlerBaseTests 
    {
		private const string HeaderFromEventsKey = nameof(HeaderFromEventsKey);
		private const string HeaderFromEventsValue = nameof(HeaderFromEventsValue);

		#region HandleForbidden

		[Fact]
		public async Task HandleForbidden()
		{
			using var server = TestServerBuilder.BuildInHeaderOrQueryParamsServerWithProvider();
			using var client = server.CreateClient();
			using var request = new HttpRequestMessage(HttpMethod.Get, TestServerBuilder.ForbiddenUrl);
			request.Headers.Add(FakeApiKeys.KeyName, FakeApiKeys.FakeKey);
			using var response = await client.SendAsync(request);

			Assert.False(response.IsSuccessStatusCode);
			Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
			Assert.False(response.Headers.Contains(HeaderFromEventsKey));
		}

		[Fact]
		public async Task HandleForbidden_using_OnHandleForbidden()
		{
			using var server = TestServerBuilder.BuildInHeaderOrQueryParamsServerWithProvider(options =>
			{
				options.Realm = TestServerBuilder.Realm;
				options.KeyName = FakeApiKeys.KeyName;
				options.Events.OnHandleForbidden = context =>
				{
					context.HttpContext.Response.Headers.Add(HeaderFromEventsKey, HeaderFromEventsValue);
					return Task.CompletedTask;
				};
			});
			using var client = server.CreateClient();
			using var request = new HttpRequestMessage(HttpMethod.Get, TestServerBuilder.ForbiddenUrl);
			request.Headers.Add(FakeApiKeys.KeyName, FakeApiKeys.FakeKey);
			using var response = await client.SendAsync(request);

			Assert.False(response.IsSuccessStatusCode);
			Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
			Assert.True(response.Headers.Contains(HeaderFromEventsKey));
			Assert.Contains(HeaderFromEventsValue, response.Headers.GetValues(HeaderFromEventsKey));
		}

		#endregion // HandleForbidden

		#region HandleChallenge

		[Fact]
		public async Task HandleChallange()
		{
			using var server = TestServerBuilder.BuildInHeaderOrQueryParamsServerWithProvider();
			using var client = server.CreateClient();
			using var response = await client.GetAsync(TestServerBuilder.BaseUrl);

			Assert.False(response.IsSuccessStatusCode);
			Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
			Assert.NotEmpty(response.Headers.WwwAuthenticate);
		}

		[Fact]
		public async Task HandleChallange_using_OnHandleChallenge()
		{
			using var server = TestServerBuilder.BuildInHeaderOrQueryParamsServerWithProvider(options =>
			{
				options.Realm = TestServerBuilder.Realm;
				options.KeyName = FakeApiKeys.KeyName;
				options.Events.OnHandleChallenge = context =>
				{
					context.HttpContext.Response.Headers.Add(HeaderFromEventsKey, HeaderFromEventsValue);
					return Task.CompletedTask;
				};
			});
			using var client = server.CreateClient();
			using var response = await client.GetAsync(TestServerBuilder.BaseUrl);

			Assert.False(response.IsSuccessStatusCode);
			Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
			Assert.NotEmpty(response.Headers.WwwAuthenticate);
			Assert.True(response.Headers.Contains(HeaderFromEventsKey));
			Assert.Contains(HeaderFromEventsValue, response.Headers.GetValues(HeaderFromEventsKey));
		}

		[Fact]
		public async Task HandleChallange_using_SuppressWWWAuthenticateHeader()
		{
			using var server = TestServerBuilder.BuildInHeaderOrQueryParamsServerWithProvider(options =>
			{
				options.Realm = TestServerBuilder.Realm;
				options.KeyName = FakeApiKeys.KeyName;
				options.SuppressWWWAuthenticateHeader = true;
			});
			using var client = server.CreateClient();
			using var response = await client.GetAsync(TestServerBuilder.BaseUrl);

			Assert.False(response.IsSuccessStatusCode);
			Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
			Assert.Empty(response.Headers.WwwAuthenticate);
		}

		[Fact]
		public async Task HandleChallange_using_OnHandleChallenge_and_SuppressWWWAuthenticateHeader()
		{
			using var server = TestServerBuilder.BuildInHeaderOrQueryParamsServerWithProvider(options =>
			{
				options.Realm = TestServerBuilder.Realm;
				options.KeyName = FakeApiKeys.KeyName;
				options.SuppressWWWAuthenticateHeader = true;
				options.Events.OnHandleChallenge = context =>
				{
					context.HttpContext.Response.Headers.Add(HeaderFromEventsKey, HeaderFromEventsValue);
					return Task.CompletedTask;
				};
			});
			using var client = server.CreateClient();
			using var response = await client.GetAsync(TestServerBuilder.BaseUrl);

			Assert.False(response.IsSuccessStatusCode);
			Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
			Assert.Empty(response.Headers.WwwAuthenticate);
			Assert.True(response.Headers.Contains(HeaderFromEventsKey));
			Assert.Contains(HeaderFromEventsValue, response.Headers.GetValues(HeaderFromEventsKey));
		}

		#endregion // HandleChallenge

		#region HandleAuthenticate

#if !(NET461 || NETSTANDARD2_0 || NETCOREAPP2_1)

		[Fact]
		public async Task HandleAuthenticate_IgnoreAuthenticationIfAllowAnonymous()
		{
			using var server = TestServerBuilder.BuildInHeaderOrQueryParamsServerWithProvider();
			using var client = server.CreateClient();
			using var response = await client.GetAsync(TestServerBuilder.AnonymousUrl);
			var principal = await DeserializeClaimsPrincipalAsync(response);

			Assert.True(response.IsSuccessStatusCode);
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			Assert.False(principal.Identity.IsAuthenticated);
		}

#endif

		[Fact]
		public async Task HandleAuthenticate_ParseApiKey_empty()
		{
			using var server = TestServerBuilder.BuildInHeaderOrQueryParamsServerWithProvider();
			using var client = server.CreateClient();
			using var request = new HttpRequestMessage(HttpMethod.Get, TestServerBuilder.BaseUrl);
			request.Headers.Add(FakeApiKeys.KeyName, string.Empty);
			using var response = await client.SendAsync(request);

			Assert.False(response.IsSuccessStatusCode);
			Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public async Task HandleAuthenticate_OnValidateKey_result_not_null()
		{
			using var server = TestServerBuilder.BuildInHeaderOrQueryParamsServerWithProvider(options =>
			{
				options.Realm = TestServerBuilder.Realm;
				options.KeyName = FakeApiKeys.KeyName;
				options.Events.OnValidateKey = context =>
				{
					context.ValidationSucceeded();

					Assert.NotNull(context.Result);

					return Task.CompletedTask;
				};
			});
			using var client = server.CreateClient();
			using var request = new HttpRequestMessage(HttpMethod.Get, TestServerBuilder.ClaimsPrincipalUrl);
			request.Headers.Add(FakeApiKeys.KeyName, FakeApiKeys.FakeKey);
			using var response = await client.SendAsync(request);
			var principal = await DeserializeClaimsPrincipalAsync(response);

			Assert.True(response.IsSuccessStatusCode);
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			Assert.DoesNotContain(new ClaimDto(FakeApiKeys.FakeRoleClaim), principal.Claims);		// provider not called
		}

		[Fact]
		public async Task HandleAuthenticate_OnValidateKey_result_null()
		{
			using var server = TestServerBuilder.BuildInHeaderOrQueryParamsServerWithProvider(options =>
			{
				options.Realm = TestServerBuilder.Realm;
				options.KeyName = FakeApiKeys.KeyName;
				options.Events.OnValidateKey = context =>
				{
					Assert.Null(context.Result);

					return Task.CompletedTask;
				};
			});
			using var client = server.CreateClient();
			using var request = new HttpRequestMessage(HttpMethod.Get, TestServerBuilder.ClaimsPrincipalUrl);
			request.Headers.Add(FakeApiKeys.KeyName, FakeApiKeys.FakeKey);
			using var response = await client.SendAsync(request);
			var principal = await DeserializeClaimsPrincipalAsync(response);

			Assert.True(response.IsSuccessStatusCode);
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			Assert.Contains(new ClaimDto(FakeApiKeys.FakeRoleClaim), principal.Claims);		// coming from provider, so provider called
		}

		[Fact]
		public async Task HandleAuthenticate_OnValidateKey_result_null_without_provider_and_OnAuthenticationFailed_throws()
		{
			var expectedExceptionMessage = $"Either {nameof(ApiKeyEvents.OnValidateKey)} delegate on configure options {nameof(ApiKeyOptions.Events)} should be set or use an extention method with type parameter of type {nameof(IApiKeyProvider)}.";

			using var server = TestServerBuilder.BuildInHeaderOrQueryParamsServer(options =>
			{
				options.Realm = TestServerBuilder.Realm;
				options.KeyName = FakeApiKeys.KeyName;
				options.Events.OnValidateKey = context =>
				{
					Assert.Null(context.Result);

					return Task.CompletedTask;
				};

				options.Events.OnAuthenticationFailed = context =>
				{
					Assert.NotNull(context.Exception);
					Assert.IsType<InvalidOperationException>(context.Exception);
					Assert.Equal(expectedExceptionMessage, context.Exception.Message);

					return Task.CompletedTask;
				};
			});
			using var client = server.CreateClient();
			using var request = new HttpRequestMessage(HttpMethod.Get, TestServerBuilder.BaseUrl);
			request.Headers.Add(FakeApiKeys.KeyName, FakeApiKeys.FakeKey);

			var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
			{
				using var response = await client.SendAsync(request);

				Assert.False(response.IsSuccessStatusCode);
				Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
			});

			Assert.Equal(expectedExceptionMessage, exception.Message);
		}

		[Fact]
		public async Task HandleAuthenticate_OnValidateKey_result_null_without_provider_and_OnAuthenticationFailed_does_not_throw()
		{
			var expectedExceptionMessage = $"Either {nameof(ApiKeyEvents.OnValidateKey)} delegate on configure options {nameof(ApiKeyOptions.Events)} should be set or use an extention method with type parameter of type {nameof(IApiKeyProvider)}.";

			using var server = TestServerBuilder.BuildInHeaderOrQueryParamsServer(options =>
			{
				options.Realm = TestServerBuilder.Realm;
				options.KeyName = FakeApiKeys.KeyName;
				options.Events.OnValidateKey = context =>
				{
					Assert.Null(context.Result);

					return Task.CompletedTask;
				};

				options.Events.OnAuthenticationFailed = context =>
				{
					Assert.Null(context.Result);
					Assert.NotNull(context.Exception);
					Assert.IsType<InvalidOperationException>(context.Exception);
					Assert.Equal(expectedExceptionMessage, context.Exception.Message);

					context.NoResult();

					Assert.NotNull(context.Result);

					return Task.CompletedTask;
				};
			});
			using var client = server.CreateClient();
			using var request = new HttpRequestMessage(HttpMethod.Get, TestServerBuilder.BaseUrl);
			request.Headers.Add(FakeApiKeys.KeyName, FakeApiKeys.FakeKey);
			using var response = await client.SendAsync(request);

			Assert.False(response.IsSuccessStatusCode);
			Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public async Task HandleAuthenticate_ForLegacyIgnoreExtraValidatedApiKeyCheck()
		{
			using var server = TestServerBuilder.BuildInHeaderOrQueryParamsServerWithProvider();
			using var client = server.CreateClient();
			using var request = new HttpRequestMessage(HttpMethod.Get, TestServerBuilder.BaseUrl);
			request.Headers.Add(FakeApiKeys.KeyName, FakeApiKeys.FakeKeyForLegacyIgnoreExtraValidatedApiKeyCheck);
			using var response = await client.SendAsync(request);

			Assert.False(response.IsSuccessStatusCode);
			Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public async Task HandleAuthenticate_OnAuthenticationSucceeded_result_null()
		{
			using var server = TestServerBuilder.BuildInHeaderOrQueryParamsServerWithProvider(options =>
			{
				options.Realm = TestServerBuilder.Realm;
				options.KeyName = FakeApiKeys.KeyName;
				options.Events.OnAuthenticationSucceeded = context =>
				{
					Assert.Null(context.Result);

					return Task.CompletedTask;
				};
			});
			using var client = server.CreateClient();
			using var request = new HttpRequestMessage(HttpMethod.Get, TestServerBuilder.BaseUrl);
			request.Headers.Add(FakeApiKeys.KeyName, FakeApiKeys.FakeKey);
			using var response = await client.SendAsync(request);

			Assert.True(response.IsSuccessStatusCode);
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		}

		[Fact]
		public async Task HandleAuthenticate_OnAuthenticationSucceeded_result_and_principal_null()
		{
			using var server = TestServerBuilder.BuildInHeaderOrQueryParamsServerWithProvider(options =>
			{
				options.Realm = TestServerBuilder.Realm;
				options.KeyName = FakeApiKeys.KeyName;
				options.Events.OnAuthenticationSucceeded = context =>
				{
					context.RejectPrincipal();
					
					Assert.Null(context.Result);
					Assert.Null(context.Principal);

					return Task.CompletedTask;
				};
			});
			using var client = server.CreateClient();
			using var request = new HttpRequestMessage(HttpMethod.Get, TestServerBuilder.BaseUrl);
			request.Headers.Add(FakeApiKeys.KeyName, FakeApiKeys.FakeKey);
			using var response = await client.SendAsync(request);

			Assert.False(response.IsSuccessStatusCode);
			Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public async Task HandleAuthenticate_OnAuthenticationSucceeded_result_not_null()
		{
			using var server = TestServerBuilder.BuildInHeaderOrQueryParamsServerWithProvider(options =>
			{
				options.Realm = TestServerBuilder.Realm;
				options.KeyName = FakeApiKeys.KeyName;
				options.Events.OnAuthenticationSucceeded = context =>
				{
					context.Fail("test");

					Assert.NotNull(context.Result);
					Assert.NotNull(context.Principal);

					return Task.CompletedTask;
				};
			});
			using var client = server.CreateClient();
			using var request = new HttpRequestMessage(HttpMethod.Get, TestServerBuilder.BaseUrl);
			request.Headers.Add(FakeApiKeys.KeyName, FakeApiKeys.FakeKey);
			using var response = await client.SendAsync(request);

			Assert.False(response.IsSuccessStatusCode);
			Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		#endregion // HandleAuthenticate

		private async Task<ClaimsPrincipalDto> DeserializeClaimsPrincipalAsync(HttpResponseMessage response)
        {
			return JsonSerializer.Deserialize<ClaimsPrincipalDto>(await response.Content.ReadAsStringAsync());
		}
	}
}
