namespace FacilitiesRequisition.Models.Officers;

public class OfficerRole {
    public int Id { get; set; }
    public User Officer { get; set; }
    public Organization Organization { get; set; }
    public OrganizationPosition OrganizationPosition { get; set; }
}