using System;
using System.Collections.Generic;
using System.Linq;
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
using InterfaceEntrepriseWPF.Vues;
using InterfaceEntrepriseWPF.Vues_Modeles;

namespace InterfaceEntrepriseWPF
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // Initialisation du contexte
            this.DataContext = ApplicationVueModele.Instance;
        }

        // Listener pour garder la fenêtre centrée
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.PreviousSize != e.NewSize)
            {
                // Récupération des dimensions de l'écran
                double width = SystemParameters.PrimaryScreenWidth;
                double height = SystemParameters.PrimaryScreenHeight;

                // Recentrage de la fenêtre
                this.Left = (width - e.NewSize.Width) / 2;
                this.Top = (height - e.NewSize.Height) / 2;
            }
        }
    }
}
