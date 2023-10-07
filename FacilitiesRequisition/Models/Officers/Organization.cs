using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using FacilitiesRequisition.Models.Administrators;

namespace FacilitiesRequisition.Models.Officers;

public class Organization {
    public int Id { get; set; }
    
    [Display(Name = "Name of Organization")]
    public string Name { get; set; }
    
    [Display(Name = "Name of Adviser's Organization")]
    public User? Adviser { get; set; }
    
    [Display(Name = "Type of Organization")]
    public bool IsStudentCouncil { get; set; }
    
    [Display(Name = "Organization's Budget")]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal TotalBudget { get; set; }

    //public bool IsActive { get; set; } = true;
}