using InterfaceEntrepriseWPF.Vues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace InterfaceEntrepriseWPF.Vues_Modeles
{
    /// <summary>
    /// Classe vue-modèle pour toute l'application
    /// </summary>
    class ApplicationVueModele : VueModele
    {
        // Attributs
        private ICommand _changeViewCommand;

        private UserControl _vueCourante;
        private List<UserControl> _vues;

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public ApplicationVueModele()
        {
            // Ajout des différentes vues-modèles de l'application
            Vues.Add(new VueConnexion());

            // Initialisation de la page d'accueil
            VueCourante = Vues[0];
        }

        //============//
        // Propriétés //
        //============//

        /// <summary>
        /// Commande pour changer de page
        /// </summary>
        public ICommand ChangeViewCommand
        {
            get
            {
                if (_changeViewCommand == null)
                {
                    // Création de la commande si elle n'existe pas encore
                    _changeViewCommand = new RelayCommand(
                        p => ChangerVueCourante((UserControl)p),
                        p => p is UserControl);
                }
                return _changeViewCommand;
            }
        }

        /// <summary>
        /// Liste des pages
        /// </summary>
        public List<UserControl> Vues
        {
            get
            {
                // Création de la commande si elle n'existe pas encore
                if (_vues == null)
                {
                    _vues = new List<UserControl>();
                }
                return _vues;
            }
        }

        /// <summary>
        /// La page courante
        /// </summary>
        public UserControl VueCourante
        {
            get
            {
                return _vueCourante;
            }
            set
            {
                if (_vueCourante != value)
                {
                    _vueCourante = value;
                    OnPropertyChanged();
                }
            }
        }

        //==========//
        // Méthodes //
        //==========//

        // Méthode pour changer de page
        private void ChangerVueCourante(UserControl vue)
        {            
            if (!Vues.Contains(vue))
            {
                // Si la page n'existe pas encore, on l'ajoute
                Vues.Add(vue);
            }                

            // On remplace le contenu de la fenêtre par la nouvelle page
            VueCourante = Vues.FirstOrDefault(vm => vm == vue);
        }

    }
}
