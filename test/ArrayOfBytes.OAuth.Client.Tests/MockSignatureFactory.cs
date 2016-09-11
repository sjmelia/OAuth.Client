using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ArrayOfBytes.OAuth.Client.Tests
{
    public class MockSignatureFactory : ISignatureFactory
    {
        public string SignatureMethod
        {
            get
            {
                return "HMAC-SHA1";
            }
        }

        public Task<string> Get(HttpRequestMessage request,
            OAuthConfig config,
            string nonce,
            string timestamp,
            string version)
        {
            return Task.Run(() => "tnnArxj06cWHq44gCs1OSKk/jLY=");
        }
    }
}
