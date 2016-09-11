using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ArrayOfBytes.OAuth.Client
{
    public class OAuthHeaderFactory
    {
        private static string OAUTH_VERSION = "1.0";

        private static string SEPARATOR = "\", ";

        private static string PAIR = "=\"";

        private Func<string> nonceFactory;

        private Func<string> timestampFactory;

        private ISignatureFactory signatureFactory;

        private OAuthConfig config;

        private static Random random = new Random();

        private static object randLock = new object();

        public OAuthHeaderFactory(OAuthConfig config)
            : this(config,
                  DefaultNonceFactory,
                  DefaultTimestampFactory,
                  new HmacSha1SignatureFactory())
        {
        }

        public OAuthHeaderFactory(
            OAuthConfig config,
            Func<string> nonceFactory,
            Func<string> timestampFactory,
            ISignatureFactory signatureProvider)
        {
            this.config = config;
            this.nonceFactory = nonceFactory;
            this.timestampFactory = timestampFactory;
            this.signatureFactory = signatureProvider;
        }

        private static string DefaultNonceFactory()
        {
            lock (randLock)
            {
                return random.Next(0, Int32.MaxValue).ToString("X8");
            }
        }

        private static string DefaultTimestampFactory()
        {
            return DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
        }

        public async Task<string> GetAuthorisationHeaderParameter(HttpRequestMessage request)
        {
            var nonce = this.nonceFactory();
            var timestamp = this.timestampFactory();
            var signature = await this.signatureFactory.Get(
                request,
                this.config,
                nonce,
                timestamp,
                OAUTH_VERSION);

            return Uri.EscapeDataString("oauth_consumer_key")
                + PAIR + Uri.EscapeDataString(this.config.ConsumerKey)
                + SEPARATOR + Uri.EscapeDataString("oauth_nonce")
                + PAIR + Uri.EscapeDataString(nonce)
                + SEPARATOR + Uri.EscapeDataString("oauth_signature")
                + PAIR + Uri.EscapeDataString(signature)
                + SEPARATOR + Uri.EscapeDataString("oauth_signature_method")
                + PAIR + Uri.EscapeDataString(this.signatureFactory.SignatureMethod)
                + SEPARATOR + Uri.EscapeDataString("oauth_timestamp")
                + PAIR + Uri.EscapeDataString(timestamp)
                + SEPARATOR + Uri.EscapeDataString("oauth_token")
                + PAIR + Uri.EscapeDataString(this.config.AccessToken)
                + SEPARATOR + Uri.EscapeDataString("oauth_version")
                + PAIR + Uri.EscapeDataString(OAUTH_VERSION)
                + "\"";
        }
    }
}
