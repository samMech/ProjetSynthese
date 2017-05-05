using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

/* <summary>
/ Source: https://www.codeproject.com/Tips/497123/How-to-make-REST-requests-with-Csharp
 </summary>*/
namespace InterfaceClientWPF.Model
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
        public string RestURL { get; set; }
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
        /// <param name="restURL">L'adresse pour accéder au service web</param>
        public RestClient(string restURL) : this()
        {
            RestURL = restURL;
        }
        
        /// <summary>
        /// Constructeur avec une adresse et la méthode Http
        /// </summary>
        /// <param name="restURL">L'adresse pour accéder au service web</param>
        /// <param name="method">La méthode d'accès</param>
        public RestClient(string restURL, HttpVerb method) : this()
        {
            RestURL = restURL;
            Method = method;
        }
        
        /// <summary>
        /// Méthode pour faire une requête selon les paramètres déjà en place dans l'url ou les données POST
        /// </summary>
        /// <returns>Les données retournées en format Json</returns>
        public string MakeRequest()
        {
            // Construction de la requête
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Uri.EscapeUriString(RestURL));
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
