using FacilitiesRequisition.Models.Officers;
using FacilitiesRequisition.Models.Faculties;

namespace FacilitiesRequisition.Models.FacilityRequests.HelperClass; 

public class OAS {
    public int Id { get; set; }
    public DateTime DateFiled { get; set; }
    public Organization Requester { get; set; }
    public string ActivityTitle { get; set; }
    public string? NatureActivity { get; set; }
    public string FacilityNeeded { get; set; }
    public string? EquipmentNeeded { get; set; }
    public DateTime DateRequested { get; set; }
    public Signature Signatory { get; set; }
}