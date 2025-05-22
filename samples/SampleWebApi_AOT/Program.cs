using AspNetCore.Authentication.ApiKey;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using SampleWebApi.Repositories;
using SampleWebApi.Services;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);
builder.WebHost.UseKestrelHttpsConfiguration();

// Add User repository to the dependency container.
builder.Services.AddTransient<IApiKeyRepository, InMemoryApiKeyRepository>();

// Add the ApiKey scheme authentication here..
// It requires Realm to be set in the options if SuppressWWWAuthenticateHeader is not set.
// If an implementation of IApiKeyProvider interface is registered in the dependency register as well as OnValidateKey delegete on options.Events is also set then this delegate will be used instead of an implementation of IApiKeyProvider.
builder.Services.AddAuthentication(ApiKeyDefaults.AuthenticationScheme)

	// The below AddApiKeyInHeaderOrQueryParams without type parameter will require OnValidateKey delegete on options.Events to be set unless an implementation of IApiKeyProvider interface is registered in the dependency register.
	// Please note if OnValidateKey delegete on options.Events is also set then this delegate will be used instead of ApiKeyProvider.*
	//.AddApiKeyInHeaderOrQueryParams(options =>

	// The below AddApiKeyInHeaderOrQueryParams with type parameter will add the ApiKeyProvider to the dependency register. 
	// Please note if OnValidateKey delegete on options.Events is also set then this delegate will be used instead of ApiKeyProvider.
	.AddApiKeyInHeaderOrQueryParams<ApiKeyProvider>(options =>
	{
		options.Realm = "Sample Web API";
		options.KeyName = "X-API-KEY";

		//// Optional option to suppress the browser login dialog for ajax calls.
		//options.SuppressWWWAuthenticateHeader = true;

		//// Optional option to ignore extra check of ApiKey string after it is validated.
		//options.ForLegacyIgnoreExtraValidatedApiKeyCheck = true;

		//// Optional option to ignore authentication if AllowAnonumous metadata/filter attribute is added to an endpoint.
		//options.IgnoreAuthenticationIfAllowAnonymous = true;

		//// Optional events to override the ApiKey original logic with custom logic.
		//// Only use this if you know what you are doing at your own risk. Any of the events can be assigned. 
		options.Events = new ApiKeyEvents
		{

			//// A delegate assigned to this property will be invoked just before validating the api key. 
			//OnValidateKey = async (context) =>
			//{
			//    // custom code to handle the api key, create principal and call Success method on context.
			//    var apiKeyRepository = context.HttpContext.RequestServices.GetRequiredService<IApiKeyRepository>();
			//    var apiKey = await apiKeyRepository.GetApiKeyAsync(context.ApiKey);
			//    var isValid = apiKey != null && apiKey.Key.Equals(context.ApiKey, StringComparison.OrdinalIgnoreCase);
			//    if (isValid)
			//    {
			//        context.Response.Headers.Add("ValidationCustomHeader", "From OnValidateKey");
			//        var claims = new[]
			//        {
			//            new Claim(ClaimTypes.NameIdentifier, apiKey.OwnerName, ClaimValueTypes.String, context.Options.ClaimsIssuer),
			//            new Claim(ClaimTypes.Name, apiKey.OwnerName, ClaimValueTypes.String, context.Options.ClaimsIssuer),
			//            new Claim("CustomClaimType", "Custom Claim Value - from OnValidateKey")
			//        };
			//        context.Principal = new ClaimsPrincipal(new ClaimsIdentity(claims, context.Scheme.Name));
			//        context.Success();
			//    }
			//    else
			//    {
			//        context.NoResult();
			//    }
			//},

			//// A delegate assigned to this property will be invoked just before validating the api key. 
			//// NOTE: Same as above delegate but slightly different implementation which will give same result.
			//OnValidateKey = async (context) =>
			//{
			//    // custom code to handle the api key, create principal and call Success method on context.
			//    var apiKeyRepository = context.HttpContext.RequestServices.GetRequiredService<IApiKeyRepository>();
			//    var apiKey = await apiKeyRepository.GetApiKeyAsync(context.ApiKey);
			//    var isValid = apiKey != null && apiKey.Key.Equals(context.ApiKey, StringComparison.OrdinalIgnoreCase);
			//    if (isValid)
			//    {
			//        context.Response.Headers.Add("ValidationCustomHeader", "From OnValidateKey");
			//        var claims = new[]
			//        {
			//            new Claim("CustomClaimType", "Custom Claim Value - from OnValidateKey")
			//        };
			//        context.ValidationSucceeded(apiKey.OwnerName, claims);    // claims are optional
			//    }
			//    else
			//    {
			//        context.ValidationFailed();
			//    }
			//},

			//// A delegate assigned to this property will be invoked before a challenge is sent back to the caller when handling unauthorized response.
			//OnHandleChallenge = async (context) =>
			//{
			//    // custom code to handle authentication challenge unauthorized response.
			//    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
			//    context.Response.Headers.Add("ChallengeCustomHeader", "From OnHandleChallenge");
			//    await context.Response.WriteAsync("{\"CustomBody\":\"From OnHandleChallenge\"}");
			//    context.Handled();  // important! do not forget to call this method at the end.
			//},

			//// A delegate assigned to this property will be invoked if Authorization fails and results in a Forbidden response.
			//OnHandleForbidden = async (context) =>
			//{
			//    // custom code to handle forbidden response.
			//    context.Response.StatusCode = StatusCodes.Status403Forbidden;
			//    context.Response.Headers.Add("ForbidCustomHeader", "From OnHandleForbidden");
			//    await context.Response.WriteAsync("{\"CustomBody\":\"From OnHandleForbidden\"}");
			//    context.Handled();  // important! do not forget to call this method at the end.
			//},

			//// A delegate assigned to this property will be invoked when the authentication succeeds. It will not be called if OnValidateKey delegate is assigned.
			//// It can be used for adding claims, headers, etc to the response.
			//OnAuthenticationSucceeded = (context) =>
			//{
			//    //custom code to add extra bits to the success response.
			//    context.Response.Headers.Add("SuccessCustomHeader", "From OnAuthenticationSucceeded");
			//    var customClaims = new List<Claim>
			//    {
			//        new Claim("CustomClaimType", "Custom Claim Value - from OnAuthenticationSucceeded")
			//    };
			//    context.AddClaims(customClaims);
			//    //or can add like this - context.Principal.AddIdentity(new ClaimsIdentity(customClaims));
			//    return Task.CompletedTask;
			//},

			//// A delegate assigned to this property will be invoked when the authentication fails.
			//OnAuthenticationFailed = (context) =>
			//{
			//    // custom code to handle failed authentication.
			//    context.Fail("Failed to authenticate");
			//    return Task.CompletedTask;
			//}

		};
	});

// All the requests will need to be authorized. 
// Alternatively, add [Authorize] attribute to Controller or Action Method where necessary.
builder.Services.AddAuthorizationBuilder()
	.AddFallbackPolicy(
		"FallbackPolicy",
		new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()
	);

builder.Services.ConfigureHttpJsonOptions(options =>
{
	options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();    // NOTE: DEFAULT TEMPLATE DOES NOT HAVE THIS, THIS LINE IS REQUIRED AND HAS TO BE ADDED!!!

app.UseAuthorization();

var valuesApi = app.MapGroup("/api/values")
	.RequireAuthorization();
valuesApi.MapGet("/", () => new[] { "value1", "value2" })
	.AllowAnonymous();
valuesApi.MapGet("/claims", async (context) =>
{
	var sb = new StringBuilder();
	foreach (var claim in context.User.Claims)
	{
		sb.AppendLine($"{claim.Type}: {claim.Value}");
	}
	context.Response.StatusCode = 200;
	await context.Response.WriteAsync(sb.ToString());
});
valuesApi.MapGet("/forbid", async (context) =>
{
	await context.ForbidAsync();
});

app.Run();


[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(string[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}