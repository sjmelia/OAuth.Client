using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ArrayOfBytes.OAuth.Client.Tests
{
    public class HmacSha1SignatureFactoryTests
    {
        private OAuthConfig config;

        public HmacSha1SignatureFactoryTests()
        {
            this.config = new OAuthConfig(
                "xvz1evFS4wEEPTGEFPHBog",
                "kAcSOqF21Fu85e7zjz7ZN2U4ZRhfV3WpwPAoE3Z7kBw",
                "370773112-GmHxMAgYyLbNEtIKZeRNFsMKPR9EyMZeS9weJAEb",
                "LswwdoUaIvS8ltyTt5jkRh4J50vUPVVHtR2YPi5kE");
        }

        private HttpRequestMessage PostMessage()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.twitter.com/1/statuses/update.json?include_entities=true");
            KeyValuePair<string, string> status = new KeyValuePair<string, string>("status", "Hello Ladies + Gentlemen, a signed OAuth request!");
            request.Content = new FormUrlEncodedContent(new KeyValuePair<string, string>[] { status });
            return request;
        }

        private HttpRequestMessage GetMessage()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.twitter.com/1.1/statuses/home_timeline.json");
            return request;
        }

        [Fact]
        public async void CheckSignaturePost()
        {
            var factory = new HmacSha1SignatureFactory();
            var actual = await factory.Get(
                this.PostMessage(),
                this.config,
                "kYjzVBB8Y0ZFabxSWbWovY3uYSQ2pTgmZeNu2VS4cg",
                "1318622958",
                "1.0");

            Assert.Equal("tnnArxj06cWHq44gCs1OSKk/jLY=", actual);
        }

        [Fact]
        public async void CheckSignatureGet()
        {
            var factory = new HmacSha1SignatureFactory();
            var actual = await factory.Get(
                this.GetMessage(),
                this.config,
                "kYjzVBB8Y0ZFabxSWbWovY3uYSQ2pTgmZeNu2VS4cg",
                "1318622958",
                "1.0");

            Assert.Equal("MChed2Nhee5rRVbvne6/Ix8EwpY=", actual);
        }

        [Fact]
        public async void CheckParameterSignature()
        {            
            var factory = new HmacSha1SignatureFactory();
            var actual = await factory.GetParameterBase(
                this.PostMessage(),
                this.config,
                "kYjzVBB8Y0ZFabxSWbWovY3uYSQ2pTgmZeNu2VS4cg", 
                "1318622958",
                "1.0");

            Assert.Equal("include_entities=true&oauth_consumer_key=xvz1evFS4wEEPTGEFPHBog&oauth_nonce=kYjzVBB8Y0ZFabxSWbWovY3uYSQ2pTgmZeNu2VS4cg&oauth_signature_method=HMAC-SHA1&oauth_timestamp=1318622958&oauth_token=370773112-GmHxMAgYyLbNEtIKZeRNFsMKPR9EyMZeS9weJAEb&oauth_version=1.0&status=Hello%20Ladies%20%2B%20Gentlemen%2C%20a%20signed%20OAuth%20request%21", actual);
        }

        [Fact]
        public async void CheckSignatureBase()
        {
            var factory = new HmacSha1SignatureFactory();
            var actual = await factory.GetSignatureBase(
                this.PostMessage(),
                this.config,
                "kYjzVBB8Y0ZFabxSWbWovY3uYSQ2pTgmZeNu2VS4cg",
                "1318622958",
                "1.0");

            Assert.Equal("POST&https%3A%2F%2Fapi.twitter.com%2F1%2Fstatuses%2Fupdate.json&include_entities%3Dtrue%26oauth_consumer_key%3Dxvz1evFS4wEEPTGEFPHBog%26oauth_nonce%3DkYjzVBB8Y0ZFabxSWbWovY3uYSQ2pTgmZeNu2VS4cg%26oauth_signature_method%3DHMAC-SHA1%26oauth_timestamp%3D1318622958%26oauth_token%3D370773112-GmHxMAgYyLbNEtIKZeRNFsMKPR9EyMZeS9weJAEb%26oauth_version%3D1.0%26status%3DHello%2520Ladies%2520%252B%2520Gentlemen%252C%2520a%2520signed%2520OAuth%2520request%2521", actual);
        }
    }
}
