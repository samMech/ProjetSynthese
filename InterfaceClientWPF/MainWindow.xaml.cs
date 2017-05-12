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
using InterfaceClientWPF.ViewModels;

namespace InterfaceClientWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = ApplicationViewModel.Instance;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.PreviousSize != e.NewSize)
            {
                // Récupération des dimensions de l'écran
                double width = SystemParameters.WorkArea.Width;
                double height = SystemParameters.WorkArea.Height;

                // Ajustement des dimmensions pour rester dans l'écran
                double newWidth = e.NewSize.Width > width ? width : e.NewSize.Width;
                double newHeight = e.NewSize.Height > height ? height : e.NewSize.Height;

                // Recentrage de la fenêtre
                this.Left = (width - newWidth) / 2;
                this.Top = (height - newHeight) / 2;
            }
        }
    }
}
