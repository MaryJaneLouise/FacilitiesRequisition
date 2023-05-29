using FacilitiesRequisition.Models.Officers;

namespace FacilitiesRequisition.Models.FacilityRequests; 

public class OfficerSignatory {
    public int Id { get; set; }
    public FacilityRequest FacilityRequest { get; set; }
    public OfficerRole? Role { get; set; }
    public bool IsSigned { get; set; }
}