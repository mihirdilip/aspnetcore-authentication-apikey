using AspNetCore.Authentication.ApiKey;
using Microsoft.Extensions.Logging;
using SampleWebApi.Repositories;
using System.Threading.Tasks;

namespace SampleWebApi.Services
{
    class ApiKeyProvider : IApiKeyProvider
	{
		private readonly ILogger<IApiKeyProvider> _logger;
		private readonly IApiKeyRepository _apiKeyRepository;

		public ApiKeyProvider(ILogger<IApiKeyProvider> logger, IApiKeyRepository apiKeyRepository)
		{
			_logger = logger;
			_apiKeyRepository = apiKeyRepository;
		}

		public Task<IApiKey> ProvideAsync(string key)
		{
			try
			{
				return _apiKeyRepository.GetApiKeyAsync(key);
			}
			catch (System.Exception exception)
			{
				_logger.LogError(exception, exception.Message);
				throw;
			}
		}
	}
}