namespace ArrayOfBytes.OAuth.Client
{
    /// <summary>
    /// Config information for use when making OAuth requests.
    /// </summary>
    /// <remarks>
    /// OAuth config information can be obtained from the service provider;
    /// for example by creating a Twitter app.
    /// </remarks>
    public class OAuthConfig
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OAuthConfig"/> class.
        /// </summary>
        /// <param name="consumerKey">The consumer key.</param>
        /// <param name="consumerSecret">The consumer secret.</param>
        /// <param name="accessToken">The access token.</param>
        /// <param name="accessSecret">The access secret.</param>
        public OAuthConfig(
            string consumerKey,
            string consumerSecret,
            string accessToken,
            string accessSecret)
        {
            this.ConsumerKey = consumerKey;
            this.ConsumerSecret = consumerSecret;
            this.AccessToken = accessToken;
            this.AccessSecret = accessSecret;
        }

        /// <summary>
        /// Gets the consumer key.
        /// </summary>
        public string ConsumerKey { get; }

        /// <summary>
        /// Gets the consumer secret.
        /// </summary>
        public string ConsumerSecret { get; }

        /// <summary>
        /// Gets the access token.
        /// </summary>
        public string AccessToken { get; }

        /// <summary>
        /// Gets the access secret.
        /// </summary>
        public string AccessSecret { get; }
    }
}
