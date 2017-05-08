using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ProjetRDV247.Modele;
using System.Net;
using System.ServiceModel.Web;

namespace ProjetRDV247.Controle
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ServiceConnexion" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ServiceConnexion.svc or ServiceConnexion.svc.cs at the Solution Explorer and start debugging.
    public class ServiceConnexion : IServiceConnexion
    {
        public string DoWork()
        {
            return "Hello World !";
        }

        public bool CreerClient(string nom_client, string prenom_client, string telephone_client, string courriel_client, string password_client)
        {
            using (RDV247Entities bd = new RDV247Entities())
            {
                ObjectParameter responseMessage = new ObjectParameter("responseMessage", typeof(string));
                ObjectParameter responseCode = new ObjectParameter("responseCode", typeof(int));

                bd.ajouterClient(nom_client, prenom_client, telephone_client, courriel_client, password_client,
                    responseMessage, responseCode);

                return (int)responseCode.Value==1;
            }
        }

        public Client AuthentifierClient(string login, string password)
        {

            using (RDV247Entities bd = new RDV247Entities())
            {
                ObjectParameter responseMessage = new ObjectParameter("responseMessage", typeof(string));
                ObjectParameter responseCode = new ObjectParameter("responseCode", typeof(int));
                ObjectParameter idClient = new ObjectParameter("id_Client", typeof(int));

                bd.authentifierClient(login, password, responseMessage, responseCode, idClient);
                Dao dao = new Dao();
                Client client = null;

                if ((int)responseCode.Value == 1)
                {
                    client = dao.GetClientById((int)idClient.Value);
                }

                return client;
            }
        }

        public Employe AuthentifierEmp(string login, string password)
        {
            using (RDV247Entities bd = new RDV247Entities())
            {
                ObjectParameter responseMessage = new ObjectParameter("responseMessage", typeof(string));
                ObjectParameter responseCode = new ObjectParameter("responseCode", typeof(int));
                ObjectParameter idEmploye = new ObjectParameter("id_Employe", typeof(int));

                bd.authentifierEmploye(login, password, responseMessage, responseCode, idEmploye);

                Dao dao = new Dao();
                Employe employe = null;
                
                if ((int)responseCode.Value == 1)
                {
                    employe = dao.GetEmployeById((int)idEmploye.Value);                    
                }
                
                return employe;
            }
        }
    }
}
