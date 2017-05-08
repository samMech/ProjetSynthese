using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InterfaceEntrepriseWPF.Vues_Modeles
{
    /// <summary>
    /// Classe abstraite pour toutes les VuesModeles
    /// </summary>
    public abstract class VueModele : INotifyPropertyChanged
    {
        // Attributs
        private string _titrePage = "";

        // Commandes
        private ICommand _pageAccueilCommand;
        private ICommand _pagePrecedenteCommand;

        /// <summary>
        /// Événement à déclencher quand une propriété change
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Méthode pour déclencher l'événement de notification du changement d'un propriété
        /// </summary>
        /// <param name="nomPropriete">Le nom de la propriété qui a changée</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string nomPropriete = null)
        {
            // Si l'événement n'est pas null
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nomPropriete));
        }

        //============//
        // Propriétés //
        //============//

        /// <summary>
        /// Le titre de la page
        /// </summary>
        public string TitrePage
        {
            get
            {
                return _titrePage;
            }
            set
            {
                if (! _titrePage.Equals(value))
                {
                    _titrePage = value;
                    OnPropertyChanged();
                }                
            }
        }

        /// <summary>
        /// Commande pour le bouton permettant de retourner à la page d'accueil
        /// </summary>
        public ICommand PageAccueilCommand
        {
            get
            {
                if (_pageAccueilCommand == null)
                {
                    // Création de la commande si elle n'existe pas encore
                    _pageAccueilCommand = new RelayCommand(RetournerPageAccueil);
                }
                return _pageAccueilCommand;
            }
        }

        /// <summary>
        /// Commande pour le bouton permettant de retourner à la page d'accueil
        /// </summary>
        public ICommand PagePrecedenteCommand
        {
            get
            {
                if (_pagePrecedenteCommand == null)
                {
                    // Création de la commande si elle n'existe pas encore
                    _pagePrecedenteCommand = new RelayCommand(RetournerPagePrecedente);
                }
                return _pagePrecedenteCommand;
            }
        }

        //==========//
        // Méthodes //
        //==========//

        /// <summary>
        /// Méthode pour mettre à jour les données de la page
        /// </summary>
        public virtual void UpdateData() { }

        // Méthode pour revenir à la page d'accueil
        protected void RetournerPageAccueil(object obj)
        {
            ApplicationVueModele app = ApplicationVueModele.Instance;
            app.ChangePageCommand.Execute(Pages.PORTAIL);
        }

        // Méthode pour revenir à la page précédente
        protected void RetournerPagePrecedente(object obj)
        {
            ApplicationVueModele app = ApplicationVueModele.Instance;
            app.ChangePageCommand.Execute(app.PagePrecedente);
        }

    }
}
