using FacilitiesRequisition.Models.Administrators;

namespace FacilitiesRequisition.Models.FacilityRequests; 

public class AdministratorSignatory : Signature {
    public override int Id { get; set; }
    public FacilityRequest FacilityRequest { get; set; }
    public AdministratorRole? Role { get; set; }
    public override bool IsSigned { get; set; }

    public override User User => Role.Administrator;
}