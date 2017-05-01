using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace InterfaceEntrepriseWPF
{
    /// <summary>
    /// Classe statique pour permettre le changement de fenêtre depuis
    /// n'importe quelle fenêtre
    /// </summary>
    static class Switcher
    {
        // La fênêtre affichant le contenu
        public static MainWindow fenetreAffichage;

        /// <summary>
        /// Méthode pour changer l'écran dans la fenêtre
        /// </summary>
        /// <param name="newPage">La nouvelle "page"</param>
        public static void Switch(UserControl newPage)
        {
            fenetreAffichage.Navigate(newPage);
        }

        /// <summary>
        /// Méthode pour changer l'écran dans la fenêtre
        /// </summary>
        /// <param name="newPage">La nouvelle "page"</param>
        /// <param name="state">L'état contenant les informations à transférer à la nouvelle "page"</param>
        public static void Switch(UserControl newPage, object state)
        {
            fenetreAffichage.Navigate(newPage, state);
        }

    }
}
