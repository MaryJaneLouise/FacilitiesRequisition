using FacilitiesRequisition.Models.Officers;
using FacilitiesRequisition.Models.Faculties;
using FacilitiesRequisition.Models.Administrators;

namespace FacilitiesRequisition.Models.FacilityRequests;

public record Signatures(
    OfficerSignatory President,
    AdministratorSignatory Adviser,
    AdministratorSignatory AssistantDean,
    AdministratorSignatory Dean,
    AdministratorSignatory BuildingManager,
    AdministratorSignatory AdminServicesDirector,
    AdministratorSignatory StudentAffairsDirector,
    AdministratorSignatory CampusFacilitiesDevelopmentDirector,
    AdministratorSignatory AccountingOfficeDirector,
    AdministratorSignatory VicePresidentAcademicAffairs,
    AdministratorSignatory VicePresidentAdministration) {

    public SignatureStage GetSignatureStage() {
        if (VicePresidentAcademicAffairs.IsSigned && VicePresidentAdministration.IsSigned)
            return SignatureStage.Approved;
        if (AdminServicesDirector.IsSigned && StudentAffairsDirector.IsSigned && CampusFacilitiesDevelopmentDirector.IsSigned && AccountingOfficeDirector.IsSigned)
            return SignatureStage.VicePresidents;
        if (BuildingManager.IsSigned)
            return SignatureStage.Directors;
        if (Dean.IsSigned && AssistantDean.IsSigned)
            return SignatureStage.BuildingManager;
        if (Adviser.IsSigned && President.IsSigned)
            return SignatureStage.Deans;

        return SignatureStage.Organization;
    }

    public List<Signature> ToList() {
        return new() {
            President,
            Adviser, 
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