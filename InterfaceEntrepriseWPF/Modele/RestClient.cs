using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Source: https://www.codeproject.com/Tips/497123/How-to-make-REST-requests-with-Csharp
/// </summary>
namespace InterfaceEntrepriseWPF.Modele
{
    /// <summary>
    /// Énumération des méthodes (verbes) Http
    /// </summary>
    public enum HttpVerb
    {
        GET,
        POST,
        PUT,
        DELETE
    }
    
    /// <summary>
    /// Classe utilitaire pour encapsuler les méthodes utilisant le service web
    /// </summary>
    public class RestClient
    {
        // Propriétés
        public string EndPoint { get; set; }
        public string PostData { get; set; }
        public HttpVerb Method { get; set; }

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        private RestClient()
        {
            Method = HttpVerb.GET;
        }

        /// <summary>
        /// Constructeur avec une adresse
        /// </summary>
        /// <param name="endpoint">L'adresse pour accéder au service web</param>
        public RestClient(string endpoint) : this()
        {
            EndPoint = endpoint;
        }
        
        /// <summary>
        /// Constructeur avec une adresse et la méthode Http
        /// </summary>
        /// <param name="endpoint">L'adresse pour accéder au service web</param>
        /// <param name="method">La méthode d'accès</param>
        public RestClient(string endpoint, HttpVerb method) : this()
        {
            EndPoint = endpoint;
            Method = method;
        }

        /// <summary>
        /// Méthode pour faire une requête sans paramètres
        /// </summary>
        /// <returns>Les données retournées en format Json</returns>
        public string MakeRequest()
        {
            return MakeRequest("");
        }

        /// <summary>
        /// Méthode pour faire une requête avec paramètres
        /// </summary>
        /// <param name="parameters">Les paramètres en format Json</param>
        /// <returns>Les données retournées en format Json</returns>
        public string MakeRequest(string parameters)
        {
            // Construction de la requête
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(EndPoint + parameters);
            request.Method = Method.ToString();
            request.ContentLength = 0;
            request.ContentType = (Method == HttpVerb.GET) ? "text/xml" : "application/json";
            request.Accept = "application/json";
            
            // Si POST avec des données
            if (!string.IsNullOrEmpty(PostData) && Method == HttpVerb.POST)
            {
                // Ajout des données POST
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] bytes = Encoding.GetEncoding(encoding.CodePage).GetBytes(PostData);
                request.ContentLength = bytes.Length;

                using (Stream writeStream = request.GetRequestStream())
                {
                    writeStream.Write(bytes, 0, bytes.Length);
                }
            }

            // Appel du service web
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                String responseValue = "";

                // Vérification du code de réponse
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    // TODO: gérer exception ???
                    string message = String.Format("Request failed. Received HTTP {0}", response.StatusCode);
                    throw new ApplicationException(message);
                }

                // Récupération de la réponse
                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            responseValue = reader.ReadToEnd();
                        }
                    }
                }

                return responseValue;
            }
        }

    }
}
