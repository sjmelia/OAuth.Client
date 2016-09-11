using System.Net.Http;
using System.Threading.Tasks;

namespace ArrayOfBytes.OAuth.Client
{
    public interface ISignatureFactory
    {
        string SignatureMethod { get; }
        
        Task<string> Get(HttpRequestMessage request,
            OAuthConfig config,
            string nonce,
            string timestamp,
            string version);

    }
}
