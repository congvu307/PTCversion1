//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PTC_versionORANGE.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ref_Province
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ref_Province()
        {
            this.Ref_District = new HashSet<Ref_District>();
        }
    
        public string Province_Name { get; set; }
        public int Province_ID { get; set; }
        public string Province_Short_Name { get; set; }
        public string Province_Level { get; set; }
        public Nullable<int> Region_ID { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ref_District> Ref_District { get; set; }
        public virtual Ref_Region Ref_Region { get; set; }
    }
}
