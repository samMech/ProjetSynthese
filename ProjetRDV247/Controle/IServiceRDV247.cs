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
            UriTemplate = "TestREST")]
         string TestREST();

        // GET
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
            UriTemplate = "GetDisposEmploye/{idEmploye}/{date}")]
        List<Rendezvous> GetDisposEmploye(string idEmploye, string date);

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "GetRDVClient/{idClient}")]
        List<Rendezvous> GetRDVClient(string idClient);
        
        [OperationContract]
        [WebInvoke(Method = "GET",
           ResponseFormat = WebMessageFormat.Json,
           BodyStyle = WebMessageBodyStyle.Wrapped,
           UriTemplate = "GetRDVEmploye/{idEmploye}/{date}")]
        List<Rendezvous> GetRDVEmploye(string idEmploye, string date);

        // POST
        //=============================================================

        [OperationContract]
        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "PrendreRDV")]
        Rendezvous PrendreRDV(int idClient, int idRDV);

        [OperationContract]
        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "AnnulerRDV")]
        void AnnulerRDV(int idClient, int idRDV);

        [OperationContract]
        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "AjouterDispo")]
        List<Rendezvous> AjouterDispo(int idEmploye, DateTime debut, DateTime fin, TimeSpan dureeDispo, int idType);

        [OperationContract]
        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "ModifierDispo")]
        Rendezvous ModifierDispo(int idEmploye, int idDispo, DateTime newDebut, DateTime newFin, int idType, string raison);

        [OperationContract]
        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "SupprimerDispo")]
        void SupprimerDispo(int idEmploye, int idDispo, string raison);
        
        [OperationContract]
        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "SupprimerDispos")]
        void SupprimerDispos(int idEmploye, List<int> idDispos, string raison);
    }
}
