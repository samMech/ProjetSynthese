using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceClientWPF.Utilitaire;
using Newtonsoft.Json;
using ProjetRDV247.Modele;

namespace InterfaceClientWPF.Model
{
    static class RestDao
    {

        public static Client ConnexionClient(string slogin, string spassword)
        {
            RestClient rc = new RestClient("http://localhost:2057/Controle/ServiceConnexion.svc/AuthentifierClient", HttpVerb.POST);
            rc.PostData = JsonConvert.SerializeObject(new {login = slogin, password = spassword});

            string response = rc.MakeRequest();
            return JsonUtil.DeserialiserJson<Client>(response, "AuthentifierClientResult");
        }

        public static List<Rendezvous> GetRendezvousClient(int id_client)
        {
            RestClient rc = new RestClient("http://localhost:2057/Controle/ServiceRDV247.svc/GetRDVClient", HttpVerb.GET);

            rc.RestURL += String.Format("/{0}", id_client);
            //rc.RestURL += String.Format("/{0}", DateTime.Today.ToString("yyyyMMdd"));

            string response = rc.MakeRequest();
            return JsonUtil.DeserialiserListeJson<Rendezvous>(response, "GetRDVClientResult");
        }

        public static bool EnregistrerClient(string sNom, string sPrenom, string sTel, string sCourriel,
            string sPassword)
        {
            RestClient rc = new RestClient("http://localhost:2057/Controle/ServiceConnexion.svc/CreerClient", HttpVerb.POST);

            rc.PostData = JsonConvert.SerializeObject(new
            {
                nom_client = sNom,
                prenom_client = sPrenom,
                telephone_client = sTel,
                courriel_client = sCourriel,
                password_client = sPassword
            });

            string response = rc.MakeRequest();
            return JsonUtil.DeserialiserJson<bool>(response, "CreerClientResult");
        }

        public static List<Employe> GetEmployes()
        {
            RestClient rc = new RestClient("http://localhost:2057/Controle/ServiceRDV247.svc/GetEmployes", HttpVerb.GET);
            string response = rc.MakeRequest();
            return JsonUtil.DeserialiserListeJson<Employe>(response, "GetEmployesResult");
        }

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

        public static List<Rendezvous> GetDisposRDVEmploye(int id_employe, int id_client)
        {
            // Création du client rest
            RestClient rc = new RestClient("http://localhost:2057/Controle/ServiceRDV247.svc/GetDisposRDVEmploye", HttpVerb.GET);

            // Ajout des paramètres GET
            rc.RestURL += String.Format("/{0}", id_employe);
            rc.RestURL += String.Format("/{0}", id_client);
            rc.RestURL += String.Format("/{0}", DateTime.Today.ToString("yyyyMMdd"));

            // Récupération de la réponse
            string response = rc.MakeRequest();
            return JsonUtil.DeserialiserListeJson<Rendezvous>(response, "GetDisposRDVEmployeResult");
        }

        public static bool EnregistrerRdv(int id_client, int id_rdv)
        {
            //Creation du client rest
            RestClient rc = new RestClient("http://localhost:2057/Controle/ServiceRDV247.svc/PrendreRDV", HttpVerb.POST);

            rc.PostData = JsonConvert.SerializeObject(new
            {
                idClient = id_client,
                idRDV = id_rdv
            });

            // Récupération de la réponse
            string response = rc.MakeRequest();
            return JsonUtil.DeserialiserJson<bool>(response, "PrendreRDVResult");
        }

        public static bool AnnulerRdv(int id_client, int id_rdv)
        {
            //Creation du client rest
            RestClient rc = new RestClient("http://localhost:2057/Controle/ServiceRDV247.svc/AnnulerRDV", HttpVerb.POST);

            rc.PostData = JsonConvert.SerializeObject(new
            {
                idClient = id_client,
                idRDV = id_rdv
            });

            // Récupération de la réponse
            string response = rc.MakeRequest();
            return JsonUtil.DeserialiserJson<bool>(response, "AnnulerRDVResult");
        }
    }
}
