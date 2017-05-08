using InterfaceEntrepriseWPF.Modele;
using InterfaceEntrepriseWPF.Utilitaire;
using ProjetRDV247.Modele;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace InterfaceEntrepriseWPF.Vues_Modeles
{
    class AffichageRDVVueModele : VueModele
    {
        // Constantes
        private static readonly Dictionary<string, Color> COULEURS_STATUT = new Dictionary<string, Color>();

        // Attributs
        private DateTime _dateJour;

        // La liste des disponibilités pour le composant graphique
        private ObservableCollection<CalendrierRDV.IRendezVous> _listeRendezVous;

        // Commandes
        private ICommand _gererDisposCommand;

        /// <summary>
        /// Constructeur statique pour initialiser les constantes
        /// </summary>
        static AffichageRDVVueModele()
        {
            // TODO (prendre la liste du service web pour les status)
            COULEURS_STATUT.Add("rv", Colors.Lavender);
        }

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public AffichageRDVVueModele()
        {
            // Ajustement du titre de la fenêtre
            TitrePage = "Affichage des rendez-vous";

            // Initialisation des valeurs
            DateJour = DateTime.Today;
            ListeRendezVous = new ObservableCollection<CalendrierRDV.IRendezVous>();
        }

        //============//
        // Propriétés //
        //============//

        /// <summary>
        /// Les couleurs pour les différents statuts
        /// </summary>
        public Dictionary<string, Color> CouleurStatuts
        {
            get { return COULEURS_STATUT; }
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
        /// La liste des rendez-vous pour le composant graphique
        /// </summary>
        public ObservableCollection<CalendrierRDV.IRendezVous> ListeRendezVous
        {
            get { return _listeRendezVous; }
            set
            {
                _listeRendezVous = (value == null) ? new ObservableCollection<CalendrierRDV.IRendezVous>() : value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Commande pour le bouton permettant d'aller à la page pour gérer les disponibilités
        /// </summary>
        public ICommand GererDisposCommand
        {
            get
            {
                if (_gererDisposCommand == null)
                {
                    // Création de la commande si elle n'existe pas encore
                    _gererDisposCommand = new RelayCommand(ChargerGestionDispo);
                }
                return _gererDisposCommand;
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
            // Récupération des rendez-vous
            Employe emp = ApplicationVueModele.Instance.EmployeConnecte;
            ListeRendezVous = ConversionUtil.ConvertirRDVToIRDV(RestDao.GetRendezVousEmploye(emp.id_employe));
        }

        // Méthode pour aller à la page de gestion des disponibilités
        protected void ChargerGestionDispo(object obj)
        {
            ApplicationVueModele app = ApplicationVueModele.Instance;
            app.ChangePageCommand.Execute(Pages.GESTION_DISPOS);
        }

    }
}
