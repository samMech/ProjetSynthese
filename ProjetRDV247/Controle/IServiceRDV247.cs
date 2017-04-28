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
        List<Rendezvous> AjouterDispo(int idEmploye, DateTime debut, DateTime fin, TimeSpan dureeDispo, int idType);

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "ModifierDispo")]
        Rendezvous ModifierDispo(int idEmploye, Rendezvous dispo, DateTime newDebut, DateTime newFin, TimeSpan newDureeRDV, int idType, string raison);

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "SupprimerDispo")]
        bool SupprimerDispo(int idEmploye, Rendezvous dispo, string raison);
    }
}
