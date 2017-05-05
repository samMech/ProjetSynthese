using System.Windows.Input;

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

        public CreerCompteViewModel()
        {
            TitrePage = "Inscription";
        }


        public string Nom
        {
            get { return _nom; }
            set { _nom = value; }
        }

        public string Prenom
        {
            get { return _prenom; }
            set { _prenom = value; }
        }

        public string Telephone
        {
            get { return _telephone; }
            set { _telephone = value; }
        }

        public string Courriel
        {
            get { return _courriel; }
            set { _courriel = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public ICommand RegisterCommand
        {
            get { return _registerCommand; }
            set { _registerCommand = value; }
        }

        //TODO: Continuer CreerCompteViewModel
    }
}