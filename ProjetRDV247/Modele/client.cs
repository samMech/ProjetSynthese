//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProjetRDV247.Modele
{
    using System;
    using System.Collections.Generic;
    
    public partial class client
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public client()
        {
            this.rendezvous = new HashSet<rendezvous>();
        }
    
        public int id_client { get; set; }
        public string nom_client { get; set; }
        public string prenom_client { get; set; }
        public string telephone_client { get; set; }
        public string courriel_client { get; set; }
        public string password_client { get; set; }
        public System.Guid salt { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rendezvous> rendezvous { get; set; }
    }
}