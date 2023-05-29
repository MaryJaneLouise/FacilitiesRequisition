using FacilitiesRequisition.Models.Administrators;

namespace FacilitiesRequisition.Models.FacilityRequests; 

public class AdministratorSignatory {
    public int Id { get; set; }
    public FacilityRequest FacilityRequest { get; set; }
    public AdministratorRole? Role { get; set; }
    public bool IsSigned { get; set; }
}