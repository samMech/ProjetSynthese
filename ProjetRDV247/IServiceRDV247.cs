using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ProjetRDV247
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom d'interface "IServiceRDV247" à la fois dans le code et le fichier de configuration.
    [ServiceContract]
    public interface IServiceRDV247
    {
        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "json/{id}")]
        void DoWork();

        /*        
         
        int creerCompte(nom, prenom, telephone, email, password)
        int authentifier(email, password)

        List<Dispo> getDispo(idEmploye, date)
        Map<RDV, Client> getRDV(idEmploye, date)

        boolean prendreRDV(idDispo, idClient)
        List<RDV> getListeRDV(idClient)
        boolean annulerRDV(idRDV, idClient)
        
        ajouterDispo(date, t1, t2, dureeRDV)
        int supprimerDispo(idDispo)
        int modifierDispo(idRDV, date, t1, t2, dureeRDV, commentaire)
        modifierDispo(List<IDRDV>)

         */
    }
}
