using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetCore.Authentication.ApiKey
{
    public class ApiKeyValidator: IApiKeyValidator
    {
        public bool IsValidKey(IApiKey apiKey, string key)
        {
            if ((apiKey?.Key == null) || String.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            return (apiKey.Key.FirstOrDefault(ak => String.Equals(ak, key, apiKey.StringComparison)) !=  null);
           
        }
    }
}
