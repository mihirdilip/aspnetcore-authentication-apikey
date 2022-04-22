//using AspNetCore.Authentication.ApiKey;
//using Microsoft.AspNetCore.Authorization;
//using SampleWebApi.Repositories;
//using SampleWebApi.Services;
//using SampleWebApi_6_0;

//var builder = WebApplication.CreateBuilder(args);

//// Add User repository to the dependency container.
//builder.Services.AddTransient<IApiKeyRepository, InMemoryApiKeyRepository>();

//// Add ApiKey provider factory.
//builder.Services.AddTransient<IApiKeyProviderFactory, ApiKeyProviderFactory>();

//// Add the ApiKey scheme authentication here..
//// It requires Realm to be set in the options if SuppressWWWAuthenticateHeader is not set.
//builder.Services.AddAuthentication( "Test1" )

//	.AddApiKeyInHeader("Test1", options =>
//	{
//		options.Realm = "Sample Web API 1";
//		options.KeyName = "X-API-KEY";
//	})
//	.AddApiKeyInHeader("Test2", options =>
//	{
//		options.Realm = "Sample Web API 2";
//		options.KeyName = "X-API-KEY";
//	})
//	.AddApiKeyInHeader("Test3", options =>
//	{
//		options.Realm = "Sample Web API 3";
//		options.KeyName = "X-API-KEY";
//	});

//builder.Services.AddControllers(options =>
//{
//    // ALWAYS USE HTTPS (SSL) protocol in production when using ApiKey authentication.
//    //options.Filters.Add<RequireHttpsAttribute>();

//}); //.AddXmlSerializerFormatters()   // To enable XML along with JSON;

//// All the requests will need to be authorized. 
//// Alternatively, add [Authorize] attribute to Controller or Action Method where necessary.
//builder.Services.AddAuthorization(options =>
//{
//    options.FallbackPolicy = new AuthorizationPolicyBuilder()
//        .RequireAuthenticatedUser()
//        .Build();
//});

//var app = builder.Build();

//// Configure the HTTP request pipeline.

//app.UseHttpsRedirection();

//app.UseAuthentication();    // NOTE: DEFAULT TEMPLATE DOES NOT HAVE THIS, THIS LINE IS REQUIRED AND HAS TO BE ADDED!!!

//app.UseAuthorization();

//app.MapControllers();

//app.Run();
