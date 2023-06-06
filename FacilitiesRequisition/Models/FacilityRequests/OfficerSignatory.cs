using FacilitiesRequisition.Models.Officers;

namespace FacilitiesRequisition.Models.FacilityRequests; 

public class OfficerSignatory : Signature {
    public override int Id { get; set; }
    public FacilityRequest FacilityRequest { get; set; }
    public OfficerRole? Role { get; set; }
    public override bool IsSigned { get; set; }

    public override User User => Role.Officer;
}