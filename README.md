# AspNetCore.Authentication.ApiKey
Easy to use and very light weight Microsoft style API Key Authentication Implementation for ASP.NET Core. It can be setup so that it can accept API Key either in Header, Authorization Header, QueryParams, HeaderOrQueryParams or RouteValues.

[View On GitHub](https://github.com/mihirdilip/aspnetcore-authentication-apikey)

<br/>

## .NET (Core) Frameworks Supported  
.NET Framework 4.6.1 and/or NetStandard 2.0 onwards  
Multi targeted: net8.0; net7.0; net6.0; net5.0; netcoreapp3.1; netcoreapp3.0; netstandard2.0; net461

<br/> 

## Installing
This library is published on NuGet. So the NuGet package can be installed directly to your project if you wish to use it without making any custom changes to the code.

Download directly from [AspNetCore.Authentication.ApiKey](https://www.nuget.org/packages/AspNetCore.Authentication.ApiKey).

Or by running the below command on your project.

```
PM> Install-Package AspNetCore.Authentication.ApiKey
```

<br/>

## Example Usage

Samples are available under [samples directory](samples).

Setting it up is quite simple. You will need basic working knowledge of ASP.NET Core 2.0 or newer to get started using this library.

There are 2 different ways of using this library to do it's job. Both ways can be mixed if required.  
1] Using the implementation of *IApiKeyProvider*  
2] Using *ApiKeyOptions.Events* (OnValidateKey delegate) which is same approach you will find on Microsoft's authentication libraries

Notes:
- It requires Realm to be set in the options if SuppressWWWAuthenticateHeader is not set.
- If an implementation of IApiKeyProvider interface is used as well as options.Events.OnValidateKey delegate is also set then this delegate will be used first.

**Always use HTTPS (SSL Certificate) protocol in production when using API Key authentication.**

#### Startup.cs (ASP.NET Core 3.0 onwards)

```C#
using AspNetCore.Authentication.ApiKey;
public class Startup
{
	public void ConfigureServices(IServiceCollection services)
	{
		// It requires Realm to be set in the options if SuppressWWWAuthenticateHeader is not set.
		// If an implementation of IApiKeyProvider interface is used as well as options.Events.OnValidateKey delegate is also set then this delegate will be used first.

		services.AddAuthentication(ApiKeyDefaults.AuthenticationScheme)

			// The below AddApiKeyInHeaderOrQueryParams without type parameter will require options.Events.OnValidateKey delegete to be set.
			//.AddApiKeyInHeaderOrQueryParams(options =>

			// The below AddApiKeyInHeaderOrQueryParams with type parameter will add the ApiKeyProvider to the dependency container. 
			.AddApiKeyInHeaderOrQueryParams<ApiKeyProvider>(options =>
			{
				options.Realm = "Sample Web API";
				options.KeyName = "X-API-KEY";
			});

		services.AddControllers();

		//// By default, authentication is not challenged for every request which is ASP.NET Core's default intended behaviour.
		//// So to challenge authentication for every requests please use below FallbackPolicy option.
		//services.AddAuthorization(options =>
		//{
		//	options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
		//});
	}

	public void Configure(IApplicationBuilder app, IHostingEnvironment env)
	{
		app.UseHttpsRedirection();

		// The below order of pipeline chain is important!
		app.UseRouting();

		app.UseAuthentication();
		app.UseAuthorization();

		app.UseEndpoints(endpoints =>
		{
			endpoints.MapControllers();
		});
	}
}
```

#### Startup.cs (ASP.NET Core 2.0 onwards)

```C#
using AspNetCore.Authentication.ApiKey;
public class Startup
{
	public void ConfigureServices(IServiceCollection services)
	{
		// It requires Realm to be set in the options if SuppressWWWAuthenticateHeader is not set.
		// If an implementation of IApiKeyProvider interface is used as well as options.Events.OnValidateKey delegate is also set then this delegate will be used first.

		services.AddAuthentication(ApiKeyDefaults.AuthenticationScheme)

			// The below AddApiKeyInHeaderOrQueryParams without type parameter will require options.Events.OnValidateKey delegete to be set.
			//.AddApiKeyInHeaderOrQueryParams(options =>

			// The below AddApiKeyInHeaderOrQueryParams with type parameter will add the ApiKeyProvider to the dependency container. 
			.AddApiKeyInHeaderOrQueryParams<ApiKeyProvider>(options =>
			{
				options.Realm = "Sample Web API";
				options.KeyName = "X-API-KEY";
			});


		services.AddMvc();

		//// By default, authentication is not challenged for every request which is ASP.NET Core's default intended behaviour.
		//// So to challenge authentication for every requests please use below option instead of above services.AddMvc().
		//services.AddMvc(options => 
		//{
		//	options.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()));
		//});
	}

	public void Configure(IApplicationBuilder app, IHostingEnvironment env)
	{
		app.UseAuthentication();
		app.UseMvc();
	}
}
```



#### ApiKeyProvider.cs
```C#
using AspNetCore.Authentication.ApiKey;
public class ApiKeyProvider : IApiKeyProvider
{
	private readonly ILogger<IApiKeyProvider> _logger;
	private readonly IApiKeyRepository _apiKeyRepository;
	
	public ApiKeyProvider(ILogger<IApiKeyProvider> logger, IApiKeyRepository apiKeyRepository)
	{
		_logger = logger;
		_apiKeyRepository = apiKeyRepository;
	}

	public async Task<IApiKey> ProvideAsync(string key)
	{
		try
		{
			// write your validation implementation here and return an instance of a valid ApiKey or retun null for an invalid key.
			// return await _apiKeyRepository.GetApiKeyAsync(key);
			return null;
		}
		catch (System.Exception exception)
		{
			_logger.LogError(exception, exception.Message);
			throw;
		}
	}
}
```

#### ApiKey.cs
```C#
using AspNetCore.Authentication.ApiKey;
class ApiKey : IApiKey
{
	public ApiKey(string key, string owner, List<Claim> claims = null)
	{
		Key = key;
		OwnerName = owner;
		Claims = claims ?? new List<Claim>();
	}

	public string Key { get; }
	public string OwnerName { get; }
	public IReadOnlyCollection<Claim> Claims { get; }
}
```

<br/>
<br/>

## Configuration (ApiKeyOptions)

### KeyName
Required to be set. It is the name of the header if it is setup as in header or the name of the query parameter if set as in query_params or the name of the route key if set as in route_values.

### Realm
Required to be set if SuppressWWWAuthenticateHeader is not set to true. It is used with WWW-Authenticate response header when challenging un-authenticated requests.  

### SuppressWWWAuthenticateHeader
Default value is false.  
If set to true, it will NOT return WWW-Authenticate response header when challenging un-authenticated requests.  
If set to false, it will return WWW-Authenticate response header when challenging un-authenticated requests.

### IgnoreAuthenticationIfAllowAnonymous (available on ASP.NET Core 3.0 onwards)
Default value is false.  
If set to true, it checks if AllowAnonymous filter on controller action or metadata on the endpoint which, if found, it does not try to authenticate the request.

### ForLegacyIgnoreExtraValidatedApiKeyCheck
Default value is false. 
If set to true, IApiKey.Key property returned from IApiKeyProvider.ProvideAsync(string) method is not compared with the key parsed from the request.
This extra check did not existed in the previous version. So you if want to revert back to old version validation, please set this to true.

### ForLegacyUseKeyNameAsSchemeNameOnWWWAuthenticateHeader
Default value is false. 
If set to true, value of KeyName property is used as scheme name on the WWW-Authenticate response header when challenging un-authenticated requests.
If set to false, the authentication scheme name (set when setting up authentication on authentication builder) is used as scheme name on the WWW-Authenticate response header when challenging un-authenticated requests.

### Events
The object provided by the application to process events raised by the api key authentication middleware.  
The application may implement the interface fully, or it may create an instance of ApiKeyEvents and assign delegates only to the events it wants to process.
- #### OnValidateKey
	A delegate assigned to this property will be invoked just before validating the api key.  
	You must provide a delegate for this property for authentication to occur.  
	In your delegate you should either call context.ValidationSucceeded() which will handle construction of authentication claims principal from the api key which will be assiged the context.Principal property and calls context.Success(), or construct an authentication claims principal from the api key and assign it to the context.Principal property and finally call context.Success() method.  
	If only context.Principal property set without calling context.Success() method then, Success() method is automaticalled called.

- #### OnAuthenticationSucceeded  
	A delegate assigned to this property will be invoked when the authentication succeeds. It will not be called if OnValidateKey delegate is assigned.  
    It can be used for adding claims, headers, etc to the response.

- #### OnAuthenticationFailed  
	A delegate assigned to this property will be invoked when any unexpected exception is thrown within the library.

- #### OnHandleChallenge  
	A delegate assigned to this property will be invoked before a challenge is sent back to the caller when handling unauthorized response.  
	Only use this if you know what you are doing and if you want to use custom implementation. Set the delegate to deal with 401 challenge concerns, if an authentication scheme in question deals an authentication interaction as part of it's request flow. (like adding a response header, or changing the 401 result to 302 of a login page or external sign-in location.)  
    Call context.Handled() at the end so that any default logic for this challenge will be skipped.

- #### OnHandleForbidden  
	A delegate assigned to this property will be invoked if Authorization fails and results in a Forbidden response.  
	Only use this if you know what you are doing and if you want to use custom implementation.  
	Set the delegate to handle Forbid.  
	Call context.Handled() at the end so that any default logic will be skipped.

<br/>
<br/>

## Extension Methods  
Many overloads for each of the below are available to follow the same convension as Microsoft follows.  

### AddApiKeyInHeader  
Adds ApiKey authentication which can handle the api key in the Header.  
WWW-Authenticate challenge header will contain parameter `in="header"`.  

### AddApiKeyInAuthorizationHeader  
Adds ApiKey authentication which can handle the api key in the Authorization Header.  
WWW-Authenticate challenge header will contain parameter `in="authorization_header"`.  

### AddApiKeyInQueryParams  
Adds ApiKey authentication which can handle the api key in the url query paramter.  
WWW-Authenticate challenge header will contain parameter `in="query_params"`.  

### AddApiKeyInHeaderOrQueryParams  
Adds ApiKey authentication which can handle the api key in the either Header, Authorization Header or Query Parameter.  
WWW-Authenticate challenge header will contain parameter `in="header_or_query_params"`.  

### AddApiKeyInRouteValues  
Adds ApiKey authentication which can handle the api key in the url route identified by the route pattern key.  
WWW-Authenticate challenge header will contain parameter `in="route_values"`.  

<br/>
<br/>

## WWW-Authenticate Header
The WWW-Authenticate header returned for unauthorized requests.  

	WWW-Authenticate: <SCHEME_NAME> realm="<REALM>", charset="UTF-8", in="<IN_PARAMERTER>", key_name="<KEY_NAME>"  

where,  
- <SCHEME_NAME> == The authentication scheme name. But, if *ApiKeyOptions.ForLegacyUseKeyNameAsSchemeNameOnWWWAuthenticateHeader* is set to true then it will be *ApiKeyOptions.KeyName*  

- &lt;REALM&gt; == *ApiKeyOptions.Realm*  

