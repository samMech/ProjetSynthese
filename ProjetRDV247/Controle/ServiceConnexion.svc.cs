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

        public bool CreerClient(Client c)
        {
            using (RDV247Entities bd = new RDV247Entities())
            {
                ObjectParameter responseMessage = new ObjectParameter("responseMessage", typeof(string));
                ObjectParameter responseCode = new ObjectParameter("responseCode", typeof(int));

                bd.ajouterClient(c.nom_client, c.prenom_client, c.telephone_client, c.courriel_client, c.password_client,
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
                ObjectParameter idClient = new ObjectParameter("idClient", typeof(int));

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
                ObjectParameter idEmploye = new ObjectParameter("idEmploye", typeof(int));

                //bd.authentifierEmploye(login, password, responseMessage, responseCode, idEmploye);                    

                //Dao dao = new Dao();
                //Employe employe = null;

                //if ((int)responseCode.Value == 1)
                //{
                //    employe = dao.GetEmployeById((int)idEmploye.Value);
                //}
                Employe employe = new Employe();
                employe.id_employe = 1;
                employe.nom_employe = "Alain";
                employe.prenom_employe = "Flouflou";
                return employe;
            }
        }
    }
}
