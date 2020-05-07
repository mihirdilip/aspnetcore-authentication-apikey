# AspNetCore.Authentication.ApiKey
Easy to use and very light weight Microsoft style API Key Authentication Implementation for ASP.NET Core. It can be setup so that it can accept API Key in Header, QueryParams or HeaderOrQueryParams.

[View On GitHub](https://github.com/mihirdilip/aspnetcore-authentication-apikey)

## Installing
This library is published on NuGet. So the NuGet package can be installed directly to your project if you wish to use it without making any custom changes to the code.

Download directly from [AspNetCore.Authentication.ApiKey](https://www.nuget.org/packages/AspNetCore.Authentication.ApiKey).

Or by running the below command on your project.

```
PM> Install-Package AspNetCore.Authentication.ApiKey
```

## Example Usage

Setting it up is quite simple. You will need basic working knowledge of ASP.NET Core 2.2 or newer to get started using this code.

On [**Startup.cs**](#startupcs), as shown below, add 2 lines in *ConfigureServices* method `services.AddAuthentication(ApiKeyDefaults.AuthenticationScheme).AddApiKeyInHeaderOrQueryParams<ApiKeyProvider>(options => { options.Realm = "My App"; options.KeyName = "X-API-KEY"; });`. And a line `app.UseAuthentication();` in *Configure* method.

Also add an implementation of *IApiKeyProvider* as shown below in [**ApiKeyProvider.cs**](#apikeyprovidercs) and also an implementation of *IApiKey* as shown below in [**ApiKey.cs**](#apikeycs).

**NOTE: Always use HTTPS (SSL Certificate) protocol in production when using API Key authentication.**

#### Startup.cs (ASP.NET Core 3.0 or newer)

```C#
using AspNetCore.Authentication.ApiKey;
public class Startup
{
	public Startup(IConfiguration configuration)
	{
		Configuration = configuration;
	}

	public IConfiguration Configuration { get; }

	public void ConfigureServices(IServiceCollection services)
	{
		// Add the API Key authentication here..
		// AddApiKeyInHeaderOrQueryParams extension takes an implementation of IApiKeyProvider for validating the key. 
		// It also requires Realm and KeyName to be set in the options.
		services.AddAuthentication(ApiKeyDefaults.AuthenticationScheme)
			//// use below to accept API Key either in header or query parameter
			.AddApiKeyInHeaderOrQueryParams<ApiKeyProvider>(options => 
			{ 
				options.Realm = "My App"; 
				options.KeyName = "X-API-KEY";	// Your api key name which the clients will require to send the key.
			});

			//// use below instead to only accept API Key in header
			//.AddApiKeyInHeader<ApiKeyProvider>(options => 
			//{ 
			//	options.Realm = "My App"; 
			//	options.KeyName = "X-API-KEY";	// Your api key name which the clients will require to send the key.
			//});

			//// use below instead to only accept API Key in query parameter
			//.AddApiKeyQueryParams<ApiKeyProvider>(options => 
			//{ 
			//	options.Realm = "My App"; 
			//	options.KeyName = "X-API-KEY";	// Your api key name which the clients will require to send the key.
			//});

		services.AddControllers();

		//// By default, authentication is not challenged for every request which is ASP.NET Core's default intended behaviour.
		//// So to challenge authentication for every requests please use below option instead of above services.AddControllers().
		//services.AddControllers(options => 
		//{
		//	options.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()));
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
	public Startup(IConfiguration configuration)
	{
		Configuration = configuration;
	}

	public IConfiguration Configuration { get; }

	public void ConfigureServices(IServiceCollection services)
	{
		// Add the API Key authentication here..
		// AddApiKeyInHeaderOrQueryParams extension takes an implementation of IApiKeyProvider for validating the key. 
		// It also requires Realm and KeyName to be set in the options.
		services.AddAuthentication(ApiKeyDefaults.AuthenticationScheme)
			//// use below to accept API Key either in header or query parameter
			.AddApiKeyInHeaderOrQueryParams<ApiKeyProvider>(options => 
			{ 
				options.Realm = "My App"; 
				options.KeyName = "X-API-KEY";	// Your api key name which the clients will require to send the key.
			});

			//// use below instead to only accept API Key in header
			//.AddApiKeyInHeader<ApiKeyProvider>(options => 
			//{ 
			//	options.Realm = "My App"; 
			//	options.KeyName = "X-API-KEY";	// Your api key name which the clients will require to send the key.
			//});

			//// use below instead to only accept API Key in query parameter
			//.AddApiKeyQueryParams<ApiKeyProvider>(options => 
			//{ 
			//	options.Realm = "My App"; 
			//	options.KeyName = "X-API-KEY";	// Your api key name which the clients will require to send the key.
			//});

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
	
	public BasicUserValidationService(ILogger<ApiKeyProvider> logger)
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

## Additional Notes
Please note that, by default, with ASP.NET Core, all the requests are not challenged for authentication. So don't worry if your *ApiKeyProvider* is not hit when you don't pass the required api key authentication details with the request. It is a normal behaviour. ASP.NET Core challenges authentication only when it is specifically told to do so either by decorating controller/method with *[Authorize]* filter attribute or by some other means. 

However, if you want all the requests to challenge authentication by default, depending on what you are using, you can add the below options line to *ConfigureServices* method on *Startup* class.

```C#
services.AddControllers(options => 
{ 
    options.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()));
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
