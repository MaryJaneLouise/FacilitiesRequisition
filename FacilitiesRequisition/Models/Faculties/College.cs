using FacilitiesRequisition.Models.Administrators;

namespace FacilitiesRequisition.Models.Faculties; 

public class College {
    public int Id { get; set; }
    public string Name { get; set; }
    public Administrator DeparmentChair { get; set; }
}