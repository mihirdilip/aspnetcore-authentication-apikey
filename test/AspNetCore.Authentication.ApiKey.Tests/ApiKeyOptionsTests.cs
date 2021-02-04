using Xunit;

namespace AspNetCore.Authentication.ApiKey.Tests
{
    public class ApiKeyOptionsTests
    {
        [Fact]
        public void Events_default_not_null()
        {
            var options = new ApiKeyOptions();
            Assert.NotNull(options.Events);
        }

        [Fact]
        public void SuppressWWWAuthenticateHeader_default_false()
        {
            var options = new ApiKeyOptions();
            Assert.False(options.SuppressWWWAuthenticateHeader);
        }

        [Fact]
        public void ForLegacyIgnoreExtraValidatedApiKeyCheck_default_false()
        {
            var options = new ApiKeyOptions();
            Assert.False(options.ForLegacyIgnoreExtraValidatedApiKeyCheck);
        }

        [Fact]
        public void ApiKeyProviderType_default_null()
        {
            var options = new ApiKeyOptions();
            Assert.Null(options.ApiKeyProviderType);
        }

#if !(NET461 || NETSTANDARD2_0 || NETCOREAPP2_1)
        [Fact]
        public void IgnoreAuthenticationIfAllowAnonymous_default_false()
        {
            var options = new ApiKeyOptions();
            Assert.False(options.IgnoreAuthenticationIfAllowAnonymous);
        }
#endif

    }
}
