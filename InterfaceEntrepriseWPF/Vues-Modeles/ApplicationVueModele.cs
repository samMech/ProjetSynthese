using InterfaceEntrepriseWPF.Vues;
using ProjetRDV247.Modele;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace InterfaceEntrepriseWPF.Vues_Modeles
{
    /// <summary>
    /// Classe vue-modèle singleton pour toute l'application
    /// </summary>
    sealed class ApplicationVueModele : VueModele
    {
        // Constante pour le nom de l'application
        private const string NOM_APPLICATION = "Rendez-vous 24/7";

        // Instance courante et objet pour avoir un singleton "thread safe"
        private static volatile ApplicationVueModele _instance;
        private static object syncRoot = new Object();

        // Attributs
        private ICommand _changePageCommand;
        private VueModele _pageCourante = null;
        private VueModele _pagePrecedente = null;
        private Dictionary<Pages, VueModele> pages;

        // L'employé connecté
        private Employe _employeConnecte = null;
        
        /// <summary>
        /// Constructeur privé
        /// </summary>
        private ApplicationVueModele()
        {
            // Création du dictionnaire pour contenir les pages
            pages = new Dictionary<Pages, VueModele>();

            // Initialisation de la page d'accueil
            ChangerPageCourante(Pages.CONNEXION);
        }

        //============//
        // Propriétés //
        //============//

        /// <summary>
        /// Retourne l'instance courante due ViewModele de l'application
        /// </summary>
        public static ApplicationVueModele Instance
        {
            get
            {
                // Double-checked locking (pour garantir le "thread safe" à faible coût)
                if (_instance == null)
                {
                    // Locking --> opération très couteuse ! (Fait juste la première fois)
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new ApplicationVueModele();
                        }                            
                    }
                }

                return _instance;
            }
        }

        /// <summary>
        /// Commande pour changer de page
        /// </summary>
        public ICommand ChangePageCommand
        {
            get
            {
                if (_changePageCommand == null)
                {                 
                    // Création de la commande si elle n'existe pas encore
                    _changePageCommand = new RelayCommand(
                        p => ChangerPageCourante((Pages)p),
                        p => p is VueModele);
                }
                return _changePageCommand;
            }
        }
        
        /// <summary>
        /// La page courante pour le binding
        /// </summary>
        public VueModele PageCourante
        {
            get
            {
                return _pageCourante;
            }
            set
            {
                if (_pageCourante != value)
                {
                    _pageCourante = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// La page précédente
        /// </summary>
        public Pages PagePrecedente
        {
            get
            {
                if (_pagePrecedente == null)
                {
                    return Pages.NONE;
                }
                else
                {
                    return pages.First(x => x.Value.Equals(_pagePrecedente)).Key;
                }                
            }
        }

        /// <summary>
        /// L'employé connecté
        /// </summary>
        public Employe EmployeConnecte
        {
            get
            {
                return _employeConnecte;
            }
            set
            {
                if (! Object.Equals(_employeConnecte, value))
                {
                    _employeConnecte = value;
                    OnPropertyChanged();
                }
            }
        }
        
        //==========//
        // Méthodes //
        //==========//

        // Méthode pour changer de page
        private void ChangerPageCourante(Pages page)
        {
            // Si la page n'existe pas encore, on l'ajoute     
            if (! pages.ContainsKey(page))
            {   
                switch (page)
                {
                    case Pages.CONNEXION:
                        pages[page] = new ConnexionVueModele();
                        break;
                    case Pages.PORTAIL:
                        pages[page] = new PortailEmployeVueModele();
                        break;
                    case Pages.AFFICHAGE_RDV:
                        pages[page] = new AffichageRDVVueModele();
                        break;
                    case Pages.GESTION_DISPOS:
                        pages[page] = new GestionDisposVueModele();
                        break;
                    default:
                        break;
                }                
            }

            // On remplace le contenu de la fenêtre par la nouvelle page
            _pagePrecedente = PageCourante;
            PageCourante = pages[page];

            // Mise à jour des données
            this.UpdateData();
            PageCourante.UpdateData();            
        }

        /// <summary>
        /// Mise à jour des données
        /// </summary>
        public override void UpdateData()
        {
            // Ajustement du titre de la fenêtre
            string nouveauTitre = String.Format("{0} ({1})", NOM_APPLICATION, PageCourante.TitrePage);
            if (EmployeConnecte != null)
            {
                nouveauTitre += String.Format(" - {1} {0}", EmployeConnecte.nom_employe, EmployeConnecte.prenom_employe);
            }
            TitrePage = nouveauTitre;            
        }

    }
}
