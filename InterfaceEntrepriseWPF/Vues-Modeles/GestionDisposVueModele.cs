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
        private int _dureeDispoModifiee;
        private bool _isBoutonModifieActif;
        private Typerdv _typeDispoModifie;
        private DateTime _dateDispoModifie;
        private DateTime _debutDispoModifie;

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
        private bool _erreurDebutDispo;
        private bool _erreurConflitDispoModifiee;

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
        /// La durée modifiée de la disponibilité sélectionnée
        /// </summary>
        public int DureeDispoModifiee
        {
            get { return _dureeDispoModifiee; }
            set
            {
                if (DureesRDV.Contains(value) && value != _dureeDispoModifiee)
                {
                    _dureeDispoModifiee = value;
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
        /// Le type modifié de la disponibilité sélectionnée
        /// </summary>
        public Typerdv TypeDispoModifie
        {
            get { return _typeDispoModifie; }
            set
            {
                if (value != null && value.Equals(_typeDispoModifie) == false)
                {
                    _typeDispoModifie = ListeTypesRDV.Where(x => x.id_typerdv == value.id_typerdv).Single();
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

        /// <summary>
        /// La date modifiée de la disponibilité sélectionnée
        /// </summary>
        public DateTime DateDispoModifie
        {
            get { return _dateDispoModifie; }
            set
            {
                if (value != null && value.Equals(_dateDispoModifie) == false)
                {
                    _dateDispoModifie = value;
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
        /// Le début de la disponibilité modifiée
        /// </summary>
        public DateTime DebutDispoModifie
        {
            get { return _debutDispoModifie; }
            set
            {
                if (value != null && value.Equals(_debutDispoModifie) == false)
                {
                    _debutDispoModifie = value;
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
                if (value != _erreurDebutPlage)
                {
                    _erreurDebutPlage = value;
                    OnPropertyChanged();
                }
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
                if (value != _erreurFinPlage)
                {
                    _erreurFinPlage = value;
                    OnPropertyChanged();
                }                
            }
        }

        /// <summary>
        /// Est-ce que le début de la disponibilité modifiée est valide
        /// </summary>
        public bool ErreurDebutDispo
        {
            get { return _erreurDebutDispo; }
            set
            {
                if (value != _erreurDebutDispo)
                {
                    _erreurDebutDispo = value;
                    OnPropertyChanged();
                }                
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
                if (value != _isDispoConflitClient)
                {
                    _isDispoConflitClient = value;
                    OnPropertyChanged();
                }                
            }
        }
                
        /// <summary>
        /// Est-ce qu'un conflit a empêché la modification d'une disponibilité
        /// </summary>
        public bool ErreurConflitDispoModifiee
        {
            get { return _erreurConflitDispoModifiee; }
            set
            {
                if (value != _erreurConflitDispoModifiee)
                {
                    _erreurConflitDispoModifiee = value;
                    OnPropertyChanged();
                }
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

                    // Ajustement des valeurs modifiées
                    DateDispoModifie = _dispoSelectionnee.debut_rdv;
                    DebutDispoModifie = _dispoSelectionnee.debut_rdv;
                    DureeDispoModifiee = (int) (_dispoSelectionnee.fin_rdv - _dispoSelectionnee.debut_rdv).TotalMinutes;
                    TypeDispoModifie = _dispoSelectionnee.Typerdv;                    
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
        /// Est-ce que le bouton de modification est activé
        /// </summary>
        public bool IsBoutonModifierActif
        {
            get { return _isBoutonModifieActif; }
            set
            {
                if (value != _isBoutonModifieActif)
                {
                    _isBoutonModifieActif = value;
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
            ListeDisponibilites = ConversionUtil.ConvertirRDVToIRDV(RestDao.GetDisposEmploye(emp.id_employe, DateJour), COULEURS_STATUT);
        }

        // Méthode pour ajouter des disponibilités
        private void AjouterDispos(object obj)
        {
            try
            {
                // Calcul de la date de début et de fin
                DateTime dateDebut = DateJour.Date + DebutPlageAjout.TimeOfDay;
                DateTime dateFin = dateDebut + (FinPlageAjout - DebutPlageAjout);

                // Insertion
                Employe emp = ApplicationVueModele.Instance.EmployeConnecte;
                List<Rendezvous> disposAjoutees = RestDao.AjouterDispos(emp.id_employe, dateDebut, dateFin, DureeRDV, TypeRDV.id_typerdv);

                // Mise à jour des données
                UpdateData();
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
            // Ré-initialisation
            ErreurDebutPlage = false;
            ErreurFinPlage = false;

            return DateJour >= DateTime.Today && ValiderDebutPlage() && ValiderFinPlage();
        }
        
        // Méthode pour valider le début de la plage d'ajout
        private bool ValiderDebutPlage()
        {
            ErreurDebutPlage = (DateJour <= DateTime.Today) && (DebutPlageAjout.TimeOfDay < DateTime.Now.TimeOfDay);
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
                // Ré-initialisation
                ErreurConflitDispoModifiee = false;

                // Récupération de l'empoyé courant
                Employe emp = ApplicationVueModele.Instance.EmployeConnecte;

                // Calcul de la nouvelle date de début et de fin
                DateTime newDateDebut = DateDispoModifie.Date + DebutDispoModifie.TimeOfDay;
                DateTime newDateFin = newDateDebut.AddMinutes(DureeDispoModifiee);

                // Modification
                Rendezvous dispoModifiee = RestDao.ModifierDispo(emp.id_employe, DispoSelectionnee.id_rdv,
                    newDateDebut, newDateFin, TypeDispoModifie.id_typerdv, Raison);

                // Vérification
                if (dispoModifiee != null)
                {
                    ListeDisponibilites.Remove(ListeDisponibilites.Where(x => x.IsSelectionne).Single());
                    ListeDisponibilites.Add(new RendezVousAdapter(dispoModifiee));
                }
                else
                {
                    ErreurConflitDispoModifiee = true;
                }
            }
            catch (Exception)
            {
                // TODO
                throw;
            }
        }

        // Méthode pour valider le début de la disponibilité modifiée
        private bool ValiderDebutDispoModifie()
        {
            ErreurDebutDispo = IsBoutonModifierActif && (DateDispoModifie.Add(DebutDispoModifie.TimeOfDay) < DateTime.Now);
            if (DispoSelectionnee != null)
            {
                ErreurDebutDispo = ErreurDebutDispo && (DebutDispoModifie.TimeOfDay != DispoSelectionnee.debut_rdv.TimeOfDay);
            }
            return !ErreurDebutDispo;
        }
        
        // Méthode pour savoir si on peut modifier une disponibilité
        private bool CanModifierDispo(object obj)
        {
            // On vérifie qu'une seule disponibilité est sélectionnée
            List<CalendrierRDV.IRendezVous> dispos = ListeDisponibilites.Where(x => x.IsSelectionne).ToList();
            if (dispos.Count == 1)
            {
                // On signale l'activation
                IsBoutonModifierActif = true;
                
                // Ré-initialisation
                IsDispoConflitClient = false;
                ErreurDebutDispo = false;

                // On met à jour la disponibilité sélectionnée
                DispoSelectionnee = ((RendezVousAdapter)dispos[0]).RDV;

                // On vérifie si la disponibilité est déjà réservée
                IsDispoConflitClient = (DispoSelectionnee.Client != null);            
            }
            else
            {
                IsBoutonModifierActif = false;
                ErreurDebutDispo = false;// Pour désactiver l'erreur si le panneau n'est pas actif
            }

            return ValiderDebutDispoModifie() && !IsDispoConflitClient;
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
            // On vérifie qu'au moins une disponibilité est sélectionnée
            List<CalendrierRDV.IRendezVous> dispos = ListeDisponibilites.Where(x => x.IsSelectionne).ToList();
            if (dispos.Count > 0)
            {
                // On vérifie si une des disponibilités est déjà réservée
                IsDispoConflitClient = (dispos.FirstOrDefault(x => x.NomClient != null) != null);
                return !IsDispoConflitClient;
            }
            else
            {
                // Ré-initialisation
                IsDispoConflitClient = false;
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

