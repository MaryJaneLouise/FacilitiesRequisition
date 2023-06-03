using FacilitiesRequisition.Models.Officers;
using FacilitiesRequisition.Models.Faculties;

namespace FacilitiesRequisition.Models.FacilityRequests.HelperClass; 

public class SCC {
    public int Id { get; set; }
    public DateTime DateFiled { get; set; }
    public Organization Requester { get; set; }
    public User Position { get; set; }
    public string ActivityTitle { get; set; }
    public DateTime DateRequested { get; set; }
    public string NumHours { get; set; }
    public string VenueRequested { get; set; }
    public Signatures Signatory { get; set; }
}