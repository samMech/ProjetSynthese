using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ProjetRDV247.Modele;

namespace ProjetRDV247.Controle
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "ServiceRDV247" à la fois dans le code, le fichier svc et le fichier de configuration.
    // REMARQUE : pour lancer le client test WCF afin de tester ce service, sélectionnez ServiceRDV247.svc ou ServiceRDV247.svc.cs dans l'Explorateur de solutions et démarrez le débogage.
    public class ServiceRDV247 : IServiceRDV247
    {
        // Le data entity
        private RDV247Entities bd;

        public string TestREST()
        {
            return "Hello World !";
        }

        //============================================================================================================================

        public rendezvous AjouterDispo(employe employe, DateTime date, TimeSpan debut, TimeSpan fin, TimeSpan dureeRDV)
        {
            //bd = new RDV247Entities();
            throw new NotImplementedException();
        }

        public bool AnnulerDispo(employe employe, rendezvous dispo, string raison)
        {
            throw new NotImplementedException();
        }              

        public bool AnnulerRDV(client client, rendezvous rdv)
        {
            throw new NotImplementedException();
        }

        public List<rendezvous> GetDispoEmploye(int idEmploye, DateTime date)
        {
            throw new NotImplementedException();
        }

        public List<employe> GetEmployes()
        {
            throw new NotImplementedException();
        }

        public List<rendezvous> GetRDVClient(int idClient)
        {
            throw new NotImplementedException();
        }

        public List<rendezvous> GetRDVEmploye(int idEmploye, DateTime date)
        {
            throw new NotImplementedException();
        }

        public rendezvous ModifierDispo(employe employe, rendezvous dispo, DateTime newdate, TimeSpan newdebut, TimeSpan newfin, TimeSpan newdureeRDV, string raison)
        {
            throw new NotImplementedException();
        }

        public rendezvous PrendreRDV(client client, rendezvous rdv)
        {
            throw new NotImplementedException();
        }

    }
}
