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
    
    public partial class rendezvous
    {
        public int id_rdv { get; set; }
        public System.DateTime debut_rdv { get; set; }
        public System.DateTime fin_rdv { get; set; }
        public string statut_rdv { get; set; }
        public Nullable<int> id_client_rdv { get; set; }
        public int id_employe_rdv { get; set; }
        public int id_typerdv_rdv { get; set; }
    
        public virtual client client { get; set; }
        public virtual employe employe { get; set; }
        public virtual typerdv typerdv { get; set; }
    }
}
