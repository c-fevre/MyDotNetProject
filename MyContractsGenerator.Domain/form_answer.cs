//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyContractsGenerator.Domain
{
    using System;
    using System.Collections.Generic;
    
    public partial class form_answer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public form_answer()
        {
            this.answers = new HashSet<answer>();
        }
    
        public int form_id { get; set; }
        public int collaborator_id { get; set; }
        public bool replied { get; set; }
        public System.DateTime last_update { get; set; }
        public int id { get; set; }
    
        public virtual collaborator collaborator { get; set; }
        public virtual form form { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<answer> answers { get; set; }
    }
}