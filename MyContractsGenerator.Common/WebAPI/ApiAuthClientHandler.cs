//------------------------------------------------------------------------------
// <summary>
//    APPLICATION : MyContractsGenerator
//    Author : Clement Fevre
//    Description : Handler Client pour l'authentification
// </summary>
//------------------------------------------------------------------------------
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MyContractsGenerator.Common.WebAPI
{
    public class ApiAuthClientHandler : DelegatingHandler
    {
        private readonly string clientIdentifier;
        private readonly byte[] clientKey;

        public ApiAuthClientHandler(string clientIdentifier, byte[] clientKey)
        {
            this.clientIdentifier = clientIdentifier;
            this.clientKey = clientKey;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            ApiAuthorization.SignRequest(this.clientIdentifier, this.clientKey, request);
            return base.SendAsync(request, cancellationToken);
        }
    }
}
