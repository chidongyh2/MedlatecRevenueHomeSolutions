using IdentityModel.AspNetCore.OAuth2Introspection;
using Microsoft.AspNetCore.Http;
using System;

namespace Medlatec.Revenue.Infrustructure.Contants
{
    public class CustomTokenRetriever
    {
        public static Func<HttpRequest, string> FromHeaderAndQueryString(string headerScheme = "Bearer", string queryScheme = "access_token")
        {
            return (request) =>
            {
                var token = TokenRetrieval.FromAuthorizationHeader(headerScheme)(request);
                return !string.IsNullOrEmpty(token) ? token : TokenRetrieval.FromQueryString(queryScheme)(request);
            };
        }
    }
}
