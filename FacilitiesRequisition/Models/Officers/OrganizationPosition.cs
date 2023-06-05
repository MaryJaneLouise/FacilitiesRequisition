using System.ComponentModel.DataAnnotations;

namespace FacilitiesRequisition.Models.Officers;

public enum OrganizationPosition {
    [Display(Name = "President")]
    President,
    
    [Display(Name = "Vice President")]
    VicePresident,
    
    [Display(Name = "Secretary")]
    Secretary,
    
    [Display(Name = "Treasurer")]
    Treasurer,
    
    [Display(Name = "Auditor")]
    Auditor,
    
    [Display(Name = "Public Relations Officer")]
    PublicRelationsOfficer,
    
    [Display(Name = "Faculty")]
    Faculty
}