using InterfaceEntrepriseWPF.Modele;
using InterfaceEntrepriseWPF.Utilitaire;
using ProjetRDV247.Modele;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
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

        // Commandes
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
        /// Commande pour le bouton de connexion
        /// </summary>
        public ICommand LoginCommand
        {
            get
            {
                if (_loginCommand == null)
                {
                    // Création de la commande si elle n'existe pas encore
                    _loginCommand = new RelayCommand(ConnexionUsager, CanConnexionUsager);
                }
                return _loginCommand;
            }
        }

        /// <summary>
        /// Est-ce que l'authentification a échouée
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
                Employe emp = RestDao.ConnexionEmploye(Login, ConversionUtil.SecureStringToString(GetSecurePassword(obj)));

                if (emp != null)
                {
                    ApplicationVueModele app = ApplicationVueModele.Instance;
                    app.EmployeConnecte = emp;
                    app.ChangePageCommand.Execute(Pages.PORTAIL);
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
        private bool CanConnexionUsager(object obj)
        {
            SecureString password = GetSecurePassword(obj);
            if (password != null)
            {                
                return Login.Length > 0 && password.Length > 0;
            }
            return false;                     
        }

        // Méthode pour récupérer le password de la vue
        private SecureString GetSecurePassword(object obj)
        {
            IPassword passwordContainer = obj as IPassword;
            return (passwordContainer != null) ? passwordContainer.Password : null;
        }

    }        
}
