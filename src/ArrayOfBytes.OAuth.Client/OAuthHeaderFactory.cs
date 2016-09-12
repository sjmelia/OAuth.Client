namespace ArrayOfBytes.OAuth.Client
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>
    /// Factory for creating OAuth headers for HTTP requests.
    /// </summary>
    public class OAuthHeaderFactory
    {
        private static readonly string OAuthVersion = "1.0";

        private static readonly string SEPARATOR = "\", ";

        private static readonly string PAIR = "=\"";

        private static readonly Random Random = new Random();

        private static readonly object RandLock = new object();

        private Func<string> nonceFactory;

        private Func<string> timestampFactory;

        private ISignatureFactory signatureFactory;

        private OAuthConfig config;

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuthHeaderFactory"/> class for the given config, with default options.
        /// </summary>
        /// <param name="config">The OAuth config.</param>
        public OAuthHeaderFactory(OAuthConfig config)
            : this(
                  config,
                  DefaultNonceFactory,
                  DefaultTimestampFactory,
                  new HmacSha1SignatureFactory())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuthHeaderFactory"/> class for the given config and options.
        /// </summary>
        /// <param name="config">OAuth config.</param>
        /// <param name="nonceFactory">Method of creating a nonce.</param>
        /// <param name="timestampFactory">Method of generating a timestamp.</param>
        /// <param name="signatureProvider">Method of signing the request.</param>
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

        /// <summary>
        /// Gets a string representation of the OAuth authorisation header parameter.
        /// </summary>
        /// <param name="request">The request for which to get the header.</param>
        /// <returns>The OAuth authorisation header value.</returns>
        public async Task<string> GetAuthorisationHeaderParameter(HttpRequestMessage request)
        {
            var nonce = this.nonceFactory();
            var timestamp = this.timestampFactory();
            var signature = await this.signatureFactory.Get(
                request,
                this.config,
                nonce,
                timestamp,
                OAuthVersion).ConfigureAwait(false);

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
                + PAIR + Uri.EscapeDataString(OAuthVersion)
                + "\"";
        }

        private static string DefaultNonceFactory()
        {
            lock (RandLock)
            {
                return Random.Next(0, int.MaxValue).ToString("X8");
            }
        }

        private static string DefaultTimestampFactory()
        {
            return DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
        }
    }
}
