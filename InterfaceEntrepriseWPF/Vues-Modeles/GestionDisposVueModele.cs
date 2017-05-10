using InterfaceEntrepriseWPF.Modele;
using InterfaceEntrepriseWPF.Utilitaire;
using ProjetRDV247.Modele;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace InterfaceEntrepriseWPF.Vues_Modeles
{
    class GestionDisposVueModele : VueModele
    {
        // Constantes
        private static readonly List<int> DUREES_RDV = new List<int>(new int[]{10, 15, 20, 30, 45, 60, 90, 120});
        private static readonly Dictionary<string, Color> COULEURS_STATUT = new Dictionary<string, Color>();

        // Attributs
        private DateTime _dateJour;
        private DateTime _debutPlageAjout;
        private DateTime _finPlageAjout;
        private int _dureeRDV;
        private Typerdv _typeRDV;
        private string _raison = "";

        // La liste des disponibilités pour le composant graphique
        private ObservableCollection<CalendrierRDV.IRendezVous> _listeDisponibilites;

        // La liste des types de rendez-vous pour l'employé courant
        private List<Typerdv> _listeTypeRDV;

        // Commandes
        private ICommand _consulterRDVCommand;
        private ICommand _ajouterDisposCommand;
        private ICommand _modifierDispoCommand;
        private ICommand _supprimerDisposCommand;

        // Pour les erreurs
        private bool _erreurDebutPlage;
        private bool _erreurFinPlage;

        /// <summary>
        /// Constructeur statique pour initialiser les constantes
        /// </summary>
        static GestionDisposVueModele()
        {
            // TODO (prendre la liste du service web pour les status)
            COULEURS_STATUT.Add("rv", Colors.Lavender);
            COULEURS_STATUT.Add("dispo", Colors.GhostWhite);
            COULEURS_STATUT.Add("annule", Colors.LightPink);
        }

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public GestionDisposVueModele()
        {
            // Ajustement du titre de la fenêtre
            TitrePage = "Gestion des disponibilités";

            // Initialisation des valeurs
            DateJour = DateTime.Today;
            DureeRDV = DureesRDV.First();
            DebutPlageAjout = DateTime.Now.AddMinutes(DureeRDV - DateTime.Now.Minute % DureeRDV);
            FinPlageAjout = DebutPlageAjout.AddMinutes(DureeRDV);
            ListeDisponibilites = new ObservableCollection<CalendrierRDV.IRendezVous>();
        }

        //============//
        // Propriétés //
        //============//

        /// <summary>
        /// Les durées permises pour un rendez-vous
        /// </summary>
        public List<int> DureesRDV
        {
            get { return DUREES_RDV; }            
        }

        /// <summary>
        /// Les couleurs pour les différents statuts
        /// </summary>
        public Dictionary<string, Color> CouleurStatuts
        {
            get { return COULEURS_STATUT; }
        }

        /// <summary>
        /// La durée des rendez-vous à ajouter
        /// </summary>
        public int DureeRDV
        {
            get { return _dureeRDV; }
            set
            {
                if (DureesRDV.Contains(value))
                {
                    _dureeRDV = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Le type des rendez-vous à ajouter
        /// </summary>
        public Typerdv TypeRDV
        {
            get { return _typeRDV; }
            set
            {
                _typeRDV = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// La date du calendrier
        /// </summary>
        public DateTime DateJour
        {
            get { return _dateJour; }
            set
            {
                if (_dateJour != null)
                {
                    _dateJour = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Le début de la plage d'ajout
        /// </summary>
        public DateTime DebutPlageAjout
        {
            get { return _debutPlageAjout; }
            set
            {
                if (_debutPlageAjout != null)
                {
                    _debutPlageAjout = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Est-ce que le début de la plage est invalide ?
        /// </summary>
        public bool ErreurDebutPlage
        {
            get { return _erreurDebutPlage; }
            set
            {
                _erreurDebutPlage = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// La fin de la plage d'ajout
        /// </summary>
        public DateTime FinPlageAjout
        {
            get { return _finPlageAjout; }
            set
            {
                if (_finPlageAjout != null)
                {
                    _finPlageAjout = value;
                    OnPropertyChanged();
                }
            }
        }
        
        /// <summary>
        /// Est-ce que la fin de la plage est valide
        /// </summary>
        public bool ErreurFinPlage
        {
            get { return _erreurFinPlage; }
            set
            {
                _erreurFinPlage = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// La raison en cas de modification
        /// </summary>
        public string Raison
        {
            get { return _raison; }
            set
            {
                _raison = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// La liste des disponibilités pour le composant graphique
        /// </summary>
        public ObservableCollection<CalendrierRDV.IRendezVous> ListeDisponibilites
        {
            get { return _listeDisponibilites; }
            set
            {
                _listeDisponibilites = (value == null) ? new ObservableCollection<CalendrierRDV.IRendezVous>() : value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// La liste des types de rendez-vous de l'employé
        /// </summary>
        public List<Typerdv> ListeTypesRDV
        {
            get { return _listeTypeRDV; }
            set
            {
                if (value != null)
                {
                    _listeTypeRDV = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Commande pour le bouton pour l'ajout de disponibilités
        /// </summary>
        public ICommand AjouterDisposCommand
        {
            get
            {
                if (_ajouterDisposCommand == null)
                {
                    // Création de la commande si elle n'existe pas encore
                    _ajouterDisposCommand = new RelayCommand(AjouterDispos, CanAjouterDispos);
                }
                return _ajouterDisposCommand;
            }
        }

        /// <summary>
        /// Commande pour le bouton pour la modification d'une disponibilité
        /// </summary>
        public ICommand ModifierDispoCommand
        {
            get
            {
                if (_modifierDispoCommand == null)
                {
                    // Création de la commande si elle n'existe pas encore
                    _modifierDispoCommand = new RelayCommand(ModifierDispo, CanModifierDispo);
                }
                return _modifierDispoCommand;
            }
        }

        /// <summary>
        /// Commande pour le bouton pour la suppression de disponibilités
        /// </summary>
        public ICommand SupprimerDisposCommand
        {
            get
            {
                if (_supprimerDisposCommand == null)
                {
                    // Création de la commande si elle n'existe pas encore
                    _supprimerDisposCommand = new RelayCommand(SupprimerDispos, CanSupprimerDispos);
                }
                return _supprimerDisposCommand;
            }
        }

        /// <summary>
        /// Commande pour le bouton permettant d'aller à la page pour consulter tous les rendez-vous
        /// </summary>
        public ICommand ConsulterRDVCommand
        {
            get
            {
                if (_consulterRDVCommand == null)
                {
                    // Création de la commande si elle n'existe pas encore
                    _consulterRDVCommand = new RelayCommand(ChargerConsultationRDV);
                }
                return _consulterRDVCommand;
            }
        }

        //==========//
        // Méthodes //
        //==========//

        /// <summary>
        /// Mise à jour des données
        /// </summary>
        public override void UpdateData()
        {
            // Récupération des disponibilités et types
            Employe emp = ApplicationVueModele.Instance.EmployeConnecte;
            ListeDisponibilites = ConversionUtil.ConvertirRDVToIRDV(RestDao.GetDisposEmploye(emp.id_employe), COULEURS_STATUT);
            ListeTypesRDV = emp.Typerdv.ToList();
            TypeRDV = ListeTypesRDV[0];
        }

        // Méthode pour ajouter des disponibilités
        private void AjouterDispos(object obj)
        {
            try
            {
                // Calcul de la date de début et de fin et de la durée
                DateTime dateDebut = DateJour.Date + DebutPlageAjout.TimeOfDay;
                DateTime dateFin = dateDebut + (FinPlageAjout - DebutPlageAjout);

                // Insertion
                Employe emp = ApplicationVueModele.Instance.EmployeConnecte;
                List<Rendezvous> disposAjoutees = RestDao.AjouterDispos(emp.id_employe, dateDebut, dateFin, DureeRDV, TypeRDV.id_typerdv);

                Console.WriteLine("Nb ajouts: {0}", disposAjoutees.Count);

                // Mise à jour de la liste locales
                foreach (Rendezvous d in disposAjoutees)
                {
                    ListeDisponibilites.Add(new RendezVousAdapter(d, COULEURS_STATUT));
                }
            }
            catch (Exception)
            {
                // TODO
                throw;
            }
        }

        // Méthode pour savoir si on peut ajouter des disponibilités
        private bool CanAjouterDispos(object obj)
        {
            return DateJour >= DateTime.Today && ValiderDebutPlage() && ValiderFinPlage();
        }
        
        // Méthode pour valider le début de la plage d'ajout
        private bool ValiderDebutPlage()
        {
            ErreurDebutPlage = (DateJour == DateTime.Today) && (DebutPlageAjout.TimeOfDay < DateTime.Now.TimeOfDay);
            return !ErreurDebutPlage;
        }

        // Méthode pour valider la fin de la plage d'ajout
        private bool ValiderFinPlage()
        {
            ErreurFinPlage = FinPlageAjout.TimeOfDay < DebutPlageAjout.AddMinutes(DureeRDV).TimeOfDay;
            return !ErreurFinPlage;
        }

        // Méthode pour modifier une disponibilité
        private void ModifierDispo(object obj)
        {
            //==================================
            // TEST TEST TEST

            Client client = new Client();
            client.nom_client = "Flouflou";
            client.prenom_client = "Alain";

            Rendezvous rv = new Rendezvous();
            rv.debut_rdv = DateJour.AddHours(10).AddMinutes(30);
            rv.fin_rdv = DateJour.AddHours(11).AddMinutes(30);
            rv.Client = client;
            rv.Typerdv = TypeRDV;
            rv.statut_rdv = "RDV";
            rv.Employe = ApplicationVueModele.Instance.EmployeConnecte;

            ListeDisponibilites.Add(new RendezVousAdapter(rv, COULEURS_STATUT));
            Console.WriteLine("Nb Dispos: {0}", ListeDisponibilites.Count);

            //==================================
            //CollectionViewSource.GetDefaultView(ListeDisponibilites).Refresh();
        }

        // Méthode pour savoir si on peut modifier une disponibilité
        private bool CanModifierDispo(object obj)
        {
            return true;
        }

        // Méthode pour supprimer des disponibilités
        private void SupprimerDispos(object obj)
        {

        }

        // Méthode pour savoir si on peut supprimer des disponibilités
        private bool CanSupprimerDispos(object obj)
        {
            return true;
        }

        // Méthode pour aller à la page d'affichage des rendez-vous
        protected void ChargerConsultationRDV(object obj)
        {
            ApplicationVueModele app = ApplicationVueModele.Instance;
            app.ChangePageCommand.Execute(Pages.AFFICHAGE_RDV);
        }

    }
}

