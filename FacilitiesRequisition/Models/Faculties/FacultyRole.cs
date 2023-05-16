namespace FacilitiesRequisition.Models.Faculties;

public class FacultyRole {
    public int Id { get; set; }
    public User Faculty { get; set; }
    public College College { get; set; }
}