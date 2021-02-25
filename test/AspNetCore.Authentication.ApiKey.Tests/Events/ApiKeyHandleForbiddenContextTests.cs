// Copyright (c) Mihir Dilip. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for license information.

using AspNetCore.Authentication.ApiKey.Tests.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace AspNetCore.Authentication.ApiKey.Tests.Events
{
    public class ApiKeyHandleForbiddenContextTests : IDisposable
    {
        private readonly List<TestServer> _serversToDispose = new List<TestServer>();

        public void Dispose()
        {
            _serversToDispose.ForEach(s => s.Dispose());
        }

        [Fact]
        public async Task Handled()
        {
            using var client = BuildClient(
                context =>
                {
                    Assert.False(context.IsHandled);

                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    context.Handled();

                    Assert.True(context.IsHandled);

                    return Task.CompletedTask;
                }
            );

            using var request = new HttpRequestMessage(HttpMethod.Get, TestServerBuilder.ForbiddenUrl);
            request.Headers.Add(FakeApiKeys.KeyName, FakeApiKeys.FakeKey);
            using var response = await client.SendAsync(request);
            
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Handled_not_called()
        {
            using var client = BuildClient(
                context =>
                {
                    Assert.False(context.IsHandled);

                    context.Response.StatusCode = StatusCodes.Status400BadRequest;

                    return Task.CompletedTask;
                }
            );

            using var request = new HttpRequestMessage(HttpMethod.Get, TestServerBuilder.ForbiddenUrl);
            request.Headers.Add(FakeApiKeys.KeyName, FakeApiKeys.FakeKey);
            using var response = await client.SendAsync(request);

            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }



        private HttpClient BuildClient(Func<ApiKeyHandleForbiddenContext, Task> onHandleForbidden)
        {
            var server = TestServerBuilder.BuildInHeaderOrQueryParamsServerWithProvider(options =>
            {
                options.KeyName = FakeApiKeys.KeyName;
                options.Realm = TestServerBuilder.Realm;
                options.Events.OnHandleForbidden = onHandleForbidden;
            });

            _serversToDispose.Add(server);
            return server.CreateClient();
        }
    }
}
