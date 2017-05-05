using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using InterfaceClientWPF.Model;
using ProjetRDV247.Modele;

namespace InterfaceClientWPF.ViewModels
{
    class ConnexionViewModel: VueModele
    {
        //attributs
        private string _login = "";
        private string _password = "";
        private ICommand _loginCommand;
        private ICommand _registerCommand;

        //pour les erreurs
        private string _messageErreurService = "";
        private bool _erreurAuthentificationClient = false;

        public ConnexionViewModel()
        {
            TitrePage = "Connexion";
        }

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

        public ICommand LoginCommand
        {
            get
            {
                if (_loginCommand == null)
                {
                    _loginCommand = new RelayCommand(ConnexionClient, ConnexionClientPossible);
                }
                return _loginCommand;
            }
        }

        public bool ErreurAuthentificationClient
        {
            get { return _erreurAuthentificationClient; }
            set
            {
                _erreurAuthentificationClient = value;
                OnPropertyChanged();
            }
        }

        public string MessageErreurService
        {
            get { return _messageErreurService; }
            set
            {
                _messageErreurService = value;
                OnPropertyChanged();
            }
        }

        public ICommand RegisterCommand
        {
            get
            {
                if (_registerCommand == null)
                {
                    _registerCommand = new RelayCommand(OuvrirInscription);
                }
                return _registerCommand;
            }
        }

        private void OuvrirInscription(object obj)
        {
            try
            {
                ApplicationViewModel app = ApplicationViewModel.Instance;
                app.ChangePageCommand.Execute(new CreerCompteViewModel());
            }
            catch (WebException e)
            {
                MessageErreurService = "Le service a rencontré un probleme";
            }
        }

        private void ConnexionClient(object obj)
        {
            MessageErreurService = "";
            ErreurAuthentificationClient = false;

            try
            {
                Client cl = RestDao.ConnexionClient(Login, Password);

                if (cl != null)
                {
                    ApplicationViewModel app = ApplicationViewModel.Instance;
                    app.ClientConnecte = cl;
                    app.ChangePageCommand.Execute(new PortailClientViewModel());
                }
                else
                {
                    ErreurAuthentificationClient = true;
                }
            }
            catch (WebException e)
            {
                switch (e.Status)
                {
                    case WebExceptionStatus.ConnectFailure:
                        MessageErreurService = "Le service n'est pas disponible!";
                        break;

                    default:
                        MessageErreurService = "Une erreur est survenu";
                        break;
                }
            }
        }

        private bool ConnexionClientPossible(object obj)
        {
            return _login.Length > 0 && _password.Length > 0;
        }
    }
}
