using CalendrierRDV;
using ProjetRDV247.Modele;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceEntrepriseWPF.Modele
{
    /// <summary>
    /// Adapter pour utiliser un rendez-vous avec l'interface IRendezVous
    /// </summary>
    public class RendezVousAdapter: CalendrierRDV.IRendezVous
    {
        // Le rendez
        private Rendezvous _rdv;
        
        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public RendezVousAdapter(Rendezvous rdv)
        {
            RDV = rdv;
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

        //===============================//
        // Implémentation de l'interface //
        //===============================//

        public TimeSpan Duree
        {
            get { return _rdv.fin_rdv - _rdv.debut_rdv; }
        }

        public int ID
        {
            get { return _rdv.id_rdv; }
        }

        public DateTime JourHeure
        {
            get { return _rdv.debut_rdv; }
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
    }
}
