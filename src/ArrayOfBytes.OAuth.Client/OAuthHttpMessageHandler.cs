namespace ArrayOfBytes.OAuth.Client
{
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Message handler for authorising against OAuth services.
    /// </summary>
    public class OAuthHttpMessageHandler : DelegatingHandler
    {
        private OAuthHeaderFactory headerFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuthHttpMessageHandler"/> class,
        /// with the default delegate handler
        /// </summary>
        /// <param name="config">OAuth config.</param>
        public OAuthHttpMessageHandler(OAuthConfig config)
            : this(config, new HttpClientHandler())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuthHttpMessageHandler"/> class,
        /// with the given delegate handler.
        /// </summary>
        /// <param name="config">OAuth config.</param>
        /// <param name="innerHandler">The handler to delegate to.</param>
        public OAuthHttpMessageHandler(OAuthConfig config, HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
            this.headerFactory = new OAuthHeaderFactory(config);
        }

        /// <summary>
        /// Send the given request; adding an OAuth authorisation header to it.
        /// </summary>
        /// <param name="request">The request to handle.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The response.</returns>
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await request.AddOAuthHeader(this.headerFactory);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
