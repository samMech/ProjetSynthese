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
        private string _raison;
        private bool _isDispoConflitClient;
        private Rendezvous _dispoSelectionnee;
        private int _dureeRDVModifiee;

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

        // Booléen pour empêcher la mise à jour des valeurs plusieurs fois durant l'initialisation
        private bool initTermine = false;

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
            Raison = "";
            IsDispoConflitClient = false;

            // Récupération de la liste des type de rendez-vous de l'employé
            Employe emp = ApplicationVueModele.Instance.EmployeConnecte;
            ListeTypesRDV = emp.Typerdv.ToList();
            TypeRDV = ListeTypesRDV[0];

            // Initialisation terminée
            initTermine = true;
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
                if (DureesRDV.Contains(value) && value != _dureeRDV)
                {
                    _dureeRDV = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// La durée modifiée du rendez-vous
        /// </summary>
        public int DureeRDVModifiee
        {
            get { return _dureeRDVModifiee; }
            set
            {
                if (DureesRDV.Contains(value) && value != _dureeRDVModifiee)
                {
                    _dureeRDVModifiee = value;
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
                if (value != null && value.Equals(_typeRDV) == false)
                {
                    _typeRDV = value;
                    OnPropertyChanged();
                }                
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
                if(value != null && value.Equals(_dateJour) == false)
                {   
                    // Vérification pour savoir si on a changé de semaine
                    if (initTermine && Math.Abs((value - CalendrierRDV.Utilitaire.TrouverLundiPrecedent(_dateJour)).Days) > 6)
                    {
                        // Mise à jour des données
                        UpdateData();
                    }
                    
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
                if (value != null && value.Equals(_debutPlageAjout) == false)
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
                if (value != null && value.Equals(_finPlageAjout) == false)
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
                if (value != null && value.Equals(_raison) == false)
                {
                    _raison = value;
                    OnPropertyChanged();
                }                
            }
        }

        /// <summary>
        /// Est-ce qu'une disponibilité sélectionnée est déjà réservée pour un client
        /// </summary>
        public bool IsDispoConflitClient
        {
            get { return _isDispoConflitClient; }
            set
            {
                _isDispoConflitClient = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// La disponibilité sélectionnée pour modification
        /// </summary>
        public Rendezvous DispoSelectionnee
        {
            get { return _dispoSelectionnee; }
            set
            {
                if (value != null && value.Equals(_dispoSelectionnee) == false)
                {
                    _dispoSelectionnee = value;                    
                    OnPropertyChanged();

                    // Ajustement de la valeur dans le combobox
                    DureeRDVModifiee = (int)(_dispoSelectionnee.fin_rdv - _dispoSelectionnee.debut_rdv).TotalMinutes;
                }
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
                if (value != null && value.Equals(_listeDisponibilites) == false)
                {
                    _listeDisponibilites = value;
                    OnPropertyChanged();
                }                
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
                if (value != null && value.Equals(_listeTypeRDV) == false)
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
            // Récupération des disponibilités pour la semaine courante
            Employe emp = ApplicationVueModele.Instance.EmployeConnecte;
            ListeDisponibilites = ConversionUtil.ConvertirRDVToIRDV(RestDao.GetDisposEmploye(emp.id_employe), COULEURS_STATUT);
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
            try
            {
                // Récupération du rendez-vous sélectionné

                // 


            }
            catch (Exception)
            {
                // TODO
                throw;
            }
        }

        // Méthode pour savoir si on peut modifier une disponibilité
        private bool CanModifierDispo(object obj)
        {
            // Ré-initialisation
            IsDispoConflitClient = false;

            // On vérifie qu'une seule disponibilité est sélectionnée
            List<CalendrierRDV.IRendezVous> dispos = ListeDisponibilites.Where(x => x.IsSelectionne).ToList();
            if (dispos.Count == 1)
            {
                // On met à jour la disponibilité sélectionnée
                DispoSelectionnee = ((RendezVousAdapter)dispos[0]).RDV;

                // On vérifie si la disponibilité est déjà réservée
                IsDispoConflitClient = (DispoSelectionnee.Client != null);
                return !IsDispoConflitClient;
            }
            return false;            
        }

        // Méthode pour supprimer des disponibilités
        private void SupprimerDispos(object obj)
        {
            try
            {
                // Récupération des id des disponibilités à supprimer
                List<int> listeIdDispos = ListeDisponibilites.Where(x => x.IsSelectionne).Select(x => x.ID).ToList();
                if (listeIdDispos.Count > 0)
                {
                    // Suppression
                    Employe emp = ApplicationVueModele.Instance.EmployeConnecte;
                    RestDao.SupprimerDispos(emp.id_employe, listeIdDispos, Raison);

                    // Mise à jour des données
                    UpdateData();
                }
            }
            catch (Exception)
            {
                // TODO
                throw;
            }
        }

        // Méthode pour savoir si on peut supprimer des disponibilités
        private bool CanSupprimerDispos(object obj)
        {
            // Ré-initialisation
            IsDispoConflitClient = false;

            // On vérifie qu'au moins une disponibilité est sélectionnée
            List<CalendrierRDV.IRendezVous> dispos = ListeDisponibilites.Where(x => x.IsSelectionne).ToList();
            if (dispos.Count > 0)
            {
                // On vérifie si une des disponibilités est déjà réservée
                IsDispoConflitClient = (dispos.FirstOrDefault(x => x.NomClient != null) != null);
                return !IsDispoConflitClient;
            }
            return false;
        }

        // Méthode pour aller à la page d'affichage des rendez-vous
        protected void ChargerConsultationRDV(object obj)
        {
            ApplicationVueModele app = ApplicationVueModele.Instance;
            app.ChangePageCommand.Execute(Pages.AFFICHAGE_RDV);
        }

    }
}

