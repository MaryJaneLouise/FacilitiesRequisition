namespace FacilitiesRequisition.Models.Administrators; 

public class Administrator: User{
    public override int Id { get; set; }
    public override string FirstName { get; set; }
    public override string? MiddleName { get; set; }
    public override string LastName { get; set; }
    public override string Username { get; set; }
    public override string PasswordHash { get; set; }
    public override string PasswordSalt { get; set; }
}