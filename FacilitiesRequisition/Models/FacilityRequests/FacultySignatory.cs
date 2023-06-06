using FacilitiesRequisition.Models.Faculties;

namespace FacilitiesRequisition.Models.FacilityRequests; 

public class FacultySignatory : Signature {
    public override int Id { get; set; }
    public FacilityRequest FacilityRequest { get; set; }
    public FacultyRole? Role { get; set; }
    public override bool IsSigned { get; set; }

    public override User User => Role.Faculty;
}