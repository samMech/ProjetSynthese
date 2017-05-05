using InterfaceEntrepriseWPF.Modele;
using ProjetRDV247.Modele;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        // Pour les erreurs
        private string _messageErreurService = "";
        private bool _erreurAuthentificationUsager = false;
        
        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public ConnexionVueModele()
        {
            // Ajustement du titre de la fenêtre
            TitrePage = "Connexion";
        }

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

        /// <summary>
        /// Est-ce que l'authentification a réussie
        /// </summary>
        public bool ErreurAuthentificationUsager
        {
            get
            {
                return _erreurAuthentificationUsager;
            }
            set
            {
                _erreurAuthentificationUsager = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Le message en cas d'erreur du service web
        /// </summary>
        public string MessageErreurService
        {
            get
            {
                return _messageErreurService;
            }
            set
            {
                _messageErreurService = value;
                OnPropertyChanged();
            }
        }

        //==========//
        // Méthodes //
        //==========//

        // Méthode pour faire la connexion            
        private void ConnexionUsager(object obj)
        {
            // Réinitialisation des erreurs
            MessageErreurService = "";
            ErreurAuthentificationUsager = false;

            try
            {
                // Tentative de connexion (authentification)
                Employe emp = RestDao.ConnexionEmploye(Login, Password);

                if (emp != null)
                {
                    ApplicationVueModele app = ApplicationVueModele.Instance;
                    app.EmployeConnecte = emp;
                    app.ChangePageCommand.Execute(new PortailEmployeVueModele());
                }
                else
                {
                    ErreurAuthentificationUsager = true;
                }
            }
            catch (WebException e)
            {
                switch (e.Status)
                {                    
                    case WebExceptionStatus.ConnectFailure:
                        MessageErreurService = "Le service n'est pas disponible !";
                        break;
                    default:
                        MessageErreurService = "Le service a rencontré une erreur !";
                        break;
                }
            }
                  
        }

        // Méthode pour savoir si la connexion est possible
        private bool ConnexionUsagerPossible(object obj)
        {
            return _login.Length > 0 && _password.Length > 0;
        }

    }        
}
