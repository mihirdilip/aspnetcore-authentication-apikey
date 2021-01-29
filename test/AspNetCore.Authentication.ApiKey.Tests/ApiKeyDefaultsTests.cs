using Xunit;

namespace AspNetCore.Authentication.ApiKey.Tests
{
    public class ApiKeyDefaultsTests
    {
        [Fact]
        public void AuthenticationSchemeValueTest()
        {
            Assert.Equal("ApiKey", ApiKeyDefaults.AuthenticationScheme);
        }
    }
}
