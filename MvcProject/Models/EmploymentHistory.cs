//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MvcProject.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class EmploymentHistory
    {
        public int ID { get; set; }
        public Nullable<int> EmployeeID { get; set; }
        public Nullable<int> CertificateID { get; set; }
        public Nullable<int> InstituteID { get; set; }
        public Nullable<System.DateTime> JobStarttDate { get; set; }
        public string JobEndDate { get; set; }
    
        public virtual Certificate Certificate { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Institute Institute { get; set; }
    }
}
