using FacilitiesRequisition.Models.Administrators;

namespace FacilitiesRequisition.Models.Officers;

public class Organization {
    public int Id { get; set; }
    public string Name { get; set; }
    public Administrator Adviser { get; set; }
    public bool IsStudentCouncil { get; set; }
}