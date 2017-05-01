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
            this.DataContext = new ApplicationVueModele();
        }

        /// <summary>
        /// Méthode pour changer la "page" affichée
        /// </summary>
        /// <param name="nextPage">La nouvelle "page"</param>
        public void Navigate(UserControl nextPage)
        {
            this.Content = nextPage;
        }

        /// <summary>
        /// Méthode pour changer la "page" affichée
        /// </summary>
        /// <param name="nextPage">La nouvelle "page"</param>
        /// <param name="state">L'état contenant les informations à transférer à la nouvelle "page"</param>
        public void Navigate(UserControl nextPage, object state)
        {
            this.Content = nextPage;
            ISwitchable s = nextPage as ISwitchable;

            if (s != null)
            {
                s.UtilizeState(state);
            }
            else
            {
                throw new ArgumentException("NextPage is not ISwitchable! " + nextPage.Name.ToString());
            }                
        }
    }
}
