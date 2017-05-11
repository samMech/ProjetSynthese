using InterfaceEntrepriseWPF.Utilitaire;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProjetRDV247.Modele;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalendrierRDV;
using System.Collections.ObjectModel;
using System.Runtime.Serialization.Json;
using System.IO;

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
        /// Méthode pour récupérer la liste des disponibilités de l'employé
        /// </summary>
        /// <param name="id_employe">L'identifiant de l'employé</param>
        /// <returns>La liste des disponibilités de l'employé</returns>
        public static List<Rendezvous> GetDisposEmploye(int id_employe)
        {
            // Création du client rest
            RestClient rc = new RestClient("http://localhost:2057/Controle/ServiceRDV247.svc/GetDisposEmploye", HttpVerb.GET);

            // Ajout des paramètres GET
            rc.RestURL += String.Format("/{0}", id_employe);
            rc.RestURL += String.Format("/{0}", DateTime.Today.ToString("yyyyMMdd"));

            // Récupération de la réponse
            string response = rc.MakeRequest();
            return JsonUtil.DeserialiserListeJson<Rendezvous>(response, "GetDisposEmployeResult");
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
            
            // Récupération de la réponse
            string response = rc.MakeRequest();
            return JsonUtil.DeserialiserListeJson<Rendezvous>(response, "GetRDVEmployeResult");
        }

        /// <summary>
        /// Méthode pour ajouter des disponibilités pour un employé
        /// </summary>
        /// <param name="idEmp">L'identifiant de l'employé concerné</param>
        /// <param name="pdateDebut">Le début de la plage d'ajout</param>
        /// <param name="pdateFin">La fin de la plage d'ajout</param>
        /// <param name="dureeMinutesRDV">La durée de chaque rendez-vous en minutes</param>
        /// <param name="idTypeRDV">Le type de chaque rendez-vous</param>
        /// <returns>La liste des disponibilités ajoutées</returns>
        public static List<Rendezvous> AjouterDispos(int idEmp, DateTime pdateDebut, DateTime pdateFin, int dureeMinutesRDV, int idTypeRDV)
        {
            // Création du client rest
            RestClient rc = new RestClient("http://localhost:2057/Controle/ServiceRDV247.svc/AjouterDispos", HttpVerb.POST);
            
            // Ajout des paramètres POST
            rc.PostData = JsonConvert.SerializeObject(new { idEmploye = idEmp,
                dateDebut = pdateDebut, dateFin = pdateFin,
                dureeMinutesDispo = dureeMinutesRDV, idType = idTypeRDV },
                new JsonSerializerSettings() { DateFormatHandling = DateFormatHandling.MicrosoftDateFormat });
            
            // Récupération de la réponse
            string response = rc.MakeRequest();
            return JsonUtil.DeserialiserListeJson<Rendezvous>(response, "AjouterDisposResult");
        }

        /// <summary>
        /// Méthode pour supprimer des disponibilités pour un employé
        /// </summary>
        /// <param name="idEmp">L'identifiant de l'employé concerné</param>
        /// <param name="idDispos">La liste des id des disponibilités à supprimer</param>
        /// <param name="raison">La raison du changement s'il y a lieu</param>
        public static void SupprimerDispos(int idEmp, List<int> listeIdDispos, string sraison)
        {
            // Création du client rest
            RestClient rc = new RestClient("http://localhost:2057/Controle/ServiceRDV247.svc/SupprimerDispos", HttpVerb.POST);

            // Ajout des paramètres POST
            rc.PostData = JsonConvert.SerializeObject(new
            {
                idEmploye = idEmp,
                idDispos = listeIdDispos,
                raison = sraison
            });
                        
            // On lance la requête
            rc.MakeRequest();
        }
    }
}
