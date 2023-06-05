using System.ComponentModel.DataAnnotations;

namespace FacilitiesRequisition.Models.Administrators;

public enum AdministratorPosition {
    [Display(Name = "Adviser")]
    Adviser,
    
    [Display(Name = "Assistant Dean")]
    AssistantDean,
    
    [Display(Name = "Dean")]
    Dean,
    
    [Display(Name = "Building Manager")]
    BuildingManager,
    
    [Display(Name = "Admin Services Director")]
    AdminServicesDirector,
    
    [Display(Name = "Student Affairs Director")]
    StudentAffairsDirector,
    
    [Display(Name = "Campus Facilities Development Director")]
    CampusFacilitiesDevelopmentDirector,
    
    [Display(Name = "Accounting Office Director")]
    AccountingOfficeDirector,
    
    [Display(Name = "Vice President Academic Affairs")]
    VicePresidentAcademicAffairs,
    
    [Display(Name = "Vice President Administration")]
    VicePresidentAdministration,
    
    [Display(Name = "Super Admin")]
    SuperAdmin
}