using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ComposantsUtils
{
    /// <summary>
    /// Logique d'interaction pour TimePicker.xaml
    /// </summary>
    public partial class TimePicker : UserControl, INotifyPropertyChanged
    { 
        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public TimePicker()
        {
            InitializeComponent();
        }

        // Événement pour les notifications
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Méthode pour déclencher l'événement de notification du changement d'un propriété
        /// </summary>
        /// <param name="nomPropriete">Le nom de la propriété qui a changée</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string nomPropriete = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nomPropriete));
        }

        //============//
        // Propriétés //
        //============//
        
        //==================================================================================
        // Propriété de dépendance pour l'heure du jour
        public static readonly DependencyProperty HeureJourProperty =
                    DependencyProperty.Register("HeureJour",
                        typeof(DateTime), typeof(TimePicker),
                        new FrameworkPropertyMetadata(DateTime.Today, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        
        // Encapsulation (interne: NE PAS TOUCHER !)
        public DateTime HeureJour
        {        
            get { return (DateTime)GetValue(HeureJourProperty); }
            set { SetValue(HeureJourProperty, value); }
        }
        //==================================================================================

    }
}