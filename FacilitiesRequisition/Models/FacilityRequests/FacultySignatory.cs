using FacilitiesRequisition.Models.Faculties;

namespace FacilitiesRequisition.Models.FacilityRequests; 

public class FacultySignatory {
    public int Id { get; set; }
    public FacilityRequest FacilityRequest { get; set; }
    public FacultyRole? Role { get; set; }
    public bool IsSigned { get; set; }
}