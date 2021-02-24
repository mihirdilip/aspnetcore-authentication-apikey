# AspNetCore.Authentication.ApiKey
Easy to use and very light weight Microsoft style API Key Authentication Implementation for ASP.NET Core. It can be setup so that it can accept API Key either in Header, Authorization Header, QueryParams or HeaderOrQueryParams.

[View On GitHub](https://github.com/mihirdilip/aspnetcore-authentication-apikey)

## Installing
This library is published on NuGet. So the NuGet package can be installed directly to your project if you wish to use it without making any custom changes to the code.

Download directly from [AspNetCore.Authentication.ApiKey](https://www.nuget.org/packages/AspNetCore.Authentication.ApiKey).

Or by running the below command on your project.

```
PM> Install-Package AspNetCore.Authentication.ApiKey
```

## Example Usage

Samples are available under [samples directory](samples).

Setting it up is quite simple. You will need basic working knowledge of ASP.NET Core 2.2 or newer to get started using this code.

On [**Startup.cs**](#startupcs), as shown below, add 2 lines in *ConfigureServices* method `services.AddAuthentication(ApiKeyDefaults.AuthenticationScheme).AddApiKeyInHeaderOrQueryParams<ApiKeyProvider>(options => { options.Realm = "My App"; options.KeyName = "X-API-KEY"; });`. And a line `app.UseAuthentication();` in *Configure* method.

Also add an implementation of *IApiKeyProvider* as shown below in [**ApiKeyProvider.cs**](#apikeyprovidercs) and also an implementation of *IApiKey* as shown below in [**ApiKey.cs**](#apikeycs).

**NOTE: Always use HTTPS (SSL Certificate) protocol in production when using API Key authentication.**

#### Startup.cs (ASP.NET Core 3.0 or newer)

```C#
using AspNetCore.Authentication.ApiKey;
public class Startup
{
	public void ConfigureServices(IServiceCollection services)
	{
		// Add the ApiKey scheme authentication here..
		// It requires Realm to be set in the options if SuppressWWWAuthenticateHeader is not set.
		// If an implementation of IApiKeyProvider interface is registered in the dependency register as well as OnValidateKey delegete on options.Events is also set then this delegate will be used instead of an implementation of IApiKeyProvider.
		services.AddAuthentication(ApiKeyDefaults.AuthenticationScheme)

			// The below AddApiKeyInHeaderOrQueryParams without type parameter will require OnValidateKey delegete on options.Events to be set unless an implementation of IApiKeyProvider interface is registered in the dependency register.
			// Please note if OnValidateKey delegete on options.Events is also set then this delegate will be used instead of ApiKeyProvider.*
			//.AddApiKeyInHeaderOrQueryParams(options =>

			// The below AddApiKeyInHeaderOrQueryParams with type parameter will add the ApiKeyProvider to the dependency register. 
			// Please note if OnValidateKey delegete on options.Events is also set then this delegate will be used instead of ApiKeyProvider.
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


#### Startup.cs (ASP.NET Core 2.2)

```C#
using AspNetCore.Authentication.ApiKey;
public class Startup
{
	public void ConfigureServices(IServiceCollection services)
	{
		// Add the ApiKey scheme authentication here..
		// It requires Realm to be set in the options if SuppressWWWAuthenticateHeader is not set.
		// If an implementation of IApiKeyProvider interface is registered in the dependency register as well as OnValidateKey delegete on options.Events is also set then this delegate will be used instead of an implementation of IApiKeyProvider.
		services.AddAuthentication(ApiKeyDefaults.AuthenticationScheme)

			// The below AddApiKeyInHeaderOrQueryParams without type parameter will require OnValidateKey delegete on options.Events to be set unless an implementation of IApiKeyProvider interface is registered in the dependency register.
			// Please note if OnValidateKey delegete on options.Events is also set then this delegate will be used instead of ApiKeyProvider.*
			//.AddApiKeyInHeaderOrQueryParams(options =>

			// The below AddApiKeyInHeaderOrQueryParams with type parameter will add the ApiKeyProvider to the dependency register. 
			// Please note if OnValidateKey delegete on options.Events is also set then this delegate will be used instead of ApiKeyProvider.
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
	private readonly ILogger<ApiKeyProvider> _logger;
	
	public ApiKeyProvider(ILogger<ApiKeyProvider> logger)
	{
		_logger = logger;
	}

	public Task<IApiKey> ProvideAsync(string key)
	{
		try
		{
			// write your validation implementation here and return an instance of a valid ApiKey or retun null for an invalid key.
			return Task.FromResult(null);
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

## Configuration (ApiKeyOptions)
#### KeyName
Required to be set. It is the name of the header if it is setup as in-header or the name of the query parameter if set as in-query-string.

#### Realm
Required to be set if SuppressWWWAuthenticateHeader is not set to true. It is used with WWW-Authenticate response header when challenging un-authenticated requests.  

#### SuppressWWWAuthenticateHeader
Default value is false.  
When set to true, it will NOT return WWW-Authenticate response header when challenging un-authenticated requests.  
When set to false, it will return WWW-Authenticate response header when challenging un-authenticated requests.

#### IgnoreAuthenticationIfAllowAnonymous
Default value is false.  
If set to true, it checks if AllowAnonymous filter on controller action or metadata on the endpoint which, if found, it does not try to authenticate the request.

#### ForLegacyIgnoreExtraValidatedApiKeyCheck
Default value is false. 
If set to true, IApiKey.Key property returned from IApiKeyProvider.ProvideAsync(string) method is not compared with the key parsed from the request.
This extra check did not existed in the previous version. So you if want to revert back to old version validation, please set this to true.

#### ForLegacyUseKeyNameAsSchemeNameOnWWWAuthenticateHeader
Default value is false. 
If set to true then value of KeyName property is used as scheme name on the WWW-Authenticate response header when challenging un-authenticated requests.
else, the authentication scheme name (set when adding this authentication to the authentication builder) is used as scheme name on the WWW-Authenticate response header when challenging un-authenticated requests.

#### Events
The object provided by the application to process events raised by the api key authentication middleware.  
The application may implement the interface fully, or it may create an instance of ApiKeyEvents and assign delegates only to the events it wants to process.
- ##### OnValidateKey
	A delegate assigned to this property will be invoked just before validating the api key.  
	You must provide a delegate for this property for authentication to occur.  
	In your delegate you should either call context.ValidationSucceeded() which will handle construction of authentication principal from the api key which will be assiged the context.Principal property and call context.Success(), or construct an authentication principal from the api key & attach it to the context.Principal property and finally call context.Success() method.  
	If only context.Principal property set without calling context.Success() method then, Success() method is automaticalled called.

- ##### OnAuthenticationSucceeded  
	A delegate assigned to this property will be invoked when the authentication succeeds. It will not be called if OnValidateKey delegate is assigned.  
    It can be used for adding claims, headers, etc to the response.

- ##### OnAuthenticationFailed  
	A delegate assigned to this property will be invoked when the authentication fails.

- ##### OnHandleChallenge  
	A delegate assigned to this property will be invoked before a challenge is sent back to the caller when handling unauthorized response.  
	Only use this if you know what you are doing and if you want to use custom implementation.  Set the delegate to deal with 401 challenge concerns, if an authentication scheme in question deals an authentication interaction as part of it's request flow. (like adding a response header, or changing the 401 result to 302 of a login page or external sign-in location.)  
    Call context.Handled() at the end so that any default logic for this challenge will be skipped.

- ##### OnHandleForbidden  
	A delegate assigned to this property will be invoked if Authorization fails and results in a Forbidden response.  
	Only use this if you know what you are doing and if you want to use custom implementation.  
	Set the delegate to handle Forbid.  
	Call context.Handled() at the end so that any default logic will be skipped.


## Additional Notes
Please note that, by default, with ASP.NET Core, all the requests are not challenged for authentication. So don't worry if your *ApiKeyProvider* is not hit when you don't pass the required api key authentication details with the request. It is a normal behaviour. ASP.NET Core challenges authentication only when it is specifically told to do so either by decorating controller/method with *[Authorize]* filter attribute or by some other means. 

However, if you want all the requests to challenge authentication by default, depending on what you are using, you can add the below options line to *ConfigureServices* method on *Startup* class.

```C#
services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
});

// OR

services.AddMvc(options => 
{
    options.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()));
});
```
  
If you are not using MVC but, using Endpoints on ASP.NET Core 3.0 or newer, you can add a chain method `.RequireAuthorization()` to the endpoint map under *Configure* method on *Startup* class as shown below.

```C#
app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("Hello World!");
    }).RequireAuthorization();  // NOTE THIS HERE!!!! 
});
```


## License
[MIT License](https://github.com/mihirdilip/aspnetcore-authentication-apikey/blob/master/LICENSE)
