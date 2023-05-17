using FacilitiesRequisition.Models.Officers;
using FacilitiesRequisition.Models.Faculties;

namespace FacilitiesRequisition.Models.FacilityRequests; 

public class FacilityRequest {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public Organization Requester { get; set; }
}