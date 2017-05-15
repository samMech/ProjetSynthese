using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using CalendrierRDV;
using InterfaceClientWPF.Model;
using InterfaceClientWPF.Modele;
using InterfaceClientWPF.Utilitaire;
using ProjetRDV247.Modele;
//using CalendrierRDV = CalendrierRDV.CalendrierRDV;

namespace InterfaceClientWPF.ViewModels
{
    class ConsulterHoraireViewModel:VueModele
    {

        //constantes
        private static readonly Dictionary<string, Color> COULEURS_STATUT = new Dictionary<string, Color>();

        //attributs
        private DateTime _dateJour;
        private Rendezvous _rdvSelectionnee;
        private ObservableCollection<CalendrierRDV.IRendezVous> _listeDisponibilites;
        private ObservableCollection<Employe> _listEmployes;
        private Employe _selectedEmploye;
        private bool initTermine = false;

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
            initTermine = true;
        }

        static ConsulterHoraireViewModel()
        {
            // TODO (prendre la liste du service web pour les status)
            COULEURS_STATUT.Add("rv", Colors.Lavender);
            COULEURS_STATUT.Add("dispo", Colors.GhostWhite);
            COULEURS_STATUT.Add("annule", Colors.LightPink);
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
                    // Calcul du début de semaine pour comparaison
                    DateTime nouveauLundi = CalendrierRDV.Utilitaire.TrouverLundiPrecedent(value);
                    DateTime lundiActuel = CalendrierRDV.Utilitaire.TrouverLundiPrecedent(_dateJour);

                    // Modification
                    _dateJour = value;

                    // Vérification pour savoir si on a changé de semaine
                    if (initTermine && Math.Abs((nouveauLundi - lundiActuel).Days) > 6)
                    {
                        // Mise à jour des données
                        UpdateData();
                    }

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
                    _enregistrerRDVCommand = new RelayCommand(EnregistrerRDV, CanUseRdv);
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
                    _annulerRDVCommand = new RelayCommand(AnnulerRDV, CanUseRdv);
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

        public Rendezvous RdvSelectionnee
        {
            get { return _rdvSelectionnee; }
            set
            {
                if (value != null && value.Equals(_rdvSelectionnee) == false)
                {
                    _rdvSelectionnee = value;
                    OnPropertyChanged();
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
                SelectedEmploye==null? 0:SelectedEmploye.id_employe, cl.id_client), COULEURS_STATUT);
        }


        private void EnregistrerRDV(object obj)
        {
            Client cl = ApplicationViewModel.Instance.ClientConnecte;
            RestDao.EnregistrerRdv(cl.id_client, RdvSelectionnee.id_rdv);
            UpdateData();
        }

        private void AnnulerRDV(object obj)
        {
            Client cl = ApplicationViewModel.Instance.ClientConnecte;
            RestDao.AnnulerRdv(cl.id_client, RdvSelectionnee.id_rdv);
            UpdateData();
        }

        private bool CanUseRdv(object obj)
        {
            List<IRendezVous> liste = ListeDisponibilites.Where(x => x.IsSelectionne).ToList();
            if (liste.Count == 1)
            {
                RdvSelectionnee = ((RendezVousAdapter)liste.Single()).RDV;
                return true;
            }

            return false;
        }
    }
}
