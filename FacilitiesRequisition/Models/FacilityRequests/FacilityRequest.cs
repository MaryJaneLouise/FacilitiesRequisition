using System.ComponentModel.DataAnnotations;
using FacilitiesRequisition.Models.Officers;
using FacilitiesRequisition.Models.Faculties;

namespace FacilitiesRequisition.Models.FacilityRequests; 

public class FacilityRequest {
    public int Id { get; set; }
    
    public string DateFiled { get; set; }
    public Organization Requester { get; set; }
    
    public string NameActivity { get; set; }
    
    public string ContactNumber { get; set; }
    
    [DataType(DataType.Date)]
    public DateTime? StartDateRequested { get; set; }
    
    // [DataType(DataType.Date)]
    // public DateTime? RequestedDayOne { get; set; }
    //
    // [DataType(DataType.Time)]
    // public DateTime? RequestedDayOneStartTime { get; set; }
    //
    // [DataType(DataType.Time)]
    // public DateTime? RequestedDayOneEndTime { get; set; }
    //
    // [DataType(DataType.Date)]
    // public DateTime? RequestedDayTwo { get; set; }
    //
    // [DataType(DataType.Time)]
    // public DateTime? RequestedDayTwoStartTime { get; set; }
    //
    // [DataType(DataType.Time)]
    // public DateTime? RequestedDayTwoEndTime { get; set; }
    //
    // [DataType(DataType.Date)]
    // public DateTime? RequestedDayThree { get; set; }
    //
    // [DataType(DataType.Time)]
    // public DateTime? RequestedDayThreeStartTime { get; set; }
    //
    // [DataType(DataType.Time)]
    // public DateTime? RequestedDayThreeEndTime { get; set; }
    
    [DataType(DataType.Date)]
    public DateTime? EndDateRequested { get; set; }
    
    [EnumDataType(typeof(Venues))]
    public Venues VenueRequested { get; set; }
    
    public string? AdditionalComments { get; set; }
    
}