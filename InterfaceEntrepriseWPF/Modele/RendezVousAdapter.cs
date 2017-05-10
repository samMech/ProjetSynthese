using CalendrierRDV;
using ProjetRDV247.Modele;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace InterfaceEntrepriseWPF.Modele
{
    /// <summary>
    /// Adapter pour utiliser un rendez-vous avec l'interface IRendezVous
    /// </summary>
    public class RendezVousAdapter: CalendrierRDV.IRendezVous, INotifyPropertyChanged
    {
        // Attributs
        private Rendezvous _rdv;
        private Boolean _isSelectionne;
        private Dictionary<string, Color> _couleursStatut;
        
        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public RendezVousAdapter(Rendezvous rdv)
        {
            this.RDV = rdv;
            this.IsSelectionne = false;
            this.CouleursStatut = new Dictionary<string, Color>();
        }

        /// <summary>
        /// Constructeur avec paramètres
        /// </summary>
        public RendezVousAdapter(Rendezvous rdv, Dictionary<string, Color> couleursStatus)
        {
            this.RDV = rdv;
            this.IsSelectionne = false;
            this.CouleursStatut = couleursStatus;
        }

        // Propriété pour changer le rendez-vous
        public Rendezvous RDV
        {
            get { return _rdv; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                
                _rdv = value;
            }
        }

        // Propriété pour la liste des couleurs
        public Dictionary<string, Color> CouleursStatut
        {
            get { return _couleursStatut; }
            set
            {
                if (value != null)
                {
                    _couleursStatut = value;
                }
            }
        }
        
        // Événement pour notifier la vue quand une propriété change
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string nomPropriete = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nomPropriete));
        }

        //===============================//
        // Implémentation de l'interface //
        //===============================//

        public int ID
        {
            get { return _rdv.id_rdv; }
        }

        public DateTime Debut
        {
            get { return _rdv.debut_rdv; }
        }

        public DateTime Fin
        {
            get { return _rdv.fin_rdv; }
        }

        public string NomClient
        {
            get { return (_rdv.Client == null) ? null : _rdv.Client.nom_client; }
        }

        public string Statut
        {
            get { return _rdv.statut_rdv; }
        }

        public string Type
        {
            get { return (_rdv.Typerdv == null) ? null : _rdv.Typerdv.nom_typerdv; }
        }

        public SolidColorBrush CouleurRDV
        {
            get
            {
                if (CouleursStatut.ContainsKey(RDV.statut_rdv))
                {
                    return new SolidColorBrush(CouleursStatut[RDV.statut_rdv]);
                }
                else
                {
                    return new SolidColorBrush(Colors.Transparent);
                }
            }
        }
        
        public bool IsSelectionne
        {
            get { return _isSelectionne; }
            set
            {
                _isSelectionne = value;
                OnPropertyChanged();
            }
        }
    }
}
