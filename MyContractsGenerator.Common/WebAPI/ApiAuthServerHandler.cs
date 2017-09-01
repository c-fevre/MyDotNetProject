//------------------------------------------------------------------------------
// <summary>
//    APPLICATION : MyContractsGenerator
//    Author : Clement Fevre
//    Description : Handler Serveur pour l'authentification
// </summary>
//------------------------------------------------------------------------------
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MyContractsGenerator.Common.WebAPI
{
    public class ApiAuthServerHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            bool requestIsValid = ApiAuthorization.CheckRequest(request, this.GetClientKey);

            if (!requestIsValid)
            {
                // Send HTTP 403 Forbidden
                var response = new HttpResponseMessage(HttpStatusCode.Forbidden);
                var tsc = new TaskCompletionSource<HttpResponseMessage>();
                tsc.SetResult(response);
                return await tsc.Task;
            }

            return await base.SendAsync(request, cancellationToken);
        }

        private byte[] GetClientKey(string clientIdentifier)
        {
            switch (clientIdentifier)
            {
                case "theid": return new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
                default: return null;
            }
        }
    }
}
