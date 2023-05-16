namespace FacilitiesRequisition.Models.Administrators;

public class AdministratorRole {
    public int Id { get; set; }
    public User Administrator { get; set; }
    public AdministratorPosition Position { get; set; }
}