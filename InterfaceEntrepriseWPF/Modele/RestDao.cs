using InterfaceEntrepriseWPF.Utilitaire;
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

            // Ajout des paramètres POST
            rc.PostData = JsonConvert.SerializeObject(new { login = sLogin, password = sPassword });

            // Récupération de la réponse
            string response = rc.MakeRequest();
            return JsonUtil.DeserialiserJson<Employe>(response, "AuthentifierEmpResult");            
        }

        /// <summary>
        /// Méthode pour récupérer la liste des rendez-vous de l'employé
        /// </summary>
        /// <param name="id_employe">L'identifiant de l'employé</param>
        /// <returns>La liste des rendez-vous de l'employé</returns>
        public static List<Rendezvous> GetRendezVousEmploye(int id_employe)
        {
            // Création du client rest
            RestClient rc = new RestClient("http://localhost:2057/Controle/ServiceRDV247.svc/GetRDVEmploye", HttpVerb.GET);

            // Ajout des paramètres GET
            rc.RestURL += String.Format("/{0}", id_employe);
            rc.RestURL += String.Format("/{0}", DateTime.Today.ToString("yyyyMMdd"));

            Console.WriteLine(rc.RestURL);

            // Récupération de la réponse
            string response = rc.MakeRequest();
            return JsonUtil.DeserialiserListeJson<Rendezvous>(response, "GetRDVEmployeResult");
        }
    }
}
