// Copyright (c) Mihir Dilip. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for license information.

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AspNetCore.Authentication.ApiKey.Tests.Infrastructure
{
    partial class TestServerBuilder
    {
        internal static string BaseUrl = "http://localhost/";
        internal static string AnonymousUrl = $"{BaseUrl}anonymous";
        internal static string UserClaimsUrl = $"{BaseUrl}user-claims";
        internal static string Realm = "ApiKeyTests";

        internal static TestServer BuildInAuthorizationHeaderServer(Action<ApiKeyOptions> configureOptions = null)
        {
            return BuildTestServer(
                services =>
                {
                    var authBuilder = services.AddAuthentication(ApiKeyDefaults.AuthenticationScheme)
                        .AddApiKeyInAuthorizationHeader(configureOptions ?? DefaultApiKeyOptionsWithOnValidateKey());
                }
            );
        }

        internal static TestServer BuildInAuthorizationHeaderServerWithProvider(Action<ApiKeyOptions> configureOptions = null)
        {
            return BuildTestServer(
                services =>
                {
                    var authBuilder = services.AddAuthentication(ApiKeyDefaults.AuthenticationScheme)
                        .AddApiKeyInAuthorizationHeader<FakeApiKeyProvider>(configureOptions ?? DefaultApiKeyOptions());
                }
            );
        }

        internal static TestServer BuildInHeaderServer(Action<ApiKeyOptions> configureOptions = null)
        {
            return BuildTestServer(
                services =>
                {
                    var authBuilder = services.AddAuthentication(ApiKeyDefaults.AuthenticationScheme)
                        .AddApiKeyInHeader(configureOptions ?? DefaultApiKeyOptionsWithOnValidateKey());
                }
            );
        }

        internal static TestServer BuildInHeaderServerWithProvider(Action<ApiKeyOptions> configureOptions = null)
        {
            return BuildTestServer(
                services =>
                {
                    var authBuilder = services.AddAuthentication(ApiKeyDefaults.AuthenticationScheme)
                        .AddApiKeyInHeader<FakeApiKeyProvider>(configureOptions ?? DefaultApiKeyOptions());
                }
            );
        }

        internal static TestServer BuildInQueryParamsServer(Action<ApiKeyOptions> configureOptions = null)
        {
            return BuildTestServer(
                services =>
                {
                    var authBuilder = services.AddAuthentication(ApiKeyDefaults.AuthenticationScheme)
                        .AddApiKeyInQueryParams(configureOptions ?? DefaultApiKeyOptionsWithOnValidateKey());
                }
            );
        }

        internal static TestServer BuildInQueryParamsServerWithProvider(Action<ApiKeyOptions> configureOptions = null)
        {
            return BuildTestServer(
                services =>
                {
                    var authBuilder = services.AddAuthentication(ApiKeyDefaults.AuthenticationScheme)
                        .AddApiKeyInQueryParams<FakeApiKeyProvider>(configureOptions ?? DefaultApiKeyOptions());
                }
            );
        }

        internal static TestServer BuildInHeaderOrQueryParamsServer(Action<ApiKeyOptions> configureOptions = null)
        {
            return BuildTestServer(
                services =>
                {
                    var authBuilder = services.AddAuthentication(ApiKeyDefaults.AuthenticationScheme)
                        .AddApiKeyInHeaderOrQueryParams(configureOptions ?? DefaultApiKeyOptionsWithOnValidateKey());
                }
            );
        }

        internal static TestServer BuildInHeaderOrQueryParamsServerWithProvider(Action<ApiKeyOptions> configureOptions = null)
        {
            return BuildTestServer(
                services =>
                {
                    var authBuilder = services.AddAuthentication(ApiKeyDefaults.AuthenticationScheme)
                        .AddApiKeyInHeaderOrQueryParams<FakeApiKeyProvider>(configureOptions ?? DefaultApiKeyOptions());
                }
            );
        }

        internal static TestServer BuildTestServer(Action<IServiceCollection> configureServices, Action<IApplicationBuilder> configure = null)
        {
            if (configureServices == null) throw new ArgumentNullException(nameof(configureServices));

            return new TestServer(
                new WebHostBuilder()

                    .ConfigureServices(services =>
                    {

#if !(NET461 || NETSTANDARD2_0 || NETCOREAPP2_1)
                        services.AddRouting();
                        services.AddAuthorization();
#endif

                        configureServices(services);

                    })


                    .Configure(app =>
                    {

#if !(NET461 || NETSTANDARD2_0 || NETCOREAPP2_1)
                        
                        app.UseRouting();
                        app.UseAuthentication();
                        app.UseAuthorization();

                        if (configure != null)
                        {
                            configure(app);
                        }
                        else
                        {
                            app.UseEndpoints(endpoints =>
                            {
                                endpoints.MapGet("/", async context =>
                                {
                                    await context.Response.WriteAsync("Hello World!");
                                }).RequireAuthorization();

                                endpoints.MapGet("/user-claims", async context =>
                                {
                                    context.Response.ContentType = "application/json";
                                    await context.Response.WriteAsync(JsonSerializer.Serialize(context.User.Claims.Select(c => new ClaimDto(c))));
                                }).RequireAuthorization();

                                endpoints.MapGet("/anonymous", async context =>
                                {
                                    await context.Response.WriteAsync("Hello Anonymous World!");
                                }).WithMetadata(new Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute());
                            });
                        }

#else

                        app.UseAuthentication();

                        if (configure != null)
                        {
                            configure(app);
                        }
                        else
                        {
                            app.Run(async (context) =>
                            {
                                if (!context.User.Identity.IsAuthenticated)
                                {
                                    await context.ChallengeAsync();
                                    return;
                                }

                                if (context.Request.Path == "/user-claims")
                                {
                                    context.Response.ContentType = "application/json";
                                    await context.Response.WriteAsync(JsonSerializer.Serialize(context.User.Claims.Select(c => new ClaimDto(c))));
                                    return;
                                }

                                await context.Response.WriteAsync("Hello World!");
                            });
                        }

#endif
                        
                    })
            );
        }



        private static Action<ApiKeyOptions> DefaultApiKeyOptions(string keyName = FakeApiKeys.KeyName)
        {
            return options =>
            {
                options.Realm = Realm;
                options.KeyName = keyName;
            };
        }

        private static Action<ApiKeyOptions> DefaultApiKeyOptionsWithOnValidateKey(string keyName = FakeApiKeys.KeyName)
        {
            return options =>
            {
                options.Realm = Realm;
                options.KeyName = keyName;
                options.Events.OnValidateKey =
                    context =>
                    {
                        var apiKey = FakeApiKeys.Keys.FirstOrDefault(k => k.Key.Equals(context.ApiKey, StringComparison.OrdinalIgnoreCase));
                        if (apiKey != null)
                        {
                            context.ValidationSucceeded(apiKey.OwnerName);
                        }
                        else
                        {
                            context.ValidationFailed();
                        }
                        return Task.CompletedTask;
                    };
            };
        }
    }
}