- <IN_PARAMERTER> == Depending on the [extension method](#extension-methods) used, it could be either of *header*, *authorization_header*, *query_params*, *header_or_query_params*, *route_values*  

- <KEY_NAME> == *ApiKeyOptions.KeyName*  

<br/>
<br/>

## Additional Notes

### API Key Authentication Not Challenged
With ASP.NET Core, all the requests are not challenged for authentication by default. So don't worry if your *ApiKeyProvider* or *OnValidateKey* is not hit when you don't pass the required api key authentication details with the request. It is a normal behaviour. ASP.NET Core challenges authentication only when it is specifically told to do so either by decorating controller/method with *[Authorize]* filter attribute or by some other means. 

However, if you want all the requests to challenge authentication by default, depending on what you are using, you can add the below options line to *ConfigureServices* method on *Startup* class.

```C#
// On ASP.NET Core 3.0 onwards
services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
});

// OR

// On ASP.NET Core 2.0 onwards
services.AddMvc(options => 
{
    options.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()));
});
```
  
If you are not using MVC but, using Endpoints on ASP.NET Core 3.0 or newer, you can add a chain method `.RequireAuthorization()` to the endpoint map under *Configure* method on *Startup* class as shown below.

```C#
// ASP.NET Core 3.0 onwards
app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("Hello World!");
    }).RequireAuthorization();  // NOTE THIS HERE!!!! 
});
```

### Multiple Authentication Schemes
ASP.NET Core supports adding multiple authentication schemes which this library also supports. Just need to use the extension method which takes scheme name as parameter. The rest is all same. This can be achieved in many different ways. Below is just a quick rough example. Also refer to [this conversation here](https://github.com/mihirdilip/aspnetcore-authentication-apikey/issues/20).  

Please note that scheme name parameter can be any string you want.

```C#
public void ConfigureServices(IServiceCollection services)
{
	services.AddTransient<IApiKeyRepository, InMemoryApiKeyRepository>();

	services.AddAuthentication("InHeader")
				
		.AddApiKeyInHeader<ApiKeyProvider>("InHeader", options =>
		{
			options.Realm = "Sample Web API";
			options.KeyName = "X-API-KEY";
		})

		.AddApiKeyInQueryParams<ApiKeyProvider_2>("InQueryParams", options =>
		{
			options.Realm = "Sample Web API";
			options.KeyName = "key";
		})

		.AddApiKeyInAuthorizationHeader("XYZ", options =>
		{
			options.Realm = "Sample Web API";
			options.KeyName = "APIKEY";
			options.Events = new ApiKeyEvents
			{
				OnValidateKey = async context =>
				{
					var apiKeyRepository = context.HttpContext.RequestServices.GetRequiredService<IApiKeyRepository>();
					var apiKeyObj = await apiKeyRepository.GetApiKeyAsync(context.ApiKey);
					if (apiKeyObj != null)
					{
						context.ValidationSucceeded(apiKeyObj.Claims);
					}
					else
					{
						context.ValidationFailed();
					}
				}
			};
		});

	services.AddAuthorization(options =>
	{
		options.FallbackPolicy = new AuthorizationPolicyBuilder("InHeader", "InQueryParams","XYZ")
			.RequireAuthenticatedUser()
			.Build();
	});
}
```

<br/>
<br/>

## Release Notes
| Version | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Notes |
|---------|-------|
|8.0.1    | <ul><li>Added support to have ApiKey in request route identified by route pattern key from netcoreapp3.0 onwards [#41](https://github.com/mihirdilip/aspnetcore-authentication-apikey/issues/41)</li><li>Readme updated</li></ul> |
|8.0.0    | <ul><li>net8.0 support added</li><li>Sample project for net8.0 added</li><li>ApiKeySamplesClient.http file added for testing sample projects</li><li>Readme updated</li></ul> |
|7.0.0    | <ul><li>net7.0 support added</li><li>Information log on handler is changed to Debug log when API Key is not found on the request</li><li>Added package validations</li><li>Sample project for net7.0 added</li><li>Readme updated</li><li>Readme added to package</li></ul> |
|6.0.1    | <ul><li>net6.0 support added</li><li>Information log on handler is changed to Debug log when IgnoreAuthenticationIfAllowAnonymous is enabled</li><li>Sample project added</li><li>Readme updated</li><li>Copyright year updated on License</li></ul> |
|5.1.0    | <ul><li>WWW-Authenticate challenge header now returns SchemeName as scheme part instead of ApiKeyOptions.KeyName</li><li>WWW-Authenticate challenge header now has 2 new parameters 'in' and 'key_name' in value part</li><li>ForLegacyUseKeyNameAsSchemeNameOnWWWAuthenticateHeader added to the ApiKeyOptions</li><li>In Authorization Header now able to use either SchemeName or ApiKeyOptions.KeyName when matching AuthorizationHeader Scheme</li><li>Visibility of all the handlers changed to public [#21](https://github.com/mihirdilip/aspnetcore-authentication-apikey/issues/21)</li><li>Tests added</li><li>Readme updated</li><li>Copyright year updated on License</li></ul> |
|5.0.0    | <ul><li>Net 5.0 target framework added</li><li>IgnoreAuthenticationIfAllowAnonymous added to the ApiKeyOptions from netcoreapp3.0 onwards [#15](https://github.com/mihirdilip/aspnetcore-authentication-apikey/issues/15)</li></ul> |
|3.1.1    | <ul><li>Ability to have ApiKey in Authorization header added</li><li>Fixed extensions methods to use correct handler [#13](https://github.com/mihirdilip/aspnetcore-authentication-apikey/issues/13)</li><li>Fixed issue with resolving of IApiKeyProvider implementation when using multiple schemes [#12](https://github.com/mihirdilip/aspnetcore-authentication-apikey/issues/12)</li></ul> |
|3.1.0    | <ul><li>Multitarget framework support added</li><li>Strong Name Key support added</li><li>Source Link support added</li><li>SuppressWWWAuthenticateHeader added to configure options</li><li>ForLegacyIgnoreExtraValidatedApiKeyCheck added to configure options</li><li>Events added to configure options</li></ul> |
|2.2.0    | <ul><li>API Key Authentication Implementation for ASP.NET Core. It can be setup so that it can accept API Key in Header, QueryParams or HeaderOrQueryParams.</li></ul> |

<br/>
<br/>

## References
- [ASP.NET Core Security documentation](https://docs.microsoft.com/en-us/aspnet/core/security)
- [aspnet/Security](https://github.com/dotnet/aspnetcore/tree/master/src/Security)

## License
[MIT License](https://github.com/mihirdilip/aspnetcore-authentication-apikey/blob/master/LICENSE)
