using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InterfaceEntrepriseWPF.Vues_Modeles
{
    /// <summary>
    /// Classe vue-modèle pour toute l'application
    /// </summary>
    class ApplicationVueModele : VueModele
    {
        // Attributs
        private ICommand _changePageCommand;

        private VueModele _currentPageViewModel;
        private List<VueModele> _pageViewModels;

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public ApplicationVueModele()
        {
            // Ajout des différentes vues-modèles de l'application
            PageViewModels.Add(new ConnexionVueModele());

            // Initialisation de la page d'accueil
            CurrentPageViewModel = PageViewModels[0];
        }

        //============//
        // Propriétés //
        //============//

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
                        p => ChangeViewModel((VueModele)p),
                        p => p is VueModele);
                }
                return _changePageCommand;
            }
        }

        /// <summary>
        /// Liste des pages
        /// </summary>
        public List<VueModele> PageViewModels
        {
            get
            {
                // Création de la commande si elle n'existe pas encore
                if (_pageViewModels == null)
                {
                    _pageViewModels = new List<VueModele>();
                }
                return _pageViewModels;
            }
        }

        /// <summary>
        /// La page courante
        /// </summary>
        public VueModele CurrentPageViewModel
        {
            get
            {
                return _currentPageViewModel;
            }
            set
            {
                if (_currentPageViewModel != value)
                {
                    _currentPageViewModel = value;
                    OnPropertyChanged();
                }
            }
        }

        //==========//
        // Méthodes //
        //==========//

        // Méthode pour changer de page
        private void ChangeViewModel(VueModele viewModel)
        {            
            if (!PageViewModels.Contains(viewModel))
            {
                // Si la page n'existe pas encore, on l'ajoute
                PageViewModels.Add(viewModel);
            }                

            // On remplace le contenu de la fenêtre par la nouvelle page
            CurrentPageViewModel = PageViewModels.FirstOrDefault(vm => vm == viewModel);
        }

    }
}
