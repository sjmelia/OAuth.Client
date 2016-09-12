namespace ArrayOfBytes.OAuth.Client
{
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for factories creating signatures for HTTP request.
    /// </summary>
    public interface ISignatureFactory
    {
        /// <summary>
        /// Gets the method used to generate the signature.
        /// </summary>
        string SignatureMethod { get; }

        /// <summary>
        /// Gets a signature for a given request and config.
        /// </summary>
        /// <param name="request">The HTTP request to sign.</param>
        /// <param name="config">OAuth config information.</param>
        /// <param name="nonce">The nonce for this request.</param>
        /// <param name="timestamp">The timestamp for this request.</param>
        /// <param name="version">The OAuth version for this request.</param>
        /// <returns>A signature for this request.</returns>
        Task<string> Get(
            HttpRequestMessage request,
            OAuthConfig config,
            string nonce,
            string timestamp,
            string version);
    }
}
