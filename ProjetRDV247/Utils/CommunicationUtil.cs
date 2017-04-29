using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjetRDV247.Modele;

namespace ProjetRDV247.Utils
{
    /// <summary>
    /// Classe utilitaire pour les méthodes de communication
    /// </summary>
    public class CommunicationUtil
    {
        /// <summary>
        /// Méthode pour notifier un client de l'annulation d'un rendez-vous
        /// </summary>
        /// <param name="client">Le client concerné par l'annulation</param>
        /// <param name="rdv">Le rendez-vous annulé</param>
        /// <param name="emp">L'employé concerné par l'annulation</param>
        /// <param name="raison">La raison de l'annulation</param>
        public static void NotifierAnnulationRDV(Client client, Rendezvous rdv, Employe emp, string raison)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Méthode pour notifier un client du changement d'un rendez-vous
        /// </summary>
        /// <param name="client">Le client concerné par le changement</param>
        /// <param name="rdv">Le rendez-vous original</param>
        /// <param name="rdv">Le rendez-vous modifié</param>
        /// <param name="emp">L'employé concerné par le changement</param>
        /// <param name="raison">La raison du changement</param>        
        public static void NotifierChangementRDV(Client client, Rendezvous rdv, Rendezvous rdvModifie, Employe emp, string raison)
        {
            throw new NotImplementedException();
        }
    }
}