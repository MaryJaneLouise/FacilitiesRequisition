namespace FacilitiesRequisition.Models; 

public class User {
    public int Id { get; set; }
    public UserType Type { get; set; }
    public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
    public string? SignatureFilename { get; set; }
    // public bool IsActive { get; set; } = true;

    public string FullName => $"{FirstName} {LastName}";
}