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
        private DateTime _debutPlageAjout;
        private DateTime _finPlageAjout;

        private ICommand _returnCommand;
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
        /// Commande pour le bouton permettant de retourner à la page précédente
        /// </summary>
        public ICommand ReturnCommand
        {
            get
            {
                if (_returnCommand == null)
                {
                    // Création de la commande si elle n'existe pas encore
                    _returnCommand = new RelayCommand(RetournerPagePrecedente,
                        (x => ApplicationVueModele.Instance.PagePrecedente != null));
                }
                return _returnCommand;
            }
        }

        //==========//
        // Méthodes //
        //==========//

            // Méthode pour revenir à la page précédente
        private void RetournerPagePrecedente(object obj)
        {
            ApplicationVueModele app = ApplicationVueModele.Instance;
            app.ChangePageCommand.Execute(app.PagePrecedente);
        }

    }
}
