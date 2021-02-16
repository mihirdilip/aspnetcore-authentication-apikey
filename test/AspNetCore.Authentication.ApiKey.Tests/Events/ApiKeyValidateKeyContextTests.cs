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

        [Fact]
        public async Task Success_and_NoResult()
        {
            using var client = BuildClient(
                context =>
                {
                    var apiKey = FakeApiKeys.Keys.FirstOrDefault(k => k.Key.Equals(context.ApiKey, StringComparison.OrdinalIgnoreCase));
                    if (apiKey != null)
                    {
                        context.Principal = new ClaimsPrincipal(new ClaimsIdentity(context.Scheme.Name));
                        context.Success();
                    }
                    else
                    {
                        context.NoResult();
                    }
                    return Task.CompletedTask;
                }
            );

            var claims = await RunSuccessTests(client);
            Assert.Empty(claims);

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
                    }
                    else
                    {
                        context.ValidationFailed();
                    }
                    return Task.CompletedTask;
                }
            );

            var claims = await RunSuccessTests(client);
            Assert.Empty(claims);

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
                    return Task.CompletedTask;
                }
            );

            var claims = await RunSuccessTests(client);
            Assert.NotEmpty(claims);

            Assert.Equal(claimsSource.Count, claims.Count());
            Assert.Contains(new ClaimDto(FakeApiKeys.FakeNameClaim), claims);
            Assert.Contains(new ClaimDto(FakeApiKeys.FakeRoleClaim), claims);
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
            using var response_unauthorized = await client.GetAsync(TestServerBuilder.UserClaimsUrl);
            Assert.False(response_unauthorized.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Unauthorized, response_unauthorized.StatusCode);
        }

        private async Task<IEnumerable<ClaimDto>> RunSuccessTests(HttpClient client)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, TestServerBuilder.UserClaimsUrl);
            request.Headers.Add(FakeApiKeys.KeyName, FakeApiKeys.FakeKey);
            using var response_ok = await client.SendAsync(request);
            Assert.True(response_ok.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, response_ok.StatusCode);

            var content = await response_ok.Content.ReadAsStringAsync();
            Assert.False(string.IsNullOrWhiteSpace(content));
            return JsonSerializer.Deserialize<IEnumerable<ClaimDto>>(content);
        }

        public void Dispose()
        {
            _serversToDispose.ForEach(s => s.Dispose());
        }
    }
}
