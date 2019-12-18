# AspNetCore.Authentication.ApiKey
Easy to use and very light weight Microsoft style API Key Authentication Implementation for ASP.NET Core. It can be setup so that it can accept API Key in Header, QueryParams or HeaderOrQueryParams.

## Installing
This library is published on NuGet. So the NuGet package can be installed directly to your project if you wish to use it without making any custom changes to the code.

Download directly from [AspNetCore.Authentication.ApiKey](https://www.nuget.org/packages/AspNetCore.Authentication.ApiKey).

Or by running the below command on your project.

```
PM> Install-Package AspNetCore.Authentication.ApiKey
```

## Example Usage

Setting it up is quite simple. You will need basic working knowledge of ASP.NET Core 2.2 to get started using this code.

On [**Startup.cs**](#startupcs), as shown below, add 2 lines in *ConfigureServices* method `services.AddAuthentication(ApiKeyDefaults.AuthenticationScheme).AddApiKeyInHeaderOrQueryParams<ApiKeyProvider>(options => { options.Realm = "My App"; options.KeyName = "X-API-KEY"; });`. And a line `app.UseAuthentication();` in *Configure* method.

Also add an implementation of *IApiKeyProvider* as shown below in [**ApiKeyProvider.cs**](#apikeyprovidercs) and also an implementation of *IApiKey* as shown below in [**ApiKey.cs**](#apikeycs).

**NOTE: Always use HTTPS (SSL Certificate) protocol in production when using API Key authentication.**

#### Startup.cs

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
		services.AddAuthentication(BasicDefaults.AuthenticationScheme)
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

## License
[MIT License](https://github.com/mihirdilip/aspnetcore-authentication-apikey/blob/master/LICENSE)
