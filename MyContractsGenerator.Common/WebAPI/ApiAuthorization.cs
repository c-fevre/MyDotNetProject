//------------------------------------------------------------------------------
// <summary>
//    APPLICATION : MyContractsGenerator
//    Author : Clement Fevre
//    Description : Contient la gestion de l'authentification des requêtes faites entre le client et le serveur de la WebAPI
// </summary>
//------------------------------------------------------------------------------
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace MyContractsGenerator.Common.WebAPI
{
    /// <summary>
    /// Contient la gestion de l'authentification des requêtes faites entre le client et le serveur de la WebAPI
    /// </summary>
    public static class ApiAuthorization
    {
        /// <summary>
        /// Préfixe de l'entête HTTP Authorization
        /// </summary>
        private const string AuthScheme = "ApiAuthHmac256";

        #region AuthHeaderData

        /// <summary>
        /// Contient les informations de l'entête HTTP Authorize sous forme structurée
        /// </summary>
        private class AuthHeaderData
        {
            // L'entête HTTP complet est construit de cette manière :
            // Authorization: ApiAuthHmac256 {clientid} {timestamp} {nonce} {HMACSHA256(clientid || method || uri || timestamp || nonce || content, key)}

            /// <summary>
            /// Identifiant du client
            /// </summary>
            public string ClientIdentifier
            {
                get;
                set;
            }

            /// <summary>
            /// Date Utc
            /// </summary>
            public DateTime UtcTimeStamp
            {
                get;
                set;
            }

            /// <summary>
            /// Nonce property
            /// </summary>
            public byte[] Nonce
            {
                get;
                set;
            }

            /// <summary>
            /// Signature
            /// </summary>
            public byte[] Signature
            {
                get;
                set;
            }

            /// <summary>
            /// Crée un AuthHeaderData à partir de la valeur brute recue dans une requête HTTP
            /// </summary>
            /// <param name="header">Valeur de l'entête</param>
            /// <returns>Donnée du header</returns>
            public static AuthHeaderData FromHeader(AuthenticationHeaderValue header)
            {
                if (header == null ||
                    header.Scheme != AuthScheme ||
                    header.Parameter == null)
                    return null;

                var components = header.Parameter.Split(' ');
                if (components.Length != 4)
                    return null;

                return new AuthHeaderData
                {
                    ClientIdentifier = WebUtility.UrlDecode(components[0]),
                    UtcTimeStamp = DateTime.ParseExact(WebUtility.UrlDecode(components[1]), "o", CultureInfo.InvariantCulture),
                    Nonce = Convert.FromBase64String(components[2]),
                    Signature = Convert.FromBase64String(components[3]),
                };
            }

            /// <summary>
            /// Convertit l'instance de AuthHeaderData en un header HTTP utilisable pour inclusion dans une réponse HTTP
            /// </summary>
            /// <returns></returns>
            public AuthenticationHeaderValue ToHeader()
            {
                return new AuthenticationHeaderValue(
                    AuthScheme,
                    string.Format(
                        "{0} {1} {2} {3}",
                        WebUtility.UrlEncode(this.ClientIdentifier),
                        WebUtility.UrlEncode(this.UtcTimeStamp.ToString("o")),
                        Convert.ToBase64String(this.Nonce),
                        Convert.ToBase64String(this.Signature)));
            }
        }

        #endregion

        /// <summary>
        /// Signe la requête passée en paramètre.
        /// L'entête est rajouté directement dans la requête.
        /// </summary>
        /// <param name="clientIdentifier">L'identifiant unique du client</param>
        /// <param name="clientKey">La clé secrète associée à l'identifiant du client</param>
        /// <param name="request">La requête à signer</param>
        public static void SignRequest(string clientIdentifier, byte[] clientKey, HttpRequestMessage request)
        {
            var authData = new AuthHeaderData
            {
                ClientIdentifier = clientIdentifier,
                UtcTimeStamp = DateTime.UtcNow,
                Nonce = GenerateNonce(),
                Signature = ComputeHash(clientIdentifier, clientKey, request),
            };

            request.Headers.Authorization = authData.ToHeader();
        }

        /// <summary>
        /// Vérifie que la requête passée en paramètre contient un header Authorize valide.
        /// </summary>
        /// <param name="request">La requête à vérifier</param>
        /// <param name="getClientKey">Fonction qui renvoie la clé secrète à partir d'un identifiant de client</param>
        /// <returns>Vrai si la signature est conforme</returns>
        public static bool CheckRequest(HttpRequestMessage request, Func<string, byte[]> getClientKey)
        {
            var authData = AuthHeaderData.FromHeader(request.Headers.Authorization);
            if (authData == null)
            {
                return false;
            }

            return CheckSignature(request, authData, getClientKey(authData.ClientIdentifier));
        }

        /// <summary>
        /// Vérifie que la requête passée en paramètre contient un header Authorize valide.
        /// </summary>
        /// <param name="request">La requête à vérifier</param>
        /// <param name="clientIdentifier">L'identifiant du client attendu dans la requête</param>
        /// <param name="clientKey">La clé secrète qui correspond à clientIdentifier</param>
        /// <returns>Vrai si la signature est conforme</returns>
        public static bool CheckRequest(HttpRequestMessage request, string clientIdentifier, byte[] clientKey)
        {
            var authData = AuthHeaderData.FromHeader(request.Headers.Authorization);
            if (authData == null)
                return false;

            if (authData.ClientIdentifier != clientIdentifier)
                return false;

            return CheckSignature(request, authData, clientKey);
        }

        private static bool CheckSignature(HttpRequestMessage request, AuthHeaderData authData, byte[] clientKey)
        {
            byte[] expectedHashValue = ComputeHash(authData.ClientIdentifier, clientKey, request);

            if (!expectedHashValue.SequenceEqual(authData.Signature))
                return false;

            // TODO : voir si ca vaut le coup de stocker et comparer les nonce et timestamp pour éviter les replay attacks.
            // Si on force le HTTPS pour le webapi, ce genre d'attaque est mitigé.

            return true;
        }

        /// <summary>
        /// Récupère les infos à hasher.
        /// Les infos sont : clientIdentifier || request.method || request.uri || timestamp || nonce || request.content
        /// </summary>
        private static byte[] GetMessageToSign(string clientIdentifier, HttpRequestMessage request)
        {
            using (var memStream = new MemoryStream())
            using (var w = new StreamWriter(memStream, Encoding.UTF8))
            {
                w.Write(clientIdentifier);
                w.Write(request.Method.Method);
                w.Write(request.RequestUri.OriginalString);
                w.Write(DateTime.UtcNow.ToString("o"));
                w.Write("someawesomenonce");
                if (request.Content != null)
                    w.Write(request.Content.ReadAsByteArrayAsync().Result);
                return memStream.ToArray();
            }
        }

        /// <summary>
        /// Calcule le HMAC de la requête passée en paramètre
        /// </summary>
        private static byte[] ComputeHash(string clientIdentifier, byte[] clientKey, HttpRequestMessage request)
        {
            using (HMACSHA256 hmac = new HMACSHA256(clientKey))
            {
                byte[] messageToSign = GetMessageToSign(clientIdentifier, request);
                return hmac.ComputeHash(messageToSign);
            }
        }

        /// <summary>
        /// Génère un nonce.
        /// </summary>
        private static byte[] GenerateNonce()
        {
            var result = new byte[16];
            using (var prng = new RNGCryptoServiceProvider())
            {
                prng.GetBytes(result);
            }
            return result;
        }
    }
}
