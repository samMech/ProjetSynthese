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
        // Attributs
        private int _heure;
        private int _minutes;

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
                        new FrameworkPropertyMetadata(DateTime.Today, OnHeureJourPropertyChanged));
        // Encapsulation
        public DateTime HeureJour
        {        
            get { return (DateTime)GetValue(HeureJourProperty); }
            set { SetValue(HeureJourProperty, value); }
        }

        // Méthode pour gérer le changement de valeur
        private static void OnHeureJourPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            TimePicker control = source as TimePicker;
            control.HeureJourInterne = (DateTime)e.NewValue;
        }
        //==================================================================================

        /// <summary>
        /// L'heure complète
        /// </summary>
        private DateTime HeureJourInterne
        {
            get
            {
                DateTime heureJour = DateTime.Today;
                heureJour.AddHours(_heure);
                heureJour.AddMinutes(_minutes);
                return heureJour;
            }
            set
            {
                if (value != null)
                {
                    Heure = value.Hour;
                    Minutes = value.Minute;
                }
                else
                {
                    Heure = 0;
                    Minutes = 0;
                }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// L'heure
        /// </summary>
        public int Heure
        {
            get { return _heure; }
            set
            {
                if (value >= 0 && value < 24)
                {
                    _heure = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Les minutes
        /// </summary>
        public int Minutes
        {
            get { return _minutes; }
            set
            {
                if (value >= 0 && value < 60)
                {
                    _minutes = value;
                    OnPropertyChanged();
                }
            }
        }

    }
}