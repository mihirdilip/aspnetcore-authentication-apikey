// Copyright (c) Mihir Dilip. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for license information.

using AspNetCore.Authentication.ApiKey.Tests.Infrastructure;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace AspNetCore.Authentication.ApiKey.Tests.Events
{
    public class ApiKeyValidateKeyContextTests : IDisposable
    {
        private readonly List<TestServer> _serversToDispose = new List<TestServer>();

        public void Dispose()
        {
            _serversToDispose.ForEach(s => s.Dispose());
        }

        [Fact]
        public async Task Success_and_NoResult()
        {
            using var client = BuildClient(
                context =>
                {
                    Assert.Null(context.Principal);
                    Assert.Null(context.Result);
                    Assert.False(string.IsNullOrWhiteSpace(context.ApiKey));

                    var apiKey = FakeApiKeys.Keys.FirstOrDefault(k => k.Key.Equals(context.ApiKey, StringComparison.OrdinalIgnoreCase));
                    if (apiKey != null)
                    {
                        context.Principal = new ClaimsPrincipal(new ClaimsIdentity(context.Scheme.Name));
                        context.Success();

                        Assert.NotNull(context.Principal);
                        Assert.NotNull(context.Result);
                        Assert.NotNull(context.Result.Principal);
                        Assert.True(context.Result.Succeeded);
                    }
                    else
                    {
                        context.NoResult();

                        Assert.Null(context.Principal);
                        Assert.NotNull(context.Result);
                        Assert.Null(context.Result.Principal);
                        Assert.False(context.Result.Succeeded);
                        Assert.True(context.Result.None);
                    }
                    return Task.CompletedTask;
                }
            );

            var principal = await RunSuccessTests(client);
            Assert.Empty(principal.Claims);

            await RunUnauthorizedTests(client);
        }

        [Fact]
        public async Task ValidationSucceeded_and_ValidationFailed()
        {
            using var client = BuildClient(
                context =>
                {
                    var apiKey = FakeApiKeys.Keys.FirstOrDefault(k => k.Key.Equals(context.ApiKey, StringComparison.OrdinalIgnoreCase));
                    if (apiKey != null)
                    {
                        context.ValidationSucceeded();

                        Assert.NotNull(context.Principal);
                        Assert.NotNull(context.Result);
                        Assert.NotNull(context.Result.Principal);
                        Assert.True(context.Result.Succeeded);
                    }
                    else
                    {
                        context.ValidationFailed();

                        Assert.Null(context.Principal);
                        Assert.NotNull(context.Result);
                        Assert.Null(context.Result.Principal);
                        Assert.False(context.Result.Succeeded);
                        Assert.True(context.Result.None);
                    }
                    return Task.CompletedTask;
                }
            );

            var principal = await RunSuccessTests(client);
            Assert.Empty(principal.Claims);

            await RunUnauthorizedTests(client);
        }

        [Fact]
        public async Task ValidationSucceeded_with_claims()
        {
            var claimsSource = new List<Claim>
            {
                FakeApiKeys.FakeNameClaim,
                FakeApiKeys.FakeRoleClaim
            };

            using var client = BuildClient(
                context =>
                {
                    context.ValidationSucceeded(claimsSource);

                    Assert.NotNull(context.Principal);
                    Assert.NotNull(context.Result);
                    Assert.NotNull(context.Result.Principal);
                    Assert.True(context.Result.Succeeded);

                    return Task.CompletedTask;
                }
            );

            var principal = await RunSuccessTests(client);
            Assert.NotEmpty(principal.Claims);

            Assert.Equal(claimsSource.Count, principal.Claims.Count());
            Assert.Contains(new ClaimDto(FakeApiKeys.FakeNameClaim), principal.Claims);
            Assert.Contains(new ClaimDto(FakeApiKeys.FakeRoleClaim), principal.Claims);
        }

        [Fact]
        public async Task ValidationSucceeded_with_ownerName()
        {
            var ownerName = "Owner";
            
            using var client = BuildClient(
                context =>
                {
                    context.ValidationSucceeded(ownerName);

                    Assert.NotNull(context.Principal);
                    Assert.NotNull(context.Result);
                    Assert.NotNull(context.Result.Principal);
                    Assert.True(context.Result.Succeeded);

                    return Task.CompletedTask;
                }
            );

            var principal = await RunSuccessTests(client);
            Assert.NotEmpty(principal.Claims);

            Assert.Equal(2, principal.Claims.Count());
            Assert.Contains(principal.Claims, c => c.Type == ClaimTypes.Name && c.Value == ownerName);
            Assert.Contains(principal.Claims, c => c.Type == ClaimTypes.NameIdentifier && c.Value == ownerName);
        }

        [Fact]
        public async Task ValidationSucceeded_with_ownerName_and_claims()
        {
            var ownerName = "Owner";
            var claimsSource = new List<Claim>
            {
                FakeApiKeys.FakeNameClaim,
                FakeApiKeys.FakeRoleClaim
            };

            using var client = BuildClient(
                context =>
                {
                    context.ValidationSucceeded(ownerName, claimsSource);

                    Assert.NotNull(context.Principal);
                    Assert.NotNull(context.Result);
                    Assert.NotNull(context.Result.Principal);
                    Assert.True(context.Result.Succeeded);

                    return Task.CompletedTask;
                }
            );

            var principal = await RunSuccessTests(client);
            Assert.NotEmpty(principal.Claims);

            Assert.Equal(claimsSource.Count + 1, principal.Claims.Count());
            Assert.Contains(new ClaimDto(FakeApiKeys.FakeNameClaim), principal.Claims);
            Assert.Contains(new ClaimDto(FakeApiKeys.FakeRoleClaim), principal.Claims);
            Assert.Contains(principal.Claims, c => c.Type == ClaimTypes.NameIdentifier && c.Value == ownerName);
        }

        [Fact]
        public async Task ValidationFailed_with_failureMessage()
        {
            using var client = BuildClient(
                context =>
                {
                    var failureMessage = "failure message";
                    context.ValidationFailed(failureMessage);

                    Assert.Null(context.Principal);
                    Assert.NotNull(context.Result);
                    Assert.Null(context.Result.Principal);
                    Assert.False(context.Result.Succeeded);
                    Assert.NotNull(context.Result.Failure);
                    Assert.Equal(failureMessage, context.Result.Failure.Message);

                    return Task.CompletedTask;
                }
            );

            await RunUnauthorizedTests(client);
        }

        [Fact]
        public async Task ValidationFailed_with_failureException()
        {
            using var client = BuildClient(
                context =>
                {
                    var failureException = new Exception();
                    context.ValidationFailed(failureException);

                    Assert.Null(context.Principal);
                    Assert.NotNull(context.Result);
                    Assert.Null(context.Result.Principal);
                    Assert.False(context.Result.Succeeded);
                    Assert.NotNull(context.Result.Failure);
                    Assert.Equal(failureException, context.Result.Failure);

                    return Task.CompletedTask;
                }
            );

            await RunUnauthorizedTests(client);
        }



        private HttpClient BuildClient(Func<ApiKeyValidateKeyContext, Task> onValidateKey)
        {
            var server = TestServerBuilder.BuildInHeaderOrQueryParamsServer(options =>
            {
                options.KeyName = FakeApiKeys.KeyName;
                options.Realm = TestServerBuilder.Realm;
                options.Events.OnValidateKey = onValidateKey;
            });

            _serversToDispose.Add(server);
            return server.CreateClient();
        }

        private async Task RunUnauthorizedTests(HttpClient client)
        {
            using var response_unauthorized = await client.GetAsync(TestServerBuilder.ClaimsPrincipalUrl);
            Assert.False(response_unauthorized.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Unauthorized, response_unauthorized.StatusCode);
        }

        private async Task<ClaimsPrincipalDto> RunSuccessTests(HttpClient client)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, TestServerBuilder.ClaimsPrincipalUrl);
            request.Headers.Add(FakeApiKeys.KeyName, FakeApiKeys.FakeKey);
            using var response_ok = await client.SendAsync(request);
            Assert.True(response_ok.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, response_ok.StatusCode);

            var content = await response_ok.Content.ReadAsStringAsync();
            Assert.False(string.IsNullOrWhiteSpace(content));
            return JsonSerializer.Deserialize<ClaimsPrincipalDto>(content);
        }
    }
}
