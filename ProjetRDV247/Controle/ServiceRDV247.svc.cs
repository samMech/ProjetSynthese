using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ProjetRDV247.Modele;
using ProjetRDV247.Utils;

namespace ProjetRDV247.Controle
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "ServiceRDV247" à la fois dans le code, le fichier svc et le fichier de configuration.
    // REMARQUE : pour lancer le client test WCF afin de tester ce service, sélectionnez ServiceRDV247.svc ou ServiceRDV247.svc.cs dans l'Explorateur de solutions et démarrez le débogage.
    public class ServiceRDV247 : IServiceRDV247
    {
        // Le data entity
        private Dao dao;
        
        public string TestREST()
        {
            return "Hello World !";
        }

        //============================================================================================================================

        /// <summary>
        /// Ajoute des disponibilités pour un employé
        /// </summary>
        /// <param name="idEmploye">L'identifiant de l'employé</param>
        /// <param name="debut">Le début de la plage horaire d'insertion</param>
        /// <param name="fin">La fin de la plage horaire d'insertion</param>
        /// <param name="dureeDispo">La durée d'une disponibilité</param>
        /// <param name="idType">Le id du type de disponibilité</param>
        /// <returns>La liste des disponibilités ajoutées</returns>
        public List<Rendezvous> AjouterDispo(int idEmploye, DateTime debut, DateTime fin, TimeSpan dureeDispo, int idType)
        {
            // Création de l'objet d'accès aux données
            dao = new Dao();
            
            // Création de la liste des nouvelles disponibilités
            List<Rendezvous> dispoAjoutees = new List<Rendezvous>();
            if(debut.Date == fin.Date)
            {
                // Récupération des disponibilités existantes de l'employé
                List<Rendezvous> dispoExistantes = dao.GetDisposEmploye(idEmploye, debut.Date);
                
                // Création des disponibilités (TODO: prendre le statut prédéfini dans la BD ???)
                dispoAjoutees = HoraireUtil.CreerDispos(debut, fin, dureeDispo, dispoExistantes, idEmploye, idType, "LIBRE");
            }

            // Sauvegarde des nouvelles disponibilités
            dao.InsertListeDispo(dispoAjoutees);

            // TODO (vérifier si les idRDV ont été synchronisés
            return dispoAjoutees;
        }

        public bool SupprimerDispo(int idEmploye, Rendezvous dispo, string raison)
        {
            throw new NotImplementedException();
        }              

        public bool AnnulerRDV(Client client, Rendezvous rdv)
        {
            throw new NotImplementedException();
        }

        public List<Rendezvous> GetDispoEmploye(int idEmploye, DateTime date)
        {
            throw new NotImplementedException();
        }

        public List<Employe> GetEmployes()
        {
            throw new NotImplementedException();
        }

        public List<Rendezvous> GetRDVClient(int idClient)
        {
            throw new NotImplementedException();
        }

        public List<Rendezvous> GetRDVEmploye(int idEmploye, DateTime date)
        {
            throw new NotImplementedException();
        }

        public Rendezvous ModifierDispo(int idEmploye, Rendezvous dispo, DateTime newDebut, DateTime newFin, TimeSpan newDureeRDV, int idType, string raison)
        {
            throw new NotImplementedException();
        }

        public Rendezvous PrendreRDV(Client client, Rendezvous rdv)
        {
            throw new NotImplementedException();
        }

    }
}
