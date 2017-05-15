using ProjetRDV247.Modele;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ProjetRDV247.Utils
{
    /// <summary>
    /// Classe utilitaire pour les méthode générale
    /// </summary>
    public class Utilitaire
    {
        /// <summary>
        /// Fonction pour trouver le lundi précédent une date
        /// </summary>
        /// <param name="dateCourante">La date courante</param>
        /// <returns></returns>
        public static DateTime TrouverLundiPrecedent(DateTime dateCourante)
        {
            // Calcul du nombre de jour entre la dateCourante et lundi dernier
            int deltaJour = DayOfWeek.Monday - dateCourante.DayOfWeek;

            // Ajustement au cas où le premier jour de la semaine n'est pas lundi
            if (deltaJour > 0)
            {
                deltaJour -= 7;
            }

            return dateCourante.AddDays(deltaJour);
        }

        /// <summary>
        /// Méthode pour valider un entier
        /// </summary>
        /// <param name="entier">La chaîne de caractères contenant l'entier</param>
        /// <returns>Vrai si la chaîne peut être convertie en entier valide</returns>
        public static bool ValiderEntier(string entier)
        {
            int valeur;
            return Int32.TryParse(entier, out valeur);
        }

        /// <summary>
        /// Méthode pour valider une date
        /// </summary>
        /// <param name="date">La chaîne de caractères contenant la date</param>
        /// <returns>Vrai si la chaîne peut être convertie en une date valide</returns>
        public static bool ValiderDate(string date)
        {
            DateTime datetime;
            return DateTime.TryParseExact(date, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out datetime);
        }

        /// <summary>
        /// Méthode pour créer une nouvelle copie d'un rendez-vous
        /// </summary>
        /// <param name="rdv">Le rendez-vous à copier</param>
        /// <returns>La copie du rendez-vous</returns>
        public static Rendezvous ClonerRendezVous(Rendezvous rdv)
        {
            Rendezvous rdvCopie = new Rendezvous();
            rdvCopie.id_rdv = rdv.id_rdv;
            rdvCopie.debut_rdv = rdv.debut_rdv;
            rdvCopie.fin_rdv = rdv.fin_rdv;
            rdvCopie.statut_rdv = rdv.statut_rdv;

            rdvCopie.id_typerdv_rdv = rdv.id_typerdv_rdv;
            rdvCopie.id_client_rdv = rdv.id_client_rdv;            
            rdvCopie.id_employe_rdv = rdv.id_employe_rdv;

            rdvCopie.Client = rdv.Client;
            rdvCopie.Employe = rdv.Employe;
            rdvCopie.Typerdv = rdv.Typerdv;
            
            return rdvCopie;
        }
    }
}