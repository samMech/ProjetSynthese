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
        #region IRestService Members

        public string TestREST()
        {
            return "Hello World !";
        }

        public List<employe> GetEmployes()
        {
            throw new NotImplementedException();
        }

        public List<rendezvous> GetDispoEmploye(employe emp, DateTime date)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
