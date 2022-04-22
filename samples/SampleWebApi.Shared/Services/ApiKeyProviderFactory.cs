using AspNetCore.Authentication.ApiKey;
using Microsoft.Extensions.Logging;
using SampleWebApi.Repositories;
using System.Diagnostics;

namespace SampleWebApi.Services
{
	internal class ApiKeyProviderFactory : IApiKeyProviderFactory
	{
		private readonly ILoggerFactory loggerFactory;
		private readonly IApiKeyRepository apiKeyRepository;

		public ApiKeyProviderFactory(ILoggerFactory loggerFactory, IApiKeyRepository apiKeyRepository)
		{
			this.loggerFactory = loggerFactory;
			this.apiKeyRepository = apiKeyRepository;
		}

		/// <inheritdoc />
		public IApiKeyProvider CreateApiKeyProvider(string authenticationSchemaName)
		{
			Debug.WriteLine(authenticationSchemaName);
			return new ApiKeyProvider(loggerFactory.CreateLogger<ApiKeyProvider>(), this.apiKeyRepository);
		}
	}
}
