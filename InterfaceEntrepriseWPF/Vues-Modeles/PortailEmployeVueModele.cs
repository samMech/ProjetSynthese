using InterfaceEntrepriseWPF.Modele;
using ProjetRDV247.Modele;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace InterfaceEntrepriseWPF.Vues_Modeles
{
    /// <summary>
    /// Classe vue-modèle pour le portail employé
    /// </summary>
    class PortailEmployeVueModele : VueModele
    {
        // Attributs
        private string _texteLabelTitre = "";        
        private ObservableCollection<Rendezvous> _listeRendezVous = new ObservableCollection<Rendezvous>();

        private ICommand _consulterRDVCommand;
        private ICommand _gererDisposCommand;

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public PortailEmployeVueModele()
        {
            // Ajustement du titre de la fenêtre
            TitrePage = "Portail Employé";

            // Ajustement du titre
            TexteLabelTitre = String.Format("Bienvenue ! Voici vos rendez-vous pour aujourd'hui le {0}", DateTime.Now.ToString("dd MMMM yyyy"));            
        }
        
        //============//
        // Propriétés //
        //============//

        /// <summary>
        /// Le texte du label titre
        /// </summary>
        public string TexteLabelTitre
        {
            get { return _texteLabelTitre; }
            set
            {
                if (!_texteLabelTitre.Equals(value))
                {
                    _texteLabelTitre = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// La liste des rendez-vous de l'employé pour la journée
        /// </summary>
        public ObservableCollection<Rendezvous> ListeRendezVous
        {
            get
            {
                return _listeRendezVous;
            }
            set
            {                
                if (value != null)
                {
                    _listeRendezVous = value;
                    OnPropertyChanged();
                }
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
            ListeRendezVous = new ObservableCollection<Rendezvous>(RestDao.GetRendezVousEmploye(emp.id_employe));
        }

        // Méthode pour charger la page d'affichage des rendez-vous
        private void ChargerConsultationRDV(object obj)
        {
            ApplicationVueModele app = ApplicationVueModele.Instance;
            app.ChangePageCommand.Execute(Pages.AFFICHAGE_RDV);
        }

        // Méthode pour charger la page de gestion des disponibilités           
        private void ChargerGestionDispo(object obj)
        {
            ApplicationVueModele app = ApplicationVueModele.Instance;
            app.ChangePageCommand.Execute(Pages.GESTION_DISPOS);
        }


    }
}
