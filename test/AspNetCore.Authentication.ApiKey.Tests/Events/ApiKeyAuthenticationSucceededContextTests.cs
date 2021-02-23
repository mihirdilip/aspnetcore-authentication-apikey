// Copyright (c) Mihir Dilip. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for license information.

using AspNetCore.Authentication.ApiKey.Tests.Infrastructure;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace AspNetCore.Authentication.ApiKey.Tests.Events
{
    public class ApiKeyAuthenticationSucceededContextTests : IDisposable
    {
        private readonly List<TestServer> _serversToDispose = new List<TestServer>();

        public void Dispose()
        {
            _serversToDispose.ForEach(s => s.Dispose());
        }

        [Fact]
        public async Task Principal_not_null()
        {
            using var client = BuildClient(
                context =>
                {
                    Assert.NotNull(context.Principal);
                    Assert.Null(context.Result);
                    return Task.CompletedTask;
                }
            );

            var principal = await RunSuccessTests(client);
            Assert.True(principal.Identity.IsAuthenticated);
        }

        [Fact]
        public async Task ReplacePrincipal_null_throws_argument_null_exception()
        {
            using var client = BuildClient(
                context =>
                {
                    Assert.Throws<ArgumentNullException>(() => context.ReplacePrincipal(null));
                    return Task.CompletedTask;
                }
            );

            await RunSuccessTests(client);
        }

        [Fact]
        public async Task ReplacePrincipal()
        {
            using var client = BuildClient(
                context =>
                {
                    var newPrincipal = new ClaimsPrincipal();
                    context.ReplacePrincipal(newPrincipal);

                    Assert.NotNull(context.Principal);
                    Assert.Equal(newPrincipal, context.Principal);

                    return Task.CompletedTask;
                }
            );

            await RunUnauthorizedTests(client);
        }

        [Fact]
        public async Task RejectPrincipal()
        {
            using var client = BuildClient(
                context =>
                {
                    context.RejectPrincipal();

                    Assert.Null(context.Principal);

                    return Task.CompletedTask;
                }
            );

            await RunUnauthorizedTests(client);
        }

        [Fact]
        public async Task AddClaim()
        {
            var claim = new Claim(ClaimTypes.Actor, "Actor");

            using var client = BuildClient(
                context =>
                {
                    context.AddClaim(claim);

                    Assert.Contains(context.Principal.Claims, c => c.Type == claim.Type && c.Value == claim.Value);

                    return Task.CompletedTask;
                }
            );

            var principal = await RunSuccessTests(client);
            Assert.Contains(new ClaimDto(claim), principal.Claims);
        }

        [Fact]
        public async Task AddClaims()
        {
            var claims = new List<Claim>{
                new Claim(ClaimTypes.Actor, "Actor"),
                new Claim(ClaimTypes.Country, "Country")
            };

            using var client = BuildClient(
                context =>
                {
                    context.AddClaims(claims);

                    Assert.Contains(context.Principal.Claims, c => c.Type == claims[0].Type && c.Value == claims[0].Value);
                    Assert.Contains(context.Principal.Claims, c => c.Type == claims[1].Type && c.Value == claims[1].Value);

                    return Task.CompletedTask;
                }
            );

            var principal = await RunSuccessTests(client);
            Assert.Contains(new ClaimDto(claims[0]), principal.Claims);
            Assert.Contains(new ClaimDto(claims[1]), principal.Claims);
        }



        private HttpClient BuildClient(Func<ApiKeyAuthenticationSucceededContext, Task> onAuthenticationSucceeded)
        {
            var server = TestServerBuilder.BuildInHeaderOrQueryParamsServerWithProvider(options =>
            {
                options.KeyName = FakeApiKeys.KeyName;
                options.Realm = TestServerBuilder.Realm;
                options.Events.OnAuthenticationSucceeded = onAuthenticationSucceeded;
            });

            _serversToDispose.Add(server);
            return server.CreateClient();
        }

        private async Task RunUnauthorizedTests(HttpClient client)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, TestServerBuilder.ClaimsPrincipalUrl);
            request.Headers.Add(FakeApiKeys.KeyName, FakeApiKeys.FakeKey);
            using var response_unauthorized = await client.SendAsync(request);
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
