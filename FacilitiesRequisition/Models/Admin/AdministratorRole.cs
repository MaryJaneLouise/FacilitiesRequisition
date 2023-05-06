namespace FacilitiesRequisition.Models.Admin; 

public class AdministratorRole {
    public int Id { get; set; }
    public Administrator Administrator { get; set; }
    public AdministratorPosition Position { get; set; }
}