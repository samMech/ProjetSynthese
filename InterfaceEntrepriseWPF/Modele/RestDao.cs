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
using System.Configuration;

namespace InterfaceEntrepriseWPF.Modele
{
    /// <summary>
    /// Classe DAO pour utiliser le service web
    /// </summary>
    static class RestDao
    {
        // Constante
        private static readonly string webServiceURL = ConfigurationManager.AppSettings["webServiceURL"];

        /// <summary>
        /// Méthode pour authentifier un employe auprès du service web
        /// </summary>
        /// <param name="loginParam">Le login de l'employé</param>
        /// <param name="passwordParam">Le mot de passe de l'employé</param>
        /// <returns>L'employé connecté ou null si inexistant</returns>
        public static Employe ConnexionEmploye(string loginParam, string passwordParam)
        {
            // Création du client rest            
            RestClient rc = new RestClient(webServiceURL + "/Controle/ServiceConnexion.svc/AuthentifierEmp", HttpVerb.POST);

            // Ajout des paramètres POST
            rc.PostData = JsonConvert.SerializeObject(new { login = loginParam, password = passwordParam });

            // Récupération de la réponse
            string response = rc.MakeRequest();
            return JsonUtil.DeserialiserJson<Employe>(response, "AuthentifierEmpResult");            
        }

        /// <summary>
        /// Méthode pour récupérer la liste des disponibilités de l'employé pour une semaine
        /// </summary>
        /// <param name="id_employe">L'identifiant de l'employé</param>
        /// <param name="dateJourSemaine">Une date d'un jour dans la semaine courante</param>
        /// <returns>La liste des disponibilités de l'employé</returns>
        public static List<Rendezvous> GetDisposEmploye(int id_employe, DateTime dateJourSemaine)
        {
            // Création du client rest
            RestClient rc = new RestClient(webServiceURL + "/Controle/ServiceRDV247.svc/GetDisposEmploye", HttpVerb.GET);

            // Ajout des paramètres GET
            rc.RestURL += String.Format("/{0}", id_employe);
            rc.RestURL += String.Format("/{0}", dateJourSemaine.ToString("yyyyMMdd"));

            // Récupération de la réponse
            string response = rc.MakeRequest();
            return JsonUtil.DeserialiserListeJson<Rendezvous>(response, "GetDisposEmployeResult");
        }

        /// <summary>
        /// Méthode pour récupérer la liste des rendez-vous pours une semaine
        /// </summary>
        /// <param name="id_employe">L'identifiant de l'employé</param>
        /// <param name="dateJourSemaine">Une date d'un jour dans la semaine courante</param>
        /// <returns>La liste des rendez-vous de l'employé</returns>
        public static List<Rendezvous> GetRendezVousEmploye(int id_employe, DateTime dateJourSemaine)
        {
            // Création du client rest
            RestClient rc = new RestClient(webServiceURL + "/Controle/ServiceRDV247.svc/GetRDVEmploye", HttpVerb.GET);

            // Ajout des paramètres GET
            rc.RestURL += String.Format("/{0}", id_employe);
            rc.RestURL += String.Format("/{0}", dateJourSemaine.ToString("yyyyMMdd"));
            
            // Récupération de la réponse
            string response = rc.MakeRequest();
            return JsonUtil.DeserialiserListeJson<Rendezvous>(response, "GetRDVEmployeResult");
        }

        /// <summary>
        /// Méthode pour ajouter des disponibilités pour un employé
        /// </summary>
        /// <param name="idEmp">L'identifiant de l'employé concerné</param>
        /// <param name="dateDebutParam">Le début de la plage d'ajout</param>
        /// <param name="dateFinParam">La fin de la plage d'ajout</param>
        /// <param name="dureeMinutesDispoParam">La durée de chaque rendez-vous en minutes</param>
        /// <param name="idTypeParam">Le type de chaque rendez-vous</param>
        /// <returns>La liste des disponibilités ajoutées</returns>
        public static List<Rendezvous> AjouterDispos(int idEmp, DateTime dateDebutParam, DateTime dateFinParam, int dureeMinutesDispoParam, int idTypeParam)
        {
            // Création du client rest
            RestClient rc = new RestClient(webServiceURL + "/Controle/ServiceRDV247.svc/AjouterDispos", HttpVerb.POST);
            
            // Ajout des paramètres POST
            rc.PostData = JsonConvert.SerializeObject(new
            {
                idEmploye = idEmp,
                dateDebut = dateDebutParam,
                dateFin = dateFinParam,
                dureeMinutesDispo = dureeMinutesDispoParam,
                idType = idTypeParam
            }, new JsonSerializerSettings() { DateFormatHandling = DateFormatHandling.MicrosoftDateFormat });
            
            // Récupération de la réponse
            string response = rc.MakeRequest();
            return JsonUtil.DeserialiserListeJson<Rendezvous>(response, "AjouterDisposResult");
        }

        /// <summary>
        /// Méthode pour supprimer des disponibilités pour un employé
        /// </summary>
        /// <param name="idEmp">L'identifiant de l'employé concerné</param>
        /// <param name="idDisposParam">La liste des id des disponibilités à supprimer</param>
        /// <param name="raisonParam">La raison du changement s'il y a lieu</param>
        public static void SupprimerDispos(int idEmp, List<int> idDisposParam, string raisonParam)
        {
            // Création du client rest
            RestClient rc = new RestClient(webServiceURL + "/Controle/ServiceRDV247.svc/SupprimerDispos", HttpVerb.POST);

            // Ajout des paramètres POST
            rc.PostData = JsonConvert.SerializeObject(new
            {
                idEmploye = idEmp,
                idDispos = idDisposParam,
                raison = raisonParam
            });
                        
            // On lance la requête
            rc.MakeRequest();
        }

        /// <summary>
        /// Méthode pour modifier une disponibilité
        /// </summary>
        /// <param name="idEmp">L'identifiant de l'employé concerné</param>
        /// <param name="idDispoParam">L'identifiant de la disponibilité concernée</param>
        /// <param name="newDateDebut">La nouvelle heure de début</param>
        /// <param name="newDateFin">La nouvelle heure de fin</param>
        /// <param name="newIdType">Le nouveau type de rendez-vous</param>
        /// <param name="raisonParam">La raison du changement s'il y a lieu</param>
        /// <returns>La disponibilité modifiée ou null en cas de conflit</returns>
        public static Rendezvous ModifierDispo(int idEmp, int idDispoParam, DateTime newDateDebut, DateTime newDateFin, int newIdType, string raisonParam)
        {
            // Création du client rest
            RestClient rc = new RestClient(webServiceURL + "/Controle/ServiceRDV247.svc/ModifierDispo", HttpVerb.POST);

            // Ajout des paramètres POST
            rc.PostData = JsonConvert.SerializeObject(new
            {
                idEmploye = idEmp,
                idDispo = idDispoParam,
                newDebut = newDateDebut,
                newFin = newDateFin,
                idType = newIdType,
                raison = raisonParam
            }, new JsonSerializerSettings() { DateFormatHandling = DateFormatHandling.MicrosoftDateFormat });

            // Récupération de la réponse
            string response = rc.MakeRequest();
            return JsonUtil.DeserialiserJson<Rendezvous>(response, "ModifierDispoResult");
        }
    }
}
