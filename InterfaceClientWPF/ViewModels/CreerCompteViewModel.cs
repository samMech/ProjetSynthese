using System;
using System.Net;
using System.Windows.Documents;
using System.Windows.Input;
using InterfaceClientWPF.Model;

namespace InterfaceClientWPF.ViewModels
{
    class CreerCompteViewModel : VueModele
    {
        private string _nom = "";
        private string _prenom = "";
        private string _telephone = "";
        private string _courriel = "";
        private string _password = "";
        private ICommand _registerCommand;

        private string _messageErreur = "";

        public CreerCompteViewModel()
        {
            TitrePage = "Inscription";
        }

        public string MessageErreur
        {
            get { return _messageErreur; }
            set
            {
                _messageErreur = value;
                OnPropertyChanged();
            }
        }

        public string Nom
        {
            get { return _nom; }
            set
            {
                if (!_nom.Equals(value))
                {
                    _nom = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Prenom
        {
            get { return _prenom; }
            set
            {
                if (!_prenom.Equals(value))
                {
                    _prenom = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Telephone
        {
            get { return _telephone; }
            set
            {
                if (!_telephone.Equals(value))
                {
                    _telephone = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Courriel
        {
            get { return _courriel; }
            set
            {
                if (!_courriel.Equals(value))
                {
                    _courriel = value;
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

        public ICommand RegisterCommand
        {
            get
            {
                if (_registerCommand == null)
                {
                    _registerCommand = new RelayCommand(RegisterClient, CanRegistrer);
                }
                return _registerCommand;
            }
        }

        private void RegisterClient(object obj)
        {
            try
            {
                bool estEnregistrer = RestDao.EnregistrerClient(Nom, Prenom, Telephone, Courriel, Password);

                if (estEnregistrer)
                {
                    ApplicationViewModel app = ApplicationViewModel.Instance;
                    app.ChangePageCommand.Execute(new ConnexionViewModel());
                }
                else
                {
                    MessageErreur = "Le client n'a pas pu etre créer. ";
                }
            }
            catch (WebException e)
            {
                MessageErreur = "Le service a rencontré une erreur.";
            }
        }

        private bool CanRegistrer(object obj)
        {
            Console.Write(Nom);
            Console.Write(Prenom);
            Console.Write(Telephone);
            Console.Write(Courriel);
            Console.Write(Password);

            return Nom.Length > 0 && Prenom.Length > 0 && Telephone.Length > 0 && Courriel.Length > 0 &&
                   Password.Length > 0;
        }
    }
}