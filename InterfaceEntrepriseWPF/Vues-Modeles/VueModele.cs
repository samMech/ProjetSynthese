using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceEntrepriseWPF.Vues_Modeles
{
    /// <summary>
    /// Classe abstraite pour toutes les VuesModeles
    /// </summary>
    public abstract class VueModele : INotifyPropertyChanged
    {
        // Attributs
        private string _titrePage = "";

        /// <summary>
        /// Événement à déclencher quand une propriété change
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Méthode pour déclencher l'événement de notification du changement d'un propriété
        /// </summary>
        /// <param name="nomPropriete">Le nom de la propriété qui a changée</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string nomPropriete = null)
        {
            // Si l'événement n'est pas null
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nomPropriete));
        }
        
        /// <summary>
        /// Le titre de la page
        /// </summary>
        public string TitrePage
        {
            get
            {
                return _titrePage;
            }
            set
            {
                if (! _titrePage.Equals(value))
                {
                    _titrePage = value;
                    OnPropertyChanged();
                }                
            }
        }

    }
}
