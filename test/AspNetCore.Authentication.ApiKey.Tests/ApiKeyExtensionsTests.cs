// Copyright (c) Mihir Dilip. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for license information.

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AspNetCore.Authentication.ApiKey.Tests
{
    public class ApiKeyExtensionsTests
    {
        #region API Key - In Header

        #region Verify Auth Scheme

        [Fact]
        public async Task AddApiKeyInHeader_verify_auth_scheme_handler_default()
        {
            var scheme = await GetSchemeAsync(a => a.AddApiKeyInHeader());
            Assert.NotNull(scheme);
            Assert.Equal(typeof(ApiKeyInHeaderHandler).Name, scheme.HandlerType.Name);
            Assert.Null(scheme.DisplayName);
        }

        [Fact]
        public async Task AddApiKeyInHeader_verify_auth_scheme_handler_with_scheme()
        {
            var schemeName = "CustomScheme";
            var scheme = await GetSchemeAsync(a => a.AddApiKeyInHeader(schemeName), schemeName);
            Assert.NotNull(scheme);
            Assert.Equal(typeof(ApiKeyInHeaderHandler).Name, scheme.HandlerType.Name);
            Assert.Null(scheme.DisplayName);
            Assert.Equal(schemeName, scheme.Name);
        }

        [Fact]
        public async Task AddApiKeyInHeader_verify_auth_scheme_handler_with_configureOptions()
        {
            var scheme = await GetSchemeAsync(a => a.AddApiKeyInHeader(_ => { }));
            Assert.NotNull(scheme);
            Assert.Equal(typeof(ApiKeyInHeaderHandler).Name, scheme.HandlerType.Name);
            Assert.Null(scheme.DisplayName);
        }

        [Fact]
        public async Task AddApiKeyInHeader_verify_auth_scheme_handler_with_scheme_and_configureOptions()
        {
            var schemeName = "CustomScheme";
            var scheme = await GetSchemeAsync(a => a.AddApiKeyInHeader(schemeName, _ => { }), schemeName);
            Assert.NotNull(scheme);
            Assert.Equal(typeof(ApiKeyInHeaderHandler).Name, scheme.HandlerType.Name);
            Assert.Null(scheme.DisplayName);
            Assert.Equal(schemeName, scheme.Name);
        }

        [Fact]
        public async Task AddApiKeyInHeader_verify_auth_scheme_handler_with_scheme_displayName_and_configureOptions()
        {
            var schemeName = "CustomScheme";
            var displayName = "DisplayName";
            var scheme = await GetSchemeAsync(a => a.AddApiKeyInHeader(schemeName, displayName, _ => { }), schemeName);
            Assert.NotNull(scheme);
            Assert.Equal(typeof(ApiKeyInHeaderHandler).Name, scheme.HandlerType.Name);
            Assert.NotNull(scheme.DisplayName);
            Assert.Equal(displayName, scheme.DisplayName);
            Assert.Equal(schemeName, scheme.Name);
        }


        [Fact]
        public async Task AddApiKeyInHeader_TApiKeyProvider_verify_auth_scheme_handler_default()
        {
            var scheme = await GetSchemeAsync(a => a.AddApiKeyInHeader<MockApiKeyProvider>(), ApiKeyDefaults.AuthenticationScheme);
            Assert.NotNull(scheme);
            Assert.Equal(typeof(ApiKeyInHeaderHandler).Name, scheme.HandlerType.Name);
            Assert.Null(scheme.DisplayName);
        }

        [Fact]
        public async Task AddApiKeyInHeader_TApiKeyProvider_verify_auth_scheme_handler_with_scheme()
        {
            var schemeName = "CustomScheme";
            var scheme = await GetSchemeAsync(a => a.AddApiKeyInHeader<MockApiKeyProvider>(schemeName), schemeName);
            Assert.NotNull(scheme);
            Assert.Equal(typeof(ApiKeyInHeaderHandler).Name, scheme.HandlerType.Name);
            Assert.Null(scheme.DisplayName);
            Assert.Equal(schemeName, scheme.Name);
        }

        [Fact]
        public async Task AddApiKeyInHeader_TApiKeyProvider_verify_auth_scheme_handler_with_configureOptions()
        {
            var scheme = await GetSchemeAsync(a => a.AddApiKeyInHeader<MockApiKeyProvider>(_ => { }));
            Assert.NotNull(scheme);
            Assert.Equal(typeof(ApiKeyInHeaderHandler).Name, scheme.HandlerType.Name);
            Assert.Null(scheme.DisplayName);
        }

        [Fact]
        public async Task AddApiKeyInHeader_TApiKeyProvider_verify_auth_scheme_handler_with_scheme_and_configureOptions()
        {
            var schemeName = "CustomScheme";
            var scheme = await GetSchemeAsync(a => a.AddApiKeyInHeader<MockApiKeyProvider>(schemeName, _ => { }), schemeName);
            Assert.NotNull(scheme);
            Assert.Equal(typeof(ApiKeyInHeaderHandler).Name, scheme.HandlerType.Name);
            Assert.Null(scheme.DisplayName);
            Assert.Equal(schemeName, scheme.Name);
        }

        [Fact]
        public async Task AddApiKeyInHeader_TApiKeyProvider_verify_auth_scheme_handler_with_scheme_displayName_and_configureOptions()
        {
            var schemeName = "CustomScheme";
            var displayName = "DisplayName";
            var scheme = await GetSchemeAsync(a => a.AddApiKeyInHeader<MockApiKeyProvider>(schemeName, displayName, _ => { }), schemeName);
            Assert.NotNull(scheme);
            Assert.Equal(typeof(ApiKeyInHeaderHandler).Name, scheme.HandlerType.Name);
            Assert.NotNull(scheme.DisplayName);
            Assert.Equal(displayName, scheme.DisplayName);
            Assert.Equal(schemeName, scheme.Name);
        }

        #endregion  // Verify Auth Scheme

        #region Allows Multiple Schemes

        [Fact]
        public async Task AddApiKeyInHeader_allows_multiple_schemes()
        {
            var schemeName = "CustomScheme";
            var displayName = "DisplayName";

            var services = new ServiceCollection();
            services.AddAuthentication()
                .AddApiKeyInHeader()
                .AddApiKeyInHeader(schemeName, displayName, _ => { });

            var sp = services.BuildServiceProvider();
            var schemeProvider = sp.GetRequiredService<IAuthenticationSchemeProvider>();
            var defaultScheme = await schemeProvider.GetSchemeAsync(ApiKeyDefaults.AuthenticationScheme);
            var scheme = await schemeProvider.GetSchemeAsync(schemeName);

            Assert.NotNull(defaultScheme);
            Assert.Equal(typeof(ApiKeyInHeaderHandler).Name, defaultScheme.HandlerType.Name);
            Assert.Null(defaultScheme.DisplayName);
            Assert.Equal(ApiKeyDefaults.AuthenticationScheme, defaultScheme.Name);

            Assert.NotNull(scheme);
            Assert.Equal(typeof(ApiKeyInHeaderHandler).Name, scheme.HandlerType.Name);
            Assert.NotNull(scheme.DisplayName);
            Assert.Equal(displayName, scheme.DisplayName);
            Assert.Equal(schemeName, scheme.Name);
        }

        [Fact]
        public async Task AddApiKeyInHeader_TApiKeyProvider_allows_multiple_schemes()
        {
            var schemeName = "CustomScheme";
            var displayName = "DisplayName";

            var services = new ServiceCollection();
            services.AddAuthentication()
                .AddApiKeyInHeader<MockApiKeyProvider>()
                .AddApiKeyInHeader<MockApiKeyProvider>(schemeName, displayName, _ => { });

            var sp = services.BuildServiceProvider();
            var schemeProvider = sp.GetRequiredService<IAuthenticationSchemeProvider>();
            var defaultScheme = await schemeProvider.GetSchemeAsync(ApiKeyDefaults.AuthenticationScheme);
            var scheme = await schemeProvider.GetSchemeAsync(schemeName);

            Assert.NotNull(defaultScheme);
            Assert.Equal(typeof(ApiKeyInHeaderHandler).Name, defaultScheme.HandlerType.Name);
            Assert.Null(defaultScheme.DisplayName);
            Assert.Equal(ApiKeyDefaults.AuthenticationScheme, defaultScheme.Name);

            Assert.NotNull(scheme);
            Assert.Equal(typeof(ApiKeyInHeaderHandler).Name, scheme.HandlerType.Name);
            Assert.NotNull(scheme.DisplayName);
            Assert.Equal(displayName, scheme.DisplayName);
            Assert.Equal(schemeName, scheme.Name);
        }

        #endregion  // Allows Multiple Schemes

        #region TApiKeyProvider tests

        [Fact]
        public void AddApiKeyInHeader_TApiKeyProvider_IApiKeyProvider_is_registered_as_transient()
        {
            var services = new ServiceCollection();
            services.AddAuthentication()
                .AddApiKeyInHeader<MockApiKeyProvider>();

            var serviceDescriptor = Assert.Single(services, s => s.ServiceType == typeof(IApiKeyProvider));
            Assert.Equal(typeof(IApiKeyProvider), serviceDescriptor.ServiceType);
            Assert.Equal(typeof(MockApiKeyProvider), serviceDescriptor.ImplementationType);
            Assert.Equal(ServiceLifetime.Transient, serviceDescriptor.Lifetime);

            var sp = services.BuildServiceProvider();
            var provider = sp.GetService<IApiKeyProvider>();

            Assert.NotNull(provider);
            Assert.Equal(typeof(MockApiKeyProvider), provider.GetType());
        }

        [Fact]
        public void AddApiKeyInHeader_TApiKeyProvider_does_not_replace_previously_user_registered_IApiKeyProvider()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IApiKeyProvider, MockApiKeyProvider2>();
            services.AddAuthentication()
                .AddApiKeyInHeader<MockApiKeyProvider>();

            var serviceDescriptors = services.Where(s => s.ServiceType == typeof(IApiKeyProvider));
            Assert.Equal(2, serviceDescriptors.Count());

            var serviceDescriptor = Assert.Single(serviceDescriptors, s => s.ImplementationType == typeof(MockApiKeyProvider));
            Assert.Equal(typeof(IApiKeyProvider), serviceDescriptor.ServiceType);
            Assert.Equal(typeof(MockApiKeyProvider), serviceDescriptor.ImplementationType);
            Assert.Equal(ServiceLifetime.Transient, serviceDescriptor.Lifetime);

            serviceDescriptor = Assert.Single(serviceDescriptors, s => s.ImplementationType == typeof(MockApiKeyProvider2));
            Assert.Equal(typeof(IApiKeyProvider), serviceDescriptor.ServiceType);
            Assert.Equal(typeof(MockApiKeyProvider2), serviceDescriptor.ImplementationType);
            Assert.Equal(ServiceLifetime.Singleton, serviceDescriptor.Lifetime);
        }

        #endregion  // TApiKeyProvider tests

        #region Allows chaining

        [Fact]
        public void AddApiKeyInHeader_allows_chaining_default()
        {
            var authenticationBuilder = new ServiceCollection().AddAuthentication();
            Assert.Same(authenticationBuilder, authenticationBuilder.AddApiKeyInHeader());
        }

        [Fact]
        public void AddApiKeyInHeader_allows_chaining_with_scheme()
        {
            var authenticationBuilder = new ServiceCollection().AddAuthentication();
            Assert.Same(authenticationBuilder, authenticationBuilder.AddApiKeyInHeader(string.Empty));
        }

        [Fact]
        public void AddApiKeyInHeader_allows_chaining_with_configureOptions()
        {
            var authenticationBuilder = new ServiceCollection().AddAuthentication();
            Assert.Same(authenticationBuilder, authenticationBuilder.AddApiKeyInHeader(_ => { }));
        }

        [Fact]
        public void AddApiKeyInHeader_allows_chaining_with_scheme_and_configureOptions()
        {
            var authenticationBuilder = new ServiceCollection().AddAuthentication();
            Assert.Same(authenticationBuilder, authenticationBuilder.AddApiKeyInHeader(string.Empty, _ => { }));
        }

        [Fact]
        public void AddApiKeyInHeader_allows_chaining_with_scheme_displayName_and_configureOptions()
        {
            var authenticationBuilder = new ServiceCollection().AddAuthentication();
            Assert.Same(authenticationBuilder, authenticationBuilder.AddApiKeyInHeader(string.Empty, string.Empty, _ => { }));
        }


        [Fact]
        public void AddApiKeyInHeader_TApiKeyProvider_allows_chaining()
        {
            var authenticationBuilder = new ServiceCollection().AddAuthentication();
            Assert.Same(authenticationBuilder, authenticationBuilder.AddApiKeyInHeader<MockApiKeyProvider>());
        }

        [Fact]
        public void AddApiKeyInHeader_TApiKeyProvider_allows_chaining_with_scheme()
        {
            var authenticationBuilder = new ServiceCollection().AddAuthentication();
            Assert.Same(authenticationBuilder, authenticationBuilder.AddApiKeyInHeader<MockApiKeyProvider>(string.Empty));
        }

        [Fact]
        public void AddApiKeyInHeader_TApiKeyProvider_allows_chaining_with_configureOptions()
        {
            var authenticationBuilder = new ServiceCollection().AddAuthentication();
            Assert.Same(authenticationBuilder, authenticationBuilder.AddApiKeyInHeader<MockApiKeyProvider>(_ => { }));
        }

        [Fact]
        public void AddApiKeyInHeader_TApiKeyProvider_allows_chaining_with_scheme_and_configureOptions()
        {
            var authenticationBuilder = new ServiceCollection().AddAuthentication();
            Assert.Same(authenticationBuilder, authenticationBuilder.AddApiKeyInHeader<MockApiKeyProvider>(string.Empty, _ => { }));
        }

        [Fact]
        public void AddApiKeyInHeader_TApiKeyProvider_allows_chaining_with_scheme_displayName_and_configureOptions()
        {
            var authenticationBuilder = new ServiceCollection().AddAuthentication();
            Assert.Same(authenticationBuilder, authenticationBuilder.AddApiKeyInHeader<MockApiKeyProvider>(string.Empty, string.Empty, _ => { }));
        }

        #endregion // Allows chaining

        #endregion // API Key - In Header

        #region API Key - In Authorization Header

        #region Verify Auth Scheme

        [Fact]
        public async Task AddApiKeyInAuthorizationHeader_verify_auth_scheme_handler_default()
        {
            var scheme = await GetSchemeAsync(a => a.AddApiKeyInAuthorizationHeader());
            Assert.NotNull(scheme);
            Assert.Equal(typeof(ApiKeyInAuthorizationHeaderHandler).Name, scheme.HandlerType.Name);
            Assert.Null(scheme.DisplayName);
        }

        [Fact]
        public async Task AddApiKeyInAuthorizationHeader_verify_auth_scheme_handler_with_scheme()
        {
            var schemeName = "CustomScheme";
            var scheme = await GetSchemeAsync(a => a.AddApiKeyInAuthorizationHeader(schemeName), schemeName);
            Assert.NotNull(scheme);
            Assert.Equal(typeof(ApiKeyInAuthorizationHeaderHandler).Name, scheme.HandlerType.Name);
            Assert.Null(scheme.DisplayName);
            Assert.Equal(schemeName, scheme.Name);
        }

        [Fact]
        public async Task AddApiKeyInAuthorizationHeader_verify_auth_scheme_handler_with_configureOptions()
        {
            var scheme = await GetSchemeAsync(a => a.AddApiKeyInAuthorizationHeader(_ => { }));
            Assert.NotNull(scheme);
            Assert.Equal(typeof(ApiKeyInAuthorizationHeaderHandler).Name, scheme.HandlerType.Name);
            Assert.Null(scheme.DisplayName);
        }

        [Fact]
        public async Task AddApiKeyInAuthorizationHeader_verify_auth_scheme_handler_with_scheme_and_configureOptions()
        {
            var schemeName = "CustomScheme";
            var scheme = await GetSchemeAsync(a => a.AddApiKeyInAuthorizationHeader(schemeName, _ => { }), schemeName);
            Assert.NotNull(scheme);
            Assert.Equal(typeof(ApiKeyInAuthorizationHeaderHandler).Name, scheme.HandlerType.Name);
            Assert.Null(scheme.DisplayName);
            Assert.Equal(schemeName, scheme.Name);
        }

        [Fact]
        public async Task AddApiKeyInAuthorizationHeader_verify_auth_scheme_handler_with_scheme_displayName_and_configureOptions()
        {
            var schemeName = "CustomScheme";
            var displayName = "DisplayName";
            var scheme = await GetSchemeAsync(a => a.AddApiKeyInAuthorizationHeader(schemeName, displayName, _ => { }), schemeName);
            Assert.NotNull(scheme);
            Assert.Equal(typeof(ApiKeyInAuthorizationHeaderHandler).Name, scheme.HandlerType.Name);
            Assert.NotNull(scheme.DisplayName);
            Assert.Equal(displayName, scheme.DisplayName);
            Assert.Equal(schemeName, scheme.Name);
        }


        [Fact]
        public async Task AddApiKeyInAuthorizationHeader_TApiKeyProvider_verify_auth_scheme_handler_default()
        {
            var scheme = await GetSchemeAsync(a => a.AddApiKeyInAuthorizationHeader<MockApiKeyProvider>(), ApiKeyDefaults.AuthenticationScheme);
            Assert.NotNull(scheme);
            Assert.Equal(typeof(ApiKeyInAuthorizationHeaderHandler).Name, scheme.HandlerType.Name);
            Assert.Null(scheme.DisplayName);
        }

        [Fact]
        public async Task AddApiKeyInAuthorizationHeader_TApiKeyProvider_verify_auth_scheme_handler_with_scheme()
        {
            var schemeName = "CustomScheme";
            var scheme = await GetSchemeAsync(a => a.AddApiKeyInAuthorizationHeader<MockApiKeyProvider>(schemeName), schemeName);
            Assert.NotNull(scheme);
            Assert.Equal(typeof(ApiKeyInAuthorizationHeaderHandler).Name, scheme.HandlerType.Name);
            Assert.Null(scheme.DisplayName);
            Assert.Equal(schemeName, scheme.Name);
        }

        [Fact]
        public async Task AddApiKeyInAuthorizationHeader_TApiKeyProvider_verify_auth_scheme_handler_with_configureOptions()
        {
            var scheme = await GetSchemeAsync(a => a.AddApiKeyInAuthorizationHeader<MockApiKeyProvider>(_ => { }));
            Assert.NotNull(scheme);
            Assert.Equal(typeof(ApiKeyInAuthorizationHeaderHandler).Name, scheme.HandlerType.Name);
            Assert.Null(scheme.DisplayName);
        }

        [Fact]
        public async Task AddApiKeyInAuthorizationHeader_TApiKeyProvider_verify_auth_scheme_handler_with_scheme_and_configureOptions()
        {
            var schemeName = "CustomScheme";
            var scheme = await GetSchemeAsync(a => a.AddApiKeyInAuthorizationHeader<MockApiKeyProvider>(schemeName, _ => { }), schemeName);
            Assert.NotNull(scheme);
            Assert.Equal(typeof(ApiKeyInAuthorizationHeaderHandler).Name, scheme.HandlerType.Name);
            Assert.Null(scheme.DisplayName);
            Assert.Equal(schemeName, scheme.Name);
        }

        [Fact]
        public async Task AddApiKeyInAuthorizationHeader_TApiKeyProvider_verify_auth_scheme_handler_with_scheme_displayName_and_configureOptions()
        {
            var schemeName = "CustomScheme";
            var displayName = "DisplayName";
            var scheme = await GetSchemeAsync(a => a.AddApiKeyInAuthorizationHeader<MockApiKeyProvider>(schemeName, displayName, _ => { }), schemeName);
            Assert.NotNull(scheme);
            Assert.Equal(typeof(ApiKeyInAuthorizationHeaderHandler).Name, scheme.HandlerType.Name);
            Assert.NotNull(scheme.DisplayName);
            Assert.Equal(displayName, scheme.DisplayName);
            Assert.Equal(schemeName, scheme.Name);
        }

        #endregion  // Verify Auth Scheme

        #region Allows Multiple Schemes

        [Fact]
        public async Task AddApiKeyInAuthorizationHeader_allows_multiple_schemes()
        {
            var schemeName = "CustomScheme";
            var displayName = "DisplayName";

            var services = new ServiceCollection();
            services.AddAuthentication()
                .AddApiKeyInAuthorizationHeader()
                .AddApiKeyInAuthorizationHeader(schemeName, displayName, _ => { });

            var sp = services.BuildServiceProvider();
            var schemeProvider = sp.GetRequiredService<IAuthenticationSchemeProvider>();
            var defaultScheme = await schemeProvider.GetSchemeAsync(ApiKeyDefaults.AuthenticationScheme);
            var scheme = await schemeProvider.GetSchemeAsync(schemeName);

            Assert.NotNull(defaultScheme);
            Assert.Equal(typeof(ApiKeyInAuthorizationHeaderHandler).Name, defaultScheme.HandlerType.Name);
            Assert.Null(defaultScheme.DisplayName);
            Assert.Equal(ApiKeyDefaults.AuthenticationScheme, defaultScheme.Name);

            Assert.NotNull(scheme);
            Assert.Equal(typeof(ApiKeyInAuthorizationHeaderHandler).Name, scheme.HandlerType.Name);
            Assert.NotNull(scheme.DisplayName);
            Assert.Equal(displayName, scheme.DisplayName);
            Assert.Equal(schemeName, scheme.Name);
        }

        [Fact]
        public async Task AddApiKeyInAuthorizationHeader_TApiKeyProvider_allows_multiple_schemes()
        {
            var schemeName = "CustomScheme";
            var displayName = "DisplayName";

            var services = new ServiceCollection();
            services.AddAuthentication()
                .AddApiKeyInAuthorizationHeader<MockApiKeyProvider>()
                .AddApiKeyInAuthorizationHeader<MockApiKeyProvider>(schemeName, displayName, _ => { });

            var sp = services.BuildServiceProvider();
            var schemeProvider = sp.GetRequiredService<IAuthenticationSchemeProvider>();
            var defaultScheme = await schemeProvider.GetSchemeAsync(ApiKeyDefaults.AuthenticationScheme);
            var scheme = await schemeProvider.GetSchemeAsync(schemeName);

            Assert.NotNull(defaultScheme);
            Assert.Equal(typeof(ApiKeyInAuthorizationHeaderHandler).Name, defaultScheme.HandlerType.Name);
            Assert.Null(defaultScheme.DisplayName);
            Assert.Equal(ApiKeyDefaults.AuthenticationScheme, defaultScheme.Name);

            Assert.NotNull(scheme);
            Assert.Equal(typeof(ApiKeyInAuthorizationHeaderHandler).Name, scheme.HandlerType.Name);
            Assert.NotNull(scheme.DisplayName);
            Assert.Equal(displayName, scheme.DisplayName);
            Assert.Equal(schemeName, scheme.Name);
        }

        #endregion  // Allows Multiple Schemes

        #region TApiKeyProvider tests

        [Fact]
        public void AddApiKeyInAuthorizationHeader_TApiKeyProvider_IApiKeyProvider_is_registered_as_transient()
        {
            var services = new ServiceCollection();
            services.AddAuthentication()
                .AddApiKeyInAuthorizationHeader<MockApiKeyProvider>();

            var serviceDescriptor = Assert.Single(services, s => s.ServiceType == typeof(IApiKeyProvider));
            Assert.Equal(typeof(IApiKeyProvider), serviceDescriptor.ServiceType);
            Assert.Equal(typeof(MockApiKeyProvider), serviceDescriptor.ImplementationType);
            Assert.Equal(ServiceLifetime.Transient, serviceDescriptor.Lifetime);

            var sp = services.BuildServiceProvider();
            var provider = sp.GetService<IApiKeyProvider>();

            Assert.NotNull(provider);
            Assert.Equal(typeof(MockApiKeyProvider), provider.GetType());
        }

        [Fact]
        public void AddApiKeyInAuthorizationHeader_TApiKeyProvider_does_not_replace_previously_user_registered_IApiKeyProvider()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IApiKeyProvider, MockApiKeyProvider2>();
            services.AddAuthentication()
                .AddApiKeyInAuthorizationHeader<MockApiKeyProvider>();

            var serviceDescriptors = services.Where(s => s.ServiceType == typeof(IApiKeyProvider));
            Assert.Equal(2, serviceDescriptors.Count());

            var serviceDescriptor = Assert.Single(serviceDescriptors, s => s.ImplementationType == typeof(MockApiKeyProvider));
            Assert.Equal(typeof(IApiKeyProvider), serviceDescriptor.ServiceType);
            Assert.Equal(typeof(MockApiKeyProvider), serviceDescriptor.ImplementationType);
            Assert.Equal(ServiceLifetime.Transient, serviceDescriptor.Lifetime);

            serviceDescriptor = Assert.Single(serviceDescriptors, s => s.ImplementationType == typeof(MockApiKeyProvider2));
            Assert.Equal(typeof(IApiKeyProvider), serviceDescriptor.ServiceType);
            Assert.Equal(typeof(MockApiKeyProvider2), serviceDescriptor.ImplementationType);
            Assert.Equal(ServiceLifetime.Singleton, serviceDescriptor.Lifetime);
        }

        #endregion  // TApiKeyProvider tests

        #region Allows chaining

        [Fact]
        public void AddApiKeyInAuthorizationHeader_allows_chaining_default()
        {
            var authenticationBuilder = new ServiceCollection().AddAuthentication();
            Assert.Same(authenticationBuilder, authenticationBuilder.AddApiKeyInAuthorizationHeader());
        }

        [Fact]
        public void AddApiKeyInAuthorizationHeader_allows_chaining_with_scheme()
        {
            var authenticationBuilder = new ServiceCollection().AddAuthentication();
            Assert.Same(authenticationBuilder, authenticationBuilder.AddApiKeyInAuthorizationHeader(string.Empty));
        }

        [Fact]
        public void AddApiKeyInAuthorizationHeader_allows_chaining_with_configureOptions()
        {
            var authenticationBuilder = new ServiceCollection().AddAuthentication();
            Assert.Same(authenticationBuilder, authenticationBuilder.AddApiKeyInAuthorizationHeader(_ => { }));
        }

        [Fact]
        public void AddApiKeyInAuthorizationHeader_allows_chaining_with_scheme_and_configureOptions()
        {
            var authenticationBuilder = new ServiceCollection().AddAuthentication();
            Assert.Same(authenticationBuilder, authenticationBuilder.AddApiKeyInAuthorizationHeader(string.Empty, _ => { }));
        }

        [Fact]
        public void AddApiKeyInAuthorizationHeader_allows_chaining_with_scheme_displayName_and_configureOptions()
        {
            var authenticationBuilder = new ServiceCollection().AddAuthentication();
            Assert.Same(authenticationBuilder, authenticationBuilder.AddApiKeyInAuthorizationHeader(string.Empty, string.Empty, _ => { }));
        }


        [Fact]
        public void AddApiKeyInAuthorizationHeader_TApiKeyProvider_allows_chaining()
        {
            var authenticationBuilder = new ServiceCollection().AddAuthentication();
            Assert.Same(authenticationBuilder, authenticationBuilder.AddApiKeyInAuthorizationHeader<MockApiKeyProvider>());
        }

        [Fact]
        public void AddApiKeyInAuthorizationHeader_TApiKeyProvider_allows_chaining_with_scheme()
        {
            var authenticationBuilder = new ServiceCollection().AddAuthentication();
            Assert.Same(authenticationBuilder, authenticationBuilder.AddApiKeyInAuthorizationHeader<MockApiKeyProvider>(string.Empty));
        }

        [Fact]
        public void AddApiKeyInAuthorizationHeader_TApiKeyProvider_allows_chaining_with_configureOptions()
        {
            var authenticationBuilder = new ServiceCollection().AddAuthentication();
            Assert.Same(authenticationBuilder, authenticationBuilder.AddApiKeyInAuthorizationHeader<MockApiKeyProvider>(_ => { }));
        }

        [Fact]
        public void AddApiKeyInAuthorizationHeader_TApiKeyProvider_allows_chaining_with_scheme_and_configureOptions()
        {
            var authenticationBuilder = new ServiceCollection().AddAuthentication();
            Assert.Same(authenticationBuilder, authenticationBuilder.AddApiKeyInAuthorizationHeader<MockApiKeyProvider>(string.Empty, _ => { }));
        }

        [Fact]
        public void AddApiKeyInAuthorizationHeader_TApiKeyProvider_allows_chaining_with_scheme_displayName_and_configureOptions()
        {
            var authenticationBuilder = new ServiceCollection().AddAuthentication();
            Assert.Same(authenticationBuilder, authenticationBuilder.AddApiKeyInAuthorizationHeader<MockApiKeyProvider>(string.Empty, string.Empty, _ => { }));
        }

        #endregion // Allows chaining

        #endregion // API Key - In Authorization Header

        #region API Key - In Query Parameters

        #region Verify Auth Scheme

        [Fact]
        public async Task AddApiKeyInQueryParams_verify_auth_scheme_handler_default()
        {
            var scheme = await GetSchemeAsync(a => a.AddApiKeyInQueryParams());
            Assert.NotNull(scheme);
            Assert.Equal(typeof(ApiKeyInQueryParamsHandler).Name, scheme.HandlerType.Name);
            Assert.Null(scheme.DisplayName);
        }

        [Fact]
        public async Task AddApiKeyInQueryParams_verify_auth_scheme_handler_with_scheme()
        {
            var schemeName = "CustomScheme";
            var scheme = await GetSchemeAsync(a => a.AddApiKeyInQueryParams(schemeName), schemeName);
            Assert.NotNull(scheme);
            Assert.Equal(typeof(ApiKeyInQueryParamsHandler).Name, scheme.HandlerType.Name);
            Assert.Null(scheme.DisplayName);
            Assert.Equal(schemeName, scheme.Name);
        }

        [Fact]
        public async Task AddApiKeyInQueryParams_verify_auth_scheme_handler_with_configureOptions()
        {
            var scheme = await GetSchemeAsync(a => a.AddApiKeyInQueryParams(_ => { }));
            Assert.NotNull(scheme);
            Assert.Equal(typeof(ApiKeyInQueryParamsHandler).Name, scheme.HandlerType.Name);
            Assert.Null(scheme.DisplayName);
        }

        [Fact]
        public async Task AddApiKeyInQueryParams_verify_auth_scheme_handler_with_scheme_and_configureOptions()
        {
            var schemeName = "CustomScheme";
            var scheme = await GetSchemeAsync(a => a.AddApiKeyInQueryParams(schemeName, _ => { }), schemeName);
            Assert.NotNull(scheme);
            Assert.Equal(typeof(ApiKeyInQueryParamsHandler).Name, scheme.HandlerType.Name);
            Assert.Null(scheme.DisplayName);
            Assert.Equal(schemeName, scheme.Name);
        }

        [Fact]
        public async Task AddApiKeyInQueryParams_verify_auth_scheme_handler_with_scheme_displayName_and_configureOptions()
        {
            var schemeName = "CustomScheme";
            var displayName = "DisplayName";
            var scheme = await GetSchemeAsync(a => a.AddApiKeyInQueryParams(schemeName, displayName, _ => { }), schemeName);
            Assert.NotNull(scheme);
            Assert.Equal(typeof(ApiKeyInQueryParamsHandler).Name, scheme.HandlerType.Name);
            Assert.NotNull(scheme.DisplayName);
            Assert.Equal(displayName, scheme.DisplayName);
            Assert.Equal(schemeName, scheme.Name);
        }


        [Fact]
        public async Task AddApiKeyInQueryParams_TApiKeyProvider_verify_auth_scheme_handler_default()
        {
            var scheme = await GetSchemeAsync(a => a.AddApiKeyInQueryParams<MockApiKeyProvider>(), ApiKeyDefaults.AuthenticationScheme);
            Assert.NotNull(scheme);
            Assert.Equal(typeof(ApiKeyInQueryParamsHandler).Name, scheme.HandlerType.Name);
            Assert.Null(scheme.DisplayName);
        }

        [Fact]
        public async Task AddApiKeyInQueryParams_TApiKeyProvider_verify_auth_scheme_handler_with_scheme()
        {
            var schemeName = "CustomScheme";
            var scheme = await GetSchemeAsync(a => a.AddApiKeyInQueryParams<MockApiKeyProvider>(schemeName), schemeName);
            Assert.NotNull(scheme);
            Assert.Equal(typeof(ApiKeyInQueryParamsHandler).Name, scheme.HandlerType.Name);
            Assert.Null(scheme.DisplayName);
            Assert.Equal(schemeName, scheme.Name);
        }

        [Fact]
        public async Task AddApiKeyInQueryParams_TApiKeyProvider_verify_auth_scheme_handler_with_configureOptions()
        {
            var scheme = await GetSchemeAsync(a => a.AddApiKeyInQueryParams<MockApiKeyProvider>(_ => { }));
            Assert.NotNull(scheme);
            Assert.Equal(typeof(ApiKeyInQueryParamsHandler).Name, scheme.HandlerType.Name);
            Assert.Null(scheme.DisplayName);
        }

        [Fact]
        public async Task AddApiKeyInQueryParams_TApiKeyProvider_verify_auth_scheme_handler_with_scheme_and_configureOptions()
        {
            var schemeName = "CustomScheme";
            var scheme = await GetSchemeAsync(a => a.AddApiKeyInQueryParams<MockApiKeyProvider>(schemeName, _ => { }), schemeName);
            Assert.NotNull(scheme);
            Assert.Equal(typeof(ApiKeyInQueryParamsHandler).Name, scheme.HandlerType.Name);
            Assert.Null(scheme.DisplayName);
            Assert.Equal(schemeName, scheme.Name);
        }

        [Fact]
        public async Task AddApiKeyInQueryParams_TApiKeyProvider_verify_auth_scheme_handler_with_scheme_displayName_and_configureOptions()
        {
            var schemeName = "CustomScheme";
            var displayName = "DisplayName";
            var scheme = await GetSchemeAsync(a => a.AddApiKeyInQueryParams<MockApiKeyProvider>(schemeName, displayName, _ => { }), schemeName);
            Assert.NotNull(scheme);
            Assert.Equal(typeof(ApiKeyInQueryParamsHandler).Name, scheme.HandlerType.Name);
            Assert.NotNull(scheme.DisplayName);
            Assert.Equal(displayName, scheme.DisplayName);
            Assert.Equal(schemeName, scheme.Name);
        }

        #endregion  // Verify Auth Scheme

        #region Allows Multiple Schemes

        [Fact]
        public async Task AddApiKeyInQueryParams_allows_multiple_schemes()
        {
            var schemeName = "CustomScheme";
            var displayName = "DisplayName";

            var services = new ServiceCollection();
            services.AddAuthentication()
                .AddApiKeyInQueryParams()
                .AddApiKeyInQueryParams(schemeName, displayName, _ => { });

            var sp = services.BuildServiceProvider();
            var schemeProvider = sp.GetRequiredService<IAuthenticationSchemeProvider>();
            var defaultScheme = await schemeProvider.GetSchemeAsync(ApiKeyDefaults.AuthenticationScheme);
            var scheme = await schemeProvider.GetSchemeAsync(schemeName);

            Assert.NotNull(defaultScheme);
            Assert.Equal(typeof(ApiKeyInQueryParamsHandler).Name, defaultScheme.HandlerType.Name);
            Assert.Null(defaultScheme.DisplayName);
            Assert.Equal(ApiKeyDefaults.AuthenticationScheme, defaultScheme.Name);

            Assert.NotNull(scheme);
            Assert.Equal(typeof(ApiKeyInQueryParamsHandler).Name, scheme.HandlerType.Name);
            Assert.NotNull(scheme.DisplayName);
            Assert.Equal(displayName, scheme.DisplayName);
            Assert.Equal(schemeName, scheme.Name);
        }

        [Fact]
        public async Task AddApiKeyInQueryParams_TApiKeyProvider_allows_multiple_schemes()
        {
            var schemeName = "CustomScheme";
            var displayName = "DisplayName";

            var services = new ServiceCollection();
            services.AddAuthentication()
                .AddApiKeyInQueryParams<MockApiKeyProvider>()
                .AddApiKeyInQueryParams<MockApiKeyProvider>(schemeName, displayName, _ => { });

            var sp = services.BuildServiceProvider();
            var schemeProvider = sp.GetRequiredService<IAuthenticationSchemeProvider>();
            var defaultScheme = await schemeProvider.GetSchemeAsync(ApiKeyDefaults.AuthenticationScheme);
            var scheme = await schemeProvider.GetSchemeAsync(schemeName);

            Assert.NotNull(defaultScheme);
            Assert.Equal(typeof(ApiKeyInQueryParamsHandler).Name, defaultScheme.HandlerType.Name);
            Assert.Null(defaultScheme.DisplayName);
            Assert.Equal(ApiKeyDefaults.AuthenticationScheme, defaultScheme.Name);

            Assert.NotNull(scheme);
            Assert.Equal(typeof(ApiKeyInQueryParamsHandler).Name, scheme.HandlerType.Name);
            Assert.NotNull(scheme.DisplayName);
            Assert.Equal(displayName, scheme.DisplayName);
            Assert.Equal(schemeName, scheme.Name);
        }

        #endregion  // Allows Multiple Schemes

        #region TApiKeyProvider tests

        [Fact]
        public void AddApiKeyInQueryParams_TApiKeyProvider_IApiKeyProvider_is_registered_as_transient()
        {
            var services = new ServiceCollection();
            services.AddAuthentication()
                .AddApiKeyInQueryParams<MockApiKeyProvider>();

            var serviceDescriptor = Assert.Single(services, s => s.ServiceType == typeof(IApiKeyProvider));
            Assert.Equal(typeof(IApiKeyProvider), serviceDescriptor.ServiceType);
            Assert.Equal(typeof(MockApiKeyProvider), serviceDescriptor.ImplementationType);
            Assert.Equal(ServiceLifetime.Transient, serviceDescriptor.Lifetime);

            var sp = services.BuildServiceProvider();
            var provider = sp.GetService<IApiKeyProvider>();

            Assert.NotNull(provider);
            Assert.Equal(typeof(MockApiKeyProvider), provider.GetType());
        }

        [Fact]
        public void AddApiKeyInQueryParams_TApiKeyProvider_does_not_replace_previously_user_registered_IApiKeyProvider()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IApiKeyProvider, MockApiKeyProvider2>();
            services.AddAuthentication()
                .AddApiKeyInQueryParams<MockApiKeyProvider>();

            var serviceDescriptors = services.Where(s => s.ServiceType == typeof(IApiKeyProvider));
            Assert.Equal(2, serviceDescriptors.Count());

            var serviceDescriptor = Assert.Single(serviceDescriptors, s => s.ImplementationType == typeof(MockApiKeyProvider));
            Assert.Equal(typeof(IApiKeyProvider), serviceDescriptor.ServiceType);
            Assert.Equal(typeof(MockApiKeyProvider), serviceDescriptor.ImplementationType);
            Assert.Equal(ServiceLifetime.Transient, serviceDescriptor.Lifetime);

            serviceDescriptor = Assert.Single(serviceDescriptors, s => s.ImplementationType == typeof(MockApiKeyProvider2));
            Assert.Equal(typeof(IApiKeyProvider), serviceDescriptor.ServiceType);
            Assert.Equal(typeof(MockApiKeyProvider2), serviceDescriptor.ImplementationType);
            Assert.Equal(ServiceLifetime.Singleton, serviceDescriptor.Lifetime);
        }

        #endregion  // TApiKeyProvider tests

        #region Allows chaining

        [Fact]
        public void AddApiKeyInQueryParams_allows_chaining_default()
        {
            var authenticationBuilder = new ServiceCollection().AddAuthentication();
            Assert.Same(authenticationBuilder, authenticationBuilder.AddApiKeyInQueryParams());
        }

        [Fact]
        public void AddApiKeyInQueryParams_allows_chaining_with_scheme()
        {
            var authenticationBuilder = new ServiceCollection().AddAuthentication();
            Assert.Same(authenticationBuilder, authenticationBuilder.AddApiKeyInQueryParams(string.Empty));
        }

        [Fact]
        public void AddApiKeyInQueryParams_allows_chaining_with_configureOptions()
        {
            var authenticationBuilder = new ServiceCollection().AddAuthentication();
            Assert.Same(authenticationBuilder, authenticationBuilder.AddApiKeyInQueryParams(_ => { }));
        }

        [Fact]
        public void AddApiKeyInQueryParams_allows_chaining_with_scheme_and_configureOptions()
        {
            var authenticationBuilder = new ServiceCollection().AddAuthentication();
            Assert.Same(authenticationBuilder, authenticationBuilder.AddApiKeyInQueryParams(string.Empty, _ => { }));
        }

        [Fact]
        public void AddApiKeyInQueryParams_allows_chaining_with_scheme_displayName_and_configureOptions()
        {
            var authenticationBuilder = new ServiceCollection().AddAuthentication();
            Assert.Same(authenticationBuilder, authenticationBuilder.AddApiKeyInQueryParams(string.Empty, string.Empty, _ => { }));
        }


        [Fact]
        public void AddApiKeyInQueryParams_TApiKeyProvider_allows_chaining()
        {
            var authenticationBuilder = new ServiceCollection().AddAuthentication();
            Assert.Same(authenticationBuilder, authenticationBuilder.AddApiKeyInQueryParams<MockApiKeyProvider>());
        }

        [Fact]
        public void AddApiKeyInQueryParams_TApiKeyProvider_allows_chaining_with_scheme()
        {
            var authenticationBuilder = new ServiceCollection().AddAuthentication();
            Assert.Same(authenticationBuilder, authenticationBuilder.AddApiKeyInQueryParams<MockApiKeyProvider>(string.Empty));
        }

        [Fact]
        public void AddApiKeyInQueryParams_TApiKeyProvider_allows_chaining_with_configureOptions()
        {
            var authenticationBuilder = new ServiceCollection().AddAuthentication();
            Assert.Same(authenticationBuilder, authenticationBuilder.AddApiKeyInQueryParams<MockApiKeyProvider>(_ => { }));
        }

        [Fact]
        public void AddApiKeyInQueryParams_TApiKeyProvider_allows_chaining_with_scheme_and_configureOptions()
        {
            var authenticationBuilder = new ServiceCollection().AddAuthentication();
            Assert.Same(authenticationBuilder, authenticationBuilder.AddApiKeyInQueryParams<MockApiKeyProvider>(string.Empty, _ => { }));
        }

        [Fact]
        public void AddApiKeyInQueryParams_TApiKeyProvider_allows_chaining_with_scheme_displayName_and_configureOptions()
        {
            var authenticationBuilder = new ServiceCollection().AddAuthentication();
            Assert.Same(authenticationBuilder, authenticationBuilder.AddApiKeyInQueryParams<MockApiKeyProvider>(string.Empty, string.Empty, _ => { }));
        }

        #endregion // Allows chaining

        #endregion // API Key - In Query Parameters

        #region API Key - In Header Or Query Parameters

        #region Verify Auth Scheme

        [Fact]
        public async Task AddApiKeyInHeaderOrQueryParams_verify_auth_scheme_handler_default()
        {
            var scheme = await GetSchemeAsync(a => a.AddApiKeyInHeaderOrQueryParams());
            Assert.NotNull(scheme);
            Assert.Equal(typeof(ApiKeyInHeaderOrQueryParamsHandler).Name, scheme.HandlerType.Name);
            Assert.Null(scheme.DisplayName);
        }

        [Fact]
        public async Task AddApiKeyInHeaderOrQueryParams_verify_auth_scheme_handler_with_scheme()
        {
            var schemeName = "CustomScheme";
            var scheme = await GetSchemeAsync(a => a.AddApiKeyInHeaderOrQueryParams(schemeName), schemeName);
            Assert.NotNull(scheme);
            Assert.Equal(typeof(ApiKeyInHeaderOrQueryParamsHandler).Name, scheme.HandlerType.Name);
            Assert.Null(scheme.DisplayName);
            Assert.Equal(schemeName, scheme.Name);
        }

        [Fact]
        public async Task AddApiKeyInHeaderOrQueryParams_verify_auth_scheme_handler_with_configureOptions()
        {
            var scheme = await GetSchemeAsync(a => a.AddApiKeyInHeaderOrQueryParams(_ => { }));
            Assert.NotNull(scheme);
            Assert.Equal(typeof(ApiKeyInHeaderOrQueryParamsHandler).Name, scheme.HandlerType.Name);
            Assert.Null(scheme.DisplayName);
        }

        [Fact]
        public async Task AddApiKeyInHeaderOrQueryParams_verify_auth_scheme_handler_with_scheme_and_configureOptions()
        {
            var schemeName = "CustomScheme";
            var scheme = await GetSchemeAsync(a => a.AddApiKeyInHeaderOrQueryParams(schemeName, _ => { }), schemeName);
            Assert.NotNull(scheme);
            Assert.Equal(typeof(ApiKeyInHeaderOrQueryParamsHandler).Name, scheme.HandlerType.Name);
            Assert.Null(scheme.DisplayName);
            Assert.Equal(schemeName, scheme.Name);
        }

        [Fact]
        public async Task AddApiKeyInHeaderOrQueryParams_verify_auth_scheme_handler_with_scheme_displayName_and_configureOptions()
        {
            var schemeName = "CustomScheme";
            var displayName = "DisplayName";
            var scheme = await GetSchemeAsync(a => a.AddApiKeyInHeaderOrQueryParams(schemeName, displayName, _ => { }), schemeName);
            Assert.NotNull(scheme);
            Assert.Equal(typeof(ApiKeyInHeaderOrQueryParamsHandler).Name, scheme.HandlerType.Name);
            Assert.NotNull(scheme.DisplayName);
            Assert.Equal(displayName, scheme.DisplayName);
            Assert.Equal(schemeName, scheme.Name);
        }


        [Fact]
        public async Task AddApiKeyInHeaderOrQueryParams_TApiKeyProvider_verify_auth_scheme_handler_default()
        {
            var scheme = await GetSchemeAsync(a => a.AddApiKeyInHeaderOrQueryParams<MockApiKeyProvider>(), ApiKeyDefaults.AuthenticationScheme);
            Assert.NotNull(scheme);
            Assert.Equal(typeof(ApiKeyInHeaderOrQueryParamsHandler).Name, scheme.HandlerType.Name);
            Assert.Null(scheme.DisplayName);
        }

        [Fact]
        public async Task AddApiKeyInHeaderOrQueryParams_TApiKeyProvider_verify_auth_scheme_handler_with_scheme()
        {
            var schemeName = "CustomScheme";
            var scheme = await GetSchemeAsync(a => a.AddApiKeyInHeaderOrQueryParams<MockApiKeyProvider>(schemeName), schemeName);
            Assert.NotNull(scheme);
            Assert.Equal(typeof(ApiKeyInHeaderOrQueryParamsHandler).Name, scheme.HandlerType.Name);
            Assert.Null(scheme.DisplayName);
            Assert.Equal(schemeName, scheme.Name);
        }

        [Fact]
        public async Task AddApiKeyInHeaderOrQueryParams_TApiKeyProvider_verify_auth_scheme_handler_with_configureOptions()
        {
            var scheme = await GetSchemeAsync(a => a.AddApiKeyInHeaderOrQueryParams<MockApiKeyProvider>(_ => { }));
            Assert.NotNull(scheme);
            Assert.Equal(typeof(ApiKeyInHeaderOrQueryParamsHandler).Name, scheme.HandlerType.Name);
            Assert.Null(scheme.DisplayName);
        }

        [Fact]
        public async Task AddApiKeyInHeaderOrQueryParams_TApiKeyProvider_verify_auth_scheme_handler_with_scheme_and_configureOptions()
        {
            var schemeName = "CustomScheme";
            var scheme = await GetSchemeAsync(a => a.AddApiKeyInHeaderOrQueryParams<MockApiKeyProvider>(schemeName, _ => { }), schemeName);
            Assert.NotNull(scheme);
            Assert.Equal(typeof(ApiKeyInHeaderOrQueryParamsHandler).Name, scheme.HandlerType.Name);
            Assert.Null(scheme.DisplayName);
            Assert.Equal(schemeName, scheme.Name);
        }

        [Fact]
        public async Task AddApiKeyInHeaderOrQueryParams_TApiKeyProvider_verify_auth_scheme_handler_with_scheme_displayName_and_configureOptions()
        {
            var schemeName = "CustomScheme";
            var displayName = "DisplayName";
            var scheme = await GetSchemeAsync(a => a.AddApiKeyInHeaderOrQueryParams<MockApiKeyProvider>(schemeName, displayName, _ => { }), schemeName);
            Assert.NotNull(scheme);
            Assert.Equal(typeof(ApiKeyInHeaderOrQueryParamsHandler).Name, scheme.HandlerType.Name);
            Assert.NotNull(scheme.DisplayName);
            Assert.Equal(displayName, scheme.DisplayName);
            Assert.Equal(schemeName, scheme.Name);
        }

        #endregion  // Verify Auth Scheme

        #region Allows Multiple Schemes

        [Fact]
        public async Task AddApiKeyInHeaderOrQueryParams_allows_multiple_schemes()
        {
            var schemeName = "CustomScheme";
            var displayName = "DisplayName";

            var services = new ServiceCollection();
            services.AddAuthentication()
                .AddApiKeyInHeaderOrQueryParams()
                .AddApiKeyInHeaderOrQueryParams(schemeName, displayName, _ => { });

            var sp = services.BuildServiceProvider();
            var schemeProvider = sp.GetRequiredService<IAuthenticationSchemeProvider>();
            var defaultScheme = await schemeProvider.GetSchemeAsync(ApiKeyDefaults.AuthenticationScheme);
            var scheme = await schemeProvider.GetSchemeAsync(schemeName);

            Assert.NotNull(defaultScheme);
            Assert.Equal(typeof(ApiKeyInHeaderOrQueryParamsHandler).Name, defaultScheme.HandlerType.Name);
            Assert.Null(defaultScheme.DisplayName);
            Assert.Equal(ApiKeyDefaults.AuthenticationScheme, defaultScheme.Name);

            Assert.NotNull(scheme);
            Assert.Equal(typeof(ApiKeyInHeaderOrQueryParamsHandler).Name, scheme.HandlerType.Name);
            Assert.NotNull(scheme.DisplayName);
            Assert.Equal(displayName, scheme.DisplayName);
            Assert.Equal(schemeName, scheme.Name);
        }

        [Fact]
        public async Task AddApiKeyInHeaderOrQueryParams_TApiKeyProvider_allows_multiple_schemes()
        {
            var schemeName = "CustomScheme";
            var displayName = "DisplayName";

            var services = new ServiceCollection();
            services.AddAuthentication()
                .AddApiKeyInHeaderOrQueryParams<MockApiKeyProvider>()
                .AddApiKeyInHeaderOrQueryParams<MockApiKeyProvider>(schemeName, displayName, _ => { });

            var sp = services.BuildServiceProvider();
            var schemeProvider = sp.GetRequiredService<IAuthenticationSchemeProvider>();
            var defaultScheme = await schemeProvider.GetSchemeAsync(ApiKeyDefaults.AuthenticationScheme);
            var scheme = await schemeProvider.GetSchemeAsync(schemeName);

            Assert.NotNull(defaultScheme);
            Assert.Equal(typeof(ApiKeyInHeaderOrQueryParamsHandler).Name, defaultScheme.HandlerType.Name);
            Assert.Null(defaultScheme.DisplayName);
            Assert.Equal(ApiKeyDefaults.AuthenticationScheme, defaultScheme.Name);

            Assert.NotNull(scheme);
            Assert.Equal(typeof(ApiKeyInHeaderOrQueryParamsHandler).Name, scheme.HandlerType.Name);
            Assert.NotNull(scheme.DisplayName);
            Assert.Equal(displayName, scheme.DisplayName);
            Assert.Equal(schemeName, scheme.Name);
        }

        #endregion  // Allows Multiple Schemes

        #region TApiKeyProvider tests

        [Fact]
        public void AddApiKeyInHeaderOrQueryParams_TApiKeyProvider_IApiKeyProvider_is_registered_as_transient()
        {
            var services = new ServiceCollection();
            services.AddAuthentication()
                .AddApiKeyInHeaderOrQueryParams<MockApiKeyProvider>();

            var serviceDescriptor = Assert.Single(services, s => s.ServiceType == typeof(IApiKeyProvider));
            Assert.Equal(typeof(IApiKeyProvider), serviceDescriptor.ServiceType);
            Assert.Equal(typeof(MockApiKeyProvider), serviceDescriptor.ImplementationType);
            Assert.Equal(ServiceLifetime.Transient, serviceDescriptor.Lifetime);

            var sp = services.BuildServiceProvider();
            var provider = sp.GetService<IApiKeyProvider>();

            Assert.NotNull(provider);
            Assert.Equal(typeof(MockApiKeyProvider), provider.GetType());
        }

        [Fact]
        public void AddApiKeyInHeaderOrQueryParams_TApiKeyProvider_does_not_replace_previously_user_registered_IApiKeyProvider()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IApiKeyProvider, MockApiKeyProvider2>();
            services.AddAuthentication()
                .AddApiKeyInHeaderOrQueryParams<MockApiKeyProvider>();

            var serviceDescriptors = services.Where(s => s.ServiceType == typeof(IApiKeyProvider));
            Assert.Equal(2, serviceDescriptors.Count());

            var serviceDescriptor = Assert.Single(serviceDescriptors, s => s.ImplementationType == typeof(MockApiKeyProvider));
            Assert.Equal(typeof(IApiKeyProvider), serviceDescriptor.ServiceType);
            Assert.Equal(typeof(MockApiKeyProvider), serviceDescriptor.ImplementationType);
            Assert.Equal(ServiceLifetime.Transient, serviceDescriptor.Lifetime);

            serviceDescriptor = Assert.Single(serviceDescriptors, s => s.ImplementationType == typeof(MockApiKeyProvider2));
            Assert.Equal(typeof(IApiKeyProvider), serviceDescriptor.ServiceType);
            Assert.Equal(typeof(MockApiKeyProvider2), serviceDescriptor.ImplementationType);
            Assert.Equal(ServiceLifetime.Singleton, serviceDescriptor.Lifetime);
        }

        #endregion  // TApiKeyProvider tests

        #region Allows chaining

        [Fact]
        public void AddApiKeyInHeaderOrQueryParams_allows_chaining_default()
        {
            var authenticationBuilder = new ServiceCollection().AddAuthentication();
            Assert.Same(authenticationBuilder, authenticationBuilder.AddApiKeyInHeaderOrQueryParams());
        }

        [Fact]
        public void AddApiKeyInHeaderOrQueryParams_allows_chaining_with_scheme()
        {
            var authenticationBuilder = new ServiceCollection().AddAuthentication();
            Assert.Same(authenticationBuilder, authenticationBuilder.AddApiKeyInHeaderOrQueryParams(string.Empty));
        }

        [Fact]
        public void AddApiKeyInHeaderOrQueryParams_allows_chaining_with_configureOptions()
        {
            var authenticationBuilder = new ServiceCollection().AddAuthentication();
            Assert.Same(authenticationBuilder, authenticationBuilder.AddApiKeyInHeaderOrQueryParams(_ => { }));
        }

        [Fact]
        public void AddApiKeyInHeaderOrQueryParams_allows_chaining_with_scheme_and_configureOptions()
        {
            var authenticationBuilder = new ServiceCollection().AddAuthentication();
            Assert.Same(authenticationBuilder, authenticationBuilder.AddApiKeyInHeaderOrQueryParams(string.Empty, _ => { }));
        }

        [Fact]
        public void AddApiKeyInHeaderOrQueryParams_allows_chaining_with_scheme_displayName_and_configureOptions()
        {
            var authenticationBuilder = new ServiceCollection().AddAuthentication();
            Assert.Same(authenticationBuilder, authenticationBuilder.AddApiKeyInHeaderOrQueryParams(string.Empty, string.Empty, _ => { }));
        }


        [Fact]
        public void AddApiKeyInHeaderOrQueryParams_TApiKeyProvider_allows_chaining()
        {
            var authenticationBuilder = new ServiceCollection().AddAuthentication();
            Assert.Same(authenticationBuilder, authenticationBuilder.AddApiKeyInHeaderOrQueryParams<MockApiKeyProvider>());
        }

        [Fact]
        public void AddApiKeyInHeaderOrQueryParams_TApiKeyProvider_allows_chaining_with_scheme()
        {
            var authenticationBuilder = new ServiceCollection().AddAuthentication();
            Assert.Same(authenticationBuilder, authenticationBuilder.AddApiKeyInHeaderOrQueryParams<MockApiKeyProvider>(string.Empty));
        }

        [Fact]
        public void AddApiKeyInHeaderOrQueryParams_TApiKeyProvider_allows_chaining_with_configureOptions()
        {
            var authenticationBuilder = new ServiceCollection().AddAuthentication();
            Assert.Same(authenticationBuilder, authenticationBuilder.AddApiKeyInHeaderOrQueryParams<MockApiKeyProvider>(_ => { }));
        }

        [Fact]
        public void AddApiKeyInHeaderOrQueryParams_TApiKeyProvider_allows_chaining_with_scheme_and_configureOptions()
        {
            var authenticationBuilder = new ServiceCollection().AddAuthentication();
            Assert.Same(authenticationBuilder, authenticationBuilder.AddApiKeyInHeaderOrQueryParams<MockApiKeyProvider>(string.Empty, _ => { }));
        }

        [Fact]
        public void AddApiKeyInHeaderOrQueryParams_TApiKeyProvider_allows_chaining_with_scheme_displayName_and_configureOptions()
        {
            var authenticationBuilder = new ServiceCollection().AddAuthentication();
            Assert.Same(authenticationBuilder, authenticationBuilder.AddApiKeyInHeaderOrQueryParams<MockApiKeyProvider>(string.Empty, string.Empty, _ => { }));
        }

        #endregion // Allows chaining

        #endregion // API Key - In Header Or Query Parameters

        private static Task<AuthenticationScheme?> GetSchemeAsync(Action<AuthenticationBuilder> authenticationBuilderAction, string schemeName = ApiKeyDefaults.AuthenticationScheme)
        {
            var services = new ServiceCollection();
            authenticationBuilderAction(services.AddAuthentication());
            var sp = services.BuildServiceProvider();
            var schemeProvider = sp.GetRequiredService<IAuthenticationSchemeProvider>();
            return schemeProvider.GetSchemeAsync(schemeName);
        }

        private class MockApiKeyProvider : IApiKeyProvider
        {
            public Task<IApiKey?> ProvideAsync(string key)
            {
                throw new NotImplementedException();
            }
        }

        private class MockApiKeyProvider2 : IApiKeyProvider
        {
            public Task<IApiKey?> ProvideAsync(string key)
            {
                throw new NotImplementedException();
            }
        }
    }
}
