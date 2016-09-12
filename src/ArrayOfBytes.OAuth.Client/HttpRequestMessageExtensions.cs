namespace ArrayOfBytes.OAuth.Client
{
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    /// <summary>
    /// Extension methods for adding OAuth header to HttpRequestMessages.
    /// </summary>
    public static class HttpRequestMessageExtensions
    {
        /// <summary>
        /// Add an OAuth header to this request.
        /// </summary>
        /// <param name="request">The request to which the header should be added.</param>
        /// <param name="headerFactory">OAuth header factory.</param>
        /// <returns>Async task</returns>
        public static async Task AddOAuthHeader(this HttpRequestMessage request, OAuthHeaderFactory headerFactory)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("OAuth", await headerFactory.GetAuthorisationHeaderParameter(request));
        }
    }
}
