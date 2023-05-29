using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using FacilitiesRequisition.Models.Administrators;

namespace FacilitiesRequisition.Models.Officers;

public class Organization {
    public int Id { get; set; }
    public string Name { get; set; }
    public User? OrganizationAdviser { get; set; }
    public bool IsStudentCouncil { get; set; }
    
    [Column(TypeName = "decimal(18, 2)")]
    public decimal OrganizationBudget { get; set; }
}