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
    /// Classe vue-modèle singleton pour toute l'application
    /// </summary>
    sealed class ApplicationVueModele : VueModele
    {
        // Instance courante et objet pour avoir un singleton "thread safe"
        private static volatile ApplicationVueModele _instance;
        private static object syncRoot = new Object();

        // Attributs
        private ICommand _changePageCommand;
        private VueModele _pageCourante;
        private List<VueModele> _pages;

        /// <summary>
        /// Constructeur privé
        /// </summary>
        private ApplicationVueModele()
        {
            // Ajout des différentes vues-modèles de l'application
            Pages.Add(new ConnexionVueModele());

            // Initialisation de la page d'accueil
            PageCourante = Pages[0];
        }

        //============//
        // Propriétés //
        //============//
        
        /// <summary>
        /// Retourne l'instance courante due ViewModele de l'application
        /// </summary>
        public static ApplicationVueModele Instance
        {
            get
            {
                // Double-checked locking (pour garantir le "thread safe" à faible coût)
                if (_instance == null)
                {
                    // Locking --> opération très couteuse ! (Fait juste la première fois)
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new ApplicationVueModele();
                        }                            
                    }
                }

                return _instance;
            }
        }

        /// <summary>
        /// Commande pour changer de page
        /// </summary>
        public ICommand ChangePageCommand
        {
            get
            {
                if (_changePageCommand == null)
                {                 
                    // Création de la commande si elle n'existe pas encore
                    _changePageCommand = new RelayCommand(
                        p => ChangerPageCourante((VueModele)p),
                        p => p is VueModele);
                }
                return _changePageCommand;
            }
        }

        /// <summary>
        /// Liste des pages
        /// </summary>
        public List<VueModele> Pages
        {
            get
            {
                // Création de la commande si elle n'existe pas encore
                if (_pages == null)
                {
                    _pages = new List<VueModele>();
                }
                return _pages;
            }
        }

        /// <summary>
        /// La page courante
        /// </summary>
        public VueModele PageCourante
        {
            get
            {
                return _pageCourante;
            }
            set
            {
                if (_pageCourante != value)
                {
                    _pageCourante = value;
                    OnPropertyChanged();
                }
            }
        }

        //==========//
        // Méthodes //
        //==========//

        // Méthode pour changer de page
        private void ChangerPageCourante(VueModele page)
        {            
            if (!Pages.Contains(page))
            {
                // Si la page n'existe pas encore, on l'ajoute
                Pages.Add(page);
            }                

            // On remplace le contenu de la fenêtre par la nouvelle page
            PageCourante = Pages.FirstOrDefault(vm => vm == page);
        }

    }
}
