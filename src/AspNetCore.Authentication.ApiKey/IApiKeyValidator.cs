using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.Authentication.ApiKey
{
    public interface IApiKeyValidator
    {
        bool IsValidKey(IApiKey apiKey, string key);
    }
}
