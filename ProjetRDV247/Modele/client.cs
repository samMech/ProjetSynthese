//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Runtime.Serialization;

namespace ProjetRDV247.Modele
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Client
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Client()
        {
            this.Rendezvous = new HashSet<Rendezvous>();
        }
    
        public int id_client { get; set; }
        public string nom_client { get; set; }
        public string prenom_client { get; set; }
        public string telephone_client { get; set; }
        public string courriel_client { get; set; }
        [NotMapped]
        public string password_client { get; set; }
        [NotMapped]
        public System.Guid salt { get; set; }
    
        [IgnoreDataMember]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Rendezvous> Rendezvous { get; set; }
    }
}
