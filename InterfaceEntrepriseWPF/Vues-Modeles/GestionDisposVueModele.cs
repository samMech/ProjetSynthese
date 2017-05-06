using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InterfaceEntrepriseWPF.Vues_Modeles
{
    class GestionDisposVueModele : VueModele
    {
        // Attributs
        private static readonly List<int> _dureesRDV = new List<int>(new int[]{10, 15, 20, 30, 45, 60, 90, 120});
        private DateTime _dateJour;
        private DateTime _debutPlageAjout;
        private DateTime _finPlageAjout;
        
        // Commandes
        private ICommand _ajouterDisposCommand;
        private ICommand _modifierDispoCommand;
        private ICommand _supprimerDisposCommand;

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public GestionDisposVueModele()
        {
            // Ajustement du titre de la fenêtre
            TitrePage = "Gestion des disponibilités";            
        }

        //============//
        // Propriétés //
        //============//

        /// <summary>
        /// Les durées permises pour un rendez-vous
        /// </summary>
        public List<int> DureesRDV
        {
            get { return _dureesRDV; }            
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

        //==========//
        // Méthodes //
        //==========//

        // Méthode pour ajouter des disponibilités
        private void AjouterDispos(object obj)
        {

        }

        // Méthode pour savoir si on peut ajouter des disponibilités
        private bool CanAjouterDispos(object obj)
        {
            return DateJour >= DateTime.Today && DebutPlageAjout.TimeOfDay >= DateTime.Now.TimeOfDay;
        }

        // Méthode pour modifier une disponibilité
        private void ModifierDispo(object obj)
        {

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

    }
}
