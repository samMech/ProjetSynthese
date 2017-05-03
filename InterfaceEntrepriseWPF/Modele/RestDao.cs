using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProjetRDV247.Modele;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceEntrepriseWPF.Modele
{
    /// <summary>
    /// Classe DAO pour utiliser le service web
    /// </summary>
    static class RestDao
    {
        /// <summary>
        /// Méthode pour authentifier un employe auprès du service web
        /// </summary>
        /// <param name="login">Le login de l'employé</param>
        /// <param name="password">Le mot de passe de l'employé</param>
        /// <returns>L'employé connecté ou null si inexistant</returns>
        public static Employe ConnexionEmploye(string sLogin, string sPassword)
        {
            // Création du client rest
            RestClient rc = new RestClient("http://localhost:2057/Controle/ServiceConnexion.svc/AuthentifierEmp", HttpVerb.POST);

            // Ajout des paramètres
            string parametres = JsonConvert.SerializeObject(new { login = sLogin, password = sPassword });
            rc.PostData = parametres;

            // Récupération de la réponse
            string response = rc.MakeRequest();
            JObject jObj = (JObject) JsonConvert.DeserializeObject(response);
                       
            // Récupération de l'employé connecté
            Employe emp = jObj["AuthentifierEmpResult"].ToObject<Employe>();
            
            return emp;
        }

    }
}
