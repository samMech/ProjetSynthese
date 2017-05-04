using InterfaceEntrepriseWPF.Vues_Modeles;
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

namespace InterfaceEntrepriseWPF.Vues
{
    /// <summary>
    /// Logique d'interaction pour VuePortailEmploye.xaml
    /// </summary>
    public partial class VuePortailEmploye : UserControl
    {
        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public VuePortailEmploye()
        {
            InitializeComponent();

            // Initialisation du contexte
            this.DataContext = new PortailEmployeVueModele();
        }
    }
}
