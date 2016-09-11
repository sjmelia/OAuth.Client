using Xunit;
using System.Net.Http;
using System.Threading.Tasks;

namespace ArrayOfBytes.OAuth.Client.Tests
{
    public class OAuthHeaderFactoryTests
    {
        [Fact]
        public async Task CheckOAuthHeader()
        {
            var config = new OAuthConfig(
                "xvz1evFS4wEEPTGEFPHBog",
                string.Empty,
                "370773112-GmHxMAgYyLbNEtIKZeRNFsMKPR9EyMZeS9weJAEb",
                string.Empty);

            var headerFactory = new OAuthHeaderFactory(
                config,
                () => "kYjzVBB8Y0ZFabxSWbWovY3uYSQ2pTgmZeNu2VS4cg",
                () => "1318622958",
                new MockSignatureFactory());

            var request = new HttpRequestMessage();
            var actual = await headerFactory.GetAuthorisationHeaderParameter(request);

            Assert.Equal("oauth_consumer_key=\"xvz1evFS4wEEPTGEFPHBog\", oauth_nonce=\"kYjzVBB8Y0ZFabxSWbWovY3uYSQ2pTgmZeNu2VS4cg\", oauth_signature=\"tnnArxj06cWHq44gCs1OSKk%2FjLY%3D\", oauth_signature_method=\"HMAC-SHA1\", oauth_timestamp=\"1318622958\", oauth_token=\"370773112-GmHxMAgYyLbNEtIKZeRNFsMKPR9EyMZeS9weJAEb\", oauth_version=\"1.0\"",
                actual);
        }
    }
}
