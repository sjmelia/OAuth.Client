using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ArrayOfBytes.OAuth.Client
{
    /// <summary>
    /// Message handler for authorising against OAuth services.
    /// </summary>
    public class OAuthHttpMessageHandler : DelegatingHandler
    {
        private OAuthHeaderFactory headerFactory;
        
        public OAuthHttpMessageHandler(OAuthConfig config)
            : this(config, new HttpClientHandler())
        {
        }

        public OAuthHttpMessageHandler(OAuthConfig config, HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
            this.headerFactory = new OAuthHeaderFactory(config);
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await request.AddOAuthHeader(this.headerFactory);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
