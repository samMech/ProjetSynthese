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
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;

    public partial class Typerdv
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Typerdv()
        {
            this.Rendezvous = new HashSet<Rendezvous>();
        }
    
        public int id_typerdv { get; set; }
        public string nom_typerdv { get; set; }
        public int id_employe_typerdv { get; set; }

        [IgnoreDataMember]
        public virtual Employe Employe { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [NotMapped]
        [IgnoreDataMember]        
        public virtual ICollection<Rendezvous> Rendezvous { get; set; }
    }
}
