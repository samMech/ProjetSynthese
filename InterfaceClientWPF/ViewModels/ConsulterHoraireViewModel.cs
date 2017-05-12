using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CalendrierRDV;
using InterfaceClientWPF.Model;
using InterfaceClientWPF.Utilitaire;
using ProjetRDV247.Modele;

namespace InterfaceClientWPF.ViewModels
{
    class ConsulterHoraireViewModel:VueModele
    {
        //attributs
        private DateTime _dateJour;
        private Rendezvous _dispoSelectionnee;
        private ObservableCollection<CalendrierRDV.IRendezVous> _listeDisponibilites;
        private ObservableCollection<Employe> _listEmployes;
        private Employe _selectedEmploye;

        //commandes
        private ICommand _enregistrerRDVCommand;
        private ICommand _annulerRDVCommand;

        //constructeur par defaut
        public ConsulterHoraireViewModel()
        {
            TitrePage = "Consulter les Horaires";
            DateJour = DateTime.Today;
            ListeEmployes = new ObservableCollection<Employe>(RestDao.GetEmployes());
            ListeDisponibilites = new ObservableCollection<IRendezVous>();
        }

        //===========//
        // Properties//
        //===========//
        public ObservableCollection<Employe> ListeEmployes
        {
            get
            {
                return _listEmployes;
            }
            set
            {
                if (value != null)
                {
                    _listEmployes = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime DateJour
        {
            get { return _dateJour; }
            set
            {
                if (value != null && value.Equals(_dateJour) == false)
                {
                    // Vérification pour savoir si on a changé de semaine
                    if (Math.Abs((value - CalendrierRDV.Utilitaire.TrouverLundiPrecedent(_dateJour)).Days) > 6)
                    {
                        // Mise à jour des données
                        UpdateData();
                    }

                    _dateJour = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<IRendezVous> ListeDisponibilites
        {
            get { return _listeDisponibilites; }
            set
            {
                if (value != null && value.Equals(_listeDisponibilites) == false)
                {
                    _listeDisponibilites = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand EnregistrerRdvCommand
        {
            get
            {
                if (_enregistrerRDVCommand == null)
                {
                    _enregistrerRDVCommand = new RelayCommand(EnregistrerRDV);
                }
                return _enregistrerRDVCommand;
            }
        }

        public ICommand AnnulerRdvCommand
        {
            get
            {
                if (_annulerRDVCommand == null)
                {
                    _annulerRDVCommand = new RelayCommand(AnnulerRDV);
                }
                return _annulerRDVCommand;
            }
        }

        public Employe SelectedEmploye
        {
            get {return _selectedEmploye;}
            set
            {
                if (value != null && value.Equals(_selectedEmploye) == false)
                {
                    _selectedEmploye = value;
                    OnPropertyChanged();
                    UpdateData();
                }
            }
        }

        //==========//
        // Méthodes //
        //==========//

        public override void UpdateData()
        {
            Client cl = ApplicationViewModel.Instance.ClientConnecte;

            ListeDisponibilites = ConversionUtil.ConvertirRDVToIRDV(RestDao.GetDisposRDVEmploye(
                SelectedEmploye==null? 0:SelectedEmploye.id_employe, cl.id_client));
        }


        private void EnregistrerRDV(object obj)
        {
            throw new NotImplementedException();
        }

        private void AnnulerRDV(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
