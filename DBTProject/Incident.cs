//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DBTProject
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class Incident
    {
        public int IncidentID { get; set; }
        [Required]
        [Display(Name = "Titulo")]
        public string IncidentTitle { get; set; }
        [Required]
        [Display(Name = "Descripcion")]
        public string IncidentDescription { get; set; }
        [Required]
        [Display(Name = "Fecha de creacion")]
        public Nullable<System.DateTime> IncidentCreationDate { get; set; }
        [Required]
        [Display(Name = "Estado")]
        public Nullable<int> StatusID { get; set; }
        [Required]
        [Display(Name = "Urgencia")]
        public Nullable<int> UrgencyID { get; set; }
        [Required]
        [Display(Name = "Usuario")]
        public Nullable<int> UserID { get; set; }
        [Required]
        [Display(Name = "Tecnico")]
        public Nullable<int> TechnicianID { get; set; }
        [Required]
        [Display(Name = "Departamento")]
        public Nullable<int> DepartmentID { get; set; }
    
        public virtual Department Department { get; set; }
        public virtual Status Status { get; set; }
        public virtual Urgency Urgency { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}
