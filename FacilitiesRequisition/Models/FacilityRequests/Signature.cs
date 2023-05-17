using FacilitiesRequisition.Models.Officers;
using FacilitiesRequisition.Models.Faculties;
using FacilitiesRequisition.Models.Administrators;

namespace FacilitiesRequisition.Models.FacilityRequests; 

public class Signature {
    public int Id { get; set; }
    public OfficerRole Officer { get; set; }
    public bool HasSigned { get; set; }
}