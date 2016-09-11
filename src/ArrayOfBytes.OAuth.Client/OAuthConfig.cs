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
        public OAuthConfig(string consumerKey,
            string consumerSecret,
            string accessToken,
            string accessSecret)
        {
            this.ConsumerKey = consumerKey;
            this.ConsumerSecret = consumerSecret;
            this.AccessToken = accessToken;
            this.AccessSecret = accessSecret;
        }

        public string ConsumerKey { get; }

        public string ConsumerSecret { get; }

        public string AccessToken { get; }

        public string AccessSecret { get; }
    }
}
