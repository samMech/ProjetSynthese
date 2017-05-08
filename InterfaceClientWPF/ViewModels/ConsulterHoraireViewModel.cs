using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceClientWPF.Model;
using ProjetRDV247.Modele;

namespace InterfaceClientWPF.ViewModels
{
    class ConsulterHoraireViewModel:VueModele
    {
        private ObservableCollection<Employe> _listEmployes;

        public ConsulterHoraireViewModel()
        {
            TitrePage = "Consulter les Horaires";
        }

        public ObservableCollection<Employe> ListeEmployes
        {
            get
            {
                return _listEmployes;
            }
            set
            {
                if (value != null)
                {
                    _listEmployes = value;
                    OnPropertyChanged();
                }
            }
        }

        public override void UpdateData()
        {
            ListeEmployes = new ObservableCollection<Employe>(RestDao.GetEmployes());
        }
    }
}
