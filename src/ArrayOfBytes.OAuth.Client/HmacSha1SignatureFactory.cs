﻿using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ArrayOfBytes.OAuth.Client
{
    public class HmacSha1SignatureFactory : ISignatureFactory
    {
        public string SignatureMethod
        {
            get
            {
                return "HMAC-SHA1";
            }
        }

        /// <summary>
        /// Format the given parameters into an OAuth 1.0 signature string.
        /// </summary>
        /// <param name="request">The HTTP request to be signed.</param>
        /// <param name="config">OAuth config information.</param>
        /// <param name="nonce">The nonce for this request.</param>
        /// <param name="timestamp">The timestamp for this request.</param>
        /// <param name="version">The OAuth version string.</param>
        /// <returns>OAuth signature string.</returns>
        public async Task<string> Get(HttpRequestMessage request,
            OAuthConfig config,
            string nonce,
            string timestamp,
            string version)
        {
            var signingKey = Uri.EscapeDataString(config.ConsumerSecret)
                + "&" + Uri.EscapeDataString(config.AccessSecret);
            HMACSHA1 hmac = new HMACSHA1(Encoding.UTF8.GetBytes(signingKey));

            var signatureBase = await this.GetSignatureBase(
                request,
                config,
                nonce,
                timestamp,
                version);
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(signatureBase));
            return Convert.ToBase64String(hash);
        }
        
        public async Task<string> GetSignatureBase(HttpRequestMessage request,
            OAuthConfig config,
            string nonce,
            string timestamp,
            string version)
        {
            var parameterBase = await this.GetParameterBase(request,
                config,
                nonce,
                timestamp,
                version);

            var url = request.RequestUri.GetComponents(
                UriComponents.Scheme 
                | UriComponents.UserInfo 
                | UriComponents.Host 
                | UriComponents.Port 
                | UriComponents.Path,
                UriFormat.Unescaped);
            
            return request.Method.ToString().ToUpper()
                + "&" + Uri.EscapeDataString(url)
                + "&" + Uri.EscapeDataString(parameterBase);
        }

        public async Task<string> GetParameterBase(HttpRequestMessage request,
            OAuthConfig config,
            string nonce,
            string timestamp,
            string version)
        {
            Dictionary<string, StringValues> parameters = new Dictionary<string, StringValues>();
            if (request.Content.Headers.ContentType.MediaType == "application/x-www-form-urlencoded")
            {
                var content = await request.Content.ReadAsStringAsync();
                var fr = new FormReader(content);
                parameters.Merge(await fr.ReadFormAsync());
            }

            var queryParams = QueryHelpers.ParseQuery(request.RequestUri.Query);
            parameters.Merge(queryParams);

            parameters.Add("oauth_consumer_key", config.ConsumerKey);
            parameters.Add("oauth_nonce", nonce);
            parameters.Add("oauth_signature_method", this.SignatureMethod);
            parameters.Add("oauth_timestamp", timestamp);
            parameters.Add("oauth_token", config.AccessToken);
            parameters.Add("oauth_version", version);
            
            string parameterBase = string.Empty;
            foreach (var param in parameters.OrderBy(pair => pair.Key))
            {
                foreach (var value in param.Value.OrderBy(v => v))
                {
                    if (parameterBase.Length > 0)
                    {
                        parameterBase += "&";
                    }

                    parameterBase += Uri.EscapeDataString(param.Key)
                        + "=" + Uri.EscapeDataString(value);
                }
            }

            return parameterBase;
        }
    }
}