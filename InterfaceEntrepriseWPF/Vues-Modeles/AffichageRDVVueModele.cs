using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InterfaceEntrepriseWPF.Vues_Modeles
{
    class AffichageRDVVueModele : VueModele
    {
        // Attributs

        // Commandes
        private ICommand _gererDisposCommand;

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public AffichageRDVVueModele()
        {
            // Ajustement du titre de la fenêtre
            TitrePage = "Affichage des rendez-vous";
        }

        //============//
        // Propriétés //
        //============//

        /// <summary>
        /// Commande pour le bouton permettant d'aller à la page pour gérer les disponibilités
        /// </summary>
        public ICommand GererDisposCommand
        {
            get
            {
                if (_gererDisposCommand == null)
                {
                    // Création de la commande si elle n'existe pas encore
                    _gererDisposCommand = new RelayCommand(ChargerGestionDispo);
                }
                return _gererDisposCommand;
            }
        }

        //==========//
        // Méthodes //
        //==========//

        // Méthode pour aller à la page de gestion des disponibilités
        protected void ChargerGestionDispo(object obj)
        {
            ApplicationVueModele app = ApplicationVueModele.Instance;
            app.ChangePageCommand.Execute(Pages.GESTION_DISPOS);
        }

    }
}
