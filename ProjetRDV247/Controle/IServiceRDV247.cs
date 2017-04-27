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
        List<employe> GetEmployes();
                
        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "GetDispoEmploye/{idEmploye}/{date}")]
        List<rendezvous> GetDispoEmploye(int idEmploye, DateTime date);

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "GetRDVClient/{idClient}")]
        List<rendezvous> GetRDVClient(int idClient);

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "PrendreRDV")]
        rendezvous PrendreRDV(client client, rendezvous rdv);

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "AnnulerRDV")]
        bool AnnulerRDV(client client, rendezvous rdv);

        // Employé
        //=============================================================

        [OperationContract]
        [WebInvoke(Method = "GET",
           ResponseFormat = WebMessageFormat.Json,
           BodyStyle = WebMessageBodyStyle.Wrapped,
           UriTemplate = "GetRDVEmploye/{idEmploye}/{date}")]
        List<rendezvous> GetRDVEmploye(int idEmploye, DateTime date);

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "AjouterDispo")]
        rendezvous AjouterDispo(employe employe, DateTime date, TimeSpan debut, TimeSpan fin, TimeSpan dureeRDV);

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "ModifierDispo")]
        rendezvous ModifierDispo(employe employe, rendezvous dispo, DateTime newdate, TimeSpan newdebut, TimeSpan newfin, TimeSpan newdureeRDV, string raison);

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "AnnulerDispo")]
        bool AnnulerDispo(employe employe, rendezvous dispo, string raison);
    }
}
