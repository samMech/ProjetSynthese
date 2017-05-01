using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InterfaceEntrepriseWPF.Vues_Modeles
{
    /// <summary>
    /// Classe vue-modèle pour l'écran de connexion
    /// </summary>
    class ConnexionVueModele : VueModele
    {
        // Attributs
        private string _login = "";
        private string _password = "";
        private ICommand _loginCommand;

        //============//
        // Propriétés //
        //============//

        /// <summary>
        /// L'identifiant utilisateur
        /// </summary>
        public string Login
        {
            get { return _login; }
            set
            {
                if (!_login.Equals(value))
                {
                    _login = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Le mot de passe utilisateur
        /// </summary>
        public string Password
        {
            get { return _password; }
            set
            {
                if (!_password.Equals(value))
                {
                    _password = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Commande pour le bouton de connexion
        /// </summary>
        public ICommand LoginCommand
        {
            get
            {
                if (_loginCommand == null)
                {
                    // Création de la commande si elle n'existe pas encore
                    _loginCommand = new RelayCommand(ConnexionUsager, ConnexionUsagerPossible);
                }
                return _loginCommand;
            }
        }

        //==========//
        // Méthodes //
        //==========//

        // Méthode pour faire la connexion            
        private void ConnexionUsager(object obj)
        {
            


        }

        // Méthode pour savoir si la connexion est possible
        private bool ConnexionUsagerPossible(object obj)
        {
            return _login.Length != 0 && _password.Length != 0;
        }

    }        
}
