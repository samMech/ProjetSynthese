//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
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
