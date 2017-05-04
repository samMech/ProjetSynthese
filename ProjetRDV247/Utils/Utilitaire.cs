using System;
using System.Collections.Generic;
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
    }
}