// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Style", "IDE0028:Simplify collection initialization", Justification = "<Pending>", Scope = "member", Target = "~F:SampleWebApi.Repositories.InMemoryApiKeyRepository._cache")]
[assembly: SuppressMessage("Style", "IDE0090:Use 'new(...)'", Justification = "<Pending>", Scope = "member", Target = "~F:SampleWebApi.Repositories.InMemoryApiKeyRepository._cache")]
[assembly: SuppressMessage("Style", "IDE0290:Use primary constructor", Justification = "<Pending>", Scope = "member", Target = "~M:SampleWebApi.Models.ApiKey.#ctor(System.String,System.String,System.Collections.Generic.List{System.Security.Claims.Claim})")]
[assembly: SuppressMessage("Style", "IDE0028:Simplify collection initialization", Justification = "<Pending>", Scope = "member", Target = "~M:SampleWebApi.Models.ApiKey.#ctor(System.String,System.String,System.Collections.Generic.List{System.Security.Claims.Claim})")]
[assembly: SuppressMessage("Style", "IDE0290:Use primary constructor", Justification = "<Pending>", Scope = "member", Target = "~M:SampleWebApi.Services.ApiKeyProvider.#ctor(Microsoft.Extensions.Logging.ILogger{AspNetCore.Authentication.ApiKey.IApiKeyProvider},SampleWebApi.Repositories.IApiKeyRepository)")]

[assembly: SuppressMessage("Usage", "CA2254:Template should be a static expression", Justification = "<Pending>", Scope = "member", Target = "~M:SampleWebApi.Services.ApiKeyProvider.ProvideAsync(System.String)~System.Threading.Tasks.Task{AspNetCore.Authentication.ApiKey.IApiKey}")]

