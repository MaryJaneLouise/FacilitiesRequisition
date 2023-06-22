namespace FacilitiesRequisition.Models.Administrators;

public record CollegeAdministrators(
    User? AssistantDean,
    User? Dean,
    User? BuildingManager,
    User? AdminServicesDirector,
    User? StudentAffairsDirector,
    User? CampusFacilitiesDevelopmentDirector,
    User? AccountingOfficeDirector,
    User? VicePresidentAcademicAffairs,
    User? VicePresidentAdministration) {
    
    public List<User?> ToList() {
        return new List<User?> {
            AssistantDean, 
            Dean, 
            BuildingManager,
            AdminServicesDirector,
            StudentAffairsDirector,
            CampusFacilitiesDevelopmentDirector,
            AccountingOfficeDirector,
            VicePresidentAcademicAffairs,
            VicePresidentAdministration
        };
    }
}