using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using InterfaceClientWPF.Model;
using ProjetRDV247.Modele;

namespace InterfaceClientWPF.ViewModels
{
    class PortailClientViewModel : VueModele
    {
        private ObservableCollection<Rendezvous> _listeRendezvous = new ObservableCollection<Rendezvous>();
        private ICommand _consulterDisposCommand;

        public PortailClientViewModel()
        {
            TitrePage = "Portail Client";
        }

        public ObservableCollection<Rendezvous> ListeRendezvous
        {
            get
            {
                return _listeRendezvous;
            }
            set
            {
                if (value != null)
                {
                    _listeRendezvous = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand ConsulterDisposCommand
        {
            get
            {
                if (_consulterDisposCommand == null)
                {
                    _consulterDisposCommand = new RelayCommand(ChargerDispos);
                }

                return _consulterDisposCommand;
            }
        }

        private void ChargerDispos(object obj)
        {
            ApplicationViewModel app = ApplicationViewModel.Instance;
            app.ChangePageCommand.Execute(new ConsulterHoraireViewModel());
        }

        public override void UpdateData()
        {
            Client cl = ApplicationViewModel.Instance.ClientConnecte;
            ListeRendezvous = new ObservableCollection<Rendezvous>(RestDao.GetRendezvousClient(cl.id_client));
        }
    }
}
