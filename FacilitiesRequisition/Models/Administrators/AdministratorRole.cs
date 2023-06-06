using FacilitiesRequisition.Models.Users;

namespace FacilitiesRequisition.Models.Administrators;

public class AdministratorRole  : UserRole {
    public int Id { get; set; }
    public User Administrator { get; set; }
    public AdministratorPosition Position { get; set; }
}