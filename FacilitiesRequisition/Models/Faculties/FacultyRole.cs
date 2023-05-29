using FacilitiesRequisition.Models.Officers;

namespace FacilitiesRequisition.Models.Faculties;

public class FacultyRole {
    public int Id { get; set; }
    public User Faculty { get; set; }
    public Organization Organization { get; set; }
    public OrganizationPosition Position { get; set; }
}