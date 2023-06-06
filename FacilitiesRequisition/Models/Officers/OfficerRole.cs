using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using FacilitiesRequisition.Models.Users;

namespace FacilitiesRequisition.Models.Officers;

public class OfficerRole : UserRole {
    public int Id { get; set; }
    public User Officer { get; set; }
    public Organization Organization { get; set; }
    public OrganizationPosition Position { get; set; }
}