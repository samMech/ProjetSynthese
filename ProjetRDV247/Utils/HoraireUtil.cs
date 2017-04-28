using ProjetRDV247.Modele;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetRDV247.Utils
{
    /// <summary>
    /// Classe utilitaire pour les méthodes en lien avec les rendez-vous
    /// </summary>
    public class HoraireUtil
    {
        /// <summary>
        /// Détermine si un rendez-vous est en conflit avec d'autres rendez-vous
        /// </summary>
        /// <param name="rdv">Le rendez-vous à vérifier</param>
        /// <param name="listeRdvs">La liste des rendez-vous à utiliser</param>
        /// <returns>Vrai seulement si il y a un conflit</returns>
        public static bool IsRDVConflictuel(Rendezvous rdv, List<Rendezvous> listeRdvs)
        {
            return IsRDVConflictuel(rdv.debut_rdv, rdv.fin_rdv, listeRdvs);
        }

        /// <summary>
        /// Détermine si une plage horaire est en conflit avec d'autres rendez-vous
        /// </summary>
        /// <param name="debut">Le début de la plage horaire</param>
        /// <param name="fin">La fin de la plage horaire</param>
        /// <param name="listeRdvs">La liste des rendez-vous à utiliser</param>
        /// <returns>Vrai seulement si il y a un conflit</returns>
        public static bool IsRDVConflictuel(DateTime debut, DateTime fin, List<Rendezvous> listeRdvs)
        {
            foreach (Rendezvous r in listeRdvs)
            {
                // On vérifie si il y a un supperposition des plages horaires
                if (debut < r.fin_rdv && fin > r.debut_rdv)
                {
                    // Conflit
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Crée une liste de disponibilité de durée fixe à l'intérieur d'une plage horaire
        /// </summary>
        /// <param name="debut">Le début de la plage horaire</param>
        /// <param name="fin">La fin de la plage horaire</param>
        /// <param name="duree">La durée des disponibilités</param>
        /// <param name="dispoExistantes">La liste des disponibilités existantes (dans la plage horaire)</param>
        /// <param name="idEmploye">Le id de l'employé associé à la disponibilité</param>
        /// <param name="idType">Le id du type associé à la disponibilité</param>
        /// <param name="statut">Le statut associé à la disponibilité</param>
        /// <returns></returns>
        public static List<Rendezvous> CreerDispos(DateTime debut, DateTime fin, TimeSpan duree, List<Rendezvous> dispoExistantes, int idEmploye, int idType, string statut)
        {
            // Initialisation
            Rendezvous dispo;
            DateTime tCourant = debut;
            List<Rendezvous> disposCrees = new List<Rendezvous>();

            // Création des nouvelles disponibilités qui ne sont pas en conflit !                            
            while (tCourant.TimeOfDay + duree <= fin.TimeOfDay)
            {
                // Vérification des conflits
                if (!HoraireUtil.IsRDVConflictuel(tCourant, tCourant + duree, dispoExistantes))
                {
                    // Création de la disponibilité
                    dispo = new Rendezvous();
                    dispo.debut_rdv = tCourant;
                    dispo.fin_rdv = tCourant + duree;
                    dispo.id_employe_rdv = idEmploye;
                    dispo.id_typerdv_rdv = idType;
                    dispo.statut_rdv = statut;

                    // Ajout de la disponibilité à la liste
                    disposCrees.Add(dispo);
                }

                // On avance à la prochaine dispo
                tCourant += duree;
            }

            return disposCrees;
        }

    }
}