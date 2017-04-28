using ProjetRDV247.Modele;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ProjetRDV247.Controle
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom d'interface "IServiceRDV247" à la fois dans le code et le fichier de configuration.
    [ServiceContract]
    public interface IServiceRDV247
    {
        // TEST
        //=============================================================

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "")]
         string TestREST();

        // Client
        //=============================================================

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "GetEmployes")]
        List<Employe> GetEmployes();
                
        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "GetDispoEmploye/{idEmploye}/{date}")]
        List<Rendezvous> GetDispoEmploye(int idEmploye, DateTime date);

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "GetRDVClient/{idClient}")]
        List<Rendezvous> GetRDVClient(int idClient);

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "PrendreRDV")]
        Rendezvous PrendreRDV(Client client, Rendezvous rdv);

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "AnnulerRDV")]
        bool AnnulerRDV(Client client, Rendezvous rdv);

        // Employé
        //=============================================================

        [OperationContract]
        [WebInvoke(Method = "GET",
           ResponseFormat = WebMessageFormat.Json,
           BodyStyle = WebMessageBodyStyle.Wrapped,
           UriTemplate = "GetRDVEmploye/{idEmploye}/{date}")]
        List<Rendezvous> GetRDVEmploye(int idEmploye, DateTime date);

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "AjouterDispo")]
        Rendezvous AjouterDispo(Employe employe, DateTime date, TimeSpan debut, TimeSpan fin, TimeSpan dureeRDV);

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "ModifierDispo")]
        Rendezvous ModifierDispo(Employe employe, Rendezvous dispo, DateTime newdate, TimeSpan newdebut, TimeSpan newfin, TimeSpan newdureeRDV, string raison);

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "AnnulerDispo")]
        bool AnnulerDispo(Employe employe, Rendezvous dispo, string raison);
    }
}
