using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceEntrepriseWPF.Utilitaire
{
    /// <summary>
    /// Classe utilitaire pour les méthodes avec Json
    /// </summary>
    static class JsonUtil
    {

        /// <summary>
        /// Méthode pour retourner un objet à partir de données Json
        /// </summary>
        /// <typeparam name="T">Le type de l'objet</typeparam>
        /// <param name="json">Les données en format json</param>
        /// <param name="parent">Le nom de l'élment parent de l'objet dans le json</param>
        /// <returns>L'objet désérialisé</returns>
        public static T DeserialiserJson<T>(string json, string parent){

            // Désérialisation des données json
            JObject jObj = (JObject)JsonConvert.DeserializeObject(json);

            Console.WriteLine(jObj);

            // Récupération de l'objet recherché
            T obj = default(T);
            if (jObj[parent] != null)
            {
                obj = jObj[parent].ToObject<T>();
            }

            return obj;
        }

        /// <summary>
        /// Méthode pour retourner une liste d'objet à partir de données Json
        /// </summary>
        /// <typeparam name="T">Le type de l'objet</typeparam>
        /// <param name="json">Les données en format json</param>
        /// <param name="parent">Le nom de l'élment parent de la liste dans le json</param>
        /// <returns>L'objet désérialisé</returns>
        public static List<T> DeserialiserListeJson<T>(string json, string parent)
        {
            // Désérialisation des données json
            JObject jObj = (JObject)JsonConvert.DeserializeObject(json);
            
            Console.WriteLine(jObj);
            
            // Récupération de la liste des objets
            List<T> listeObj = new List<T>();
            foreach (JToken jt in jObj[parent].Children())
            {
                listeObj.Add(jt.ToObject<T>());
            }

            return listeObj;
        }

    }
}
