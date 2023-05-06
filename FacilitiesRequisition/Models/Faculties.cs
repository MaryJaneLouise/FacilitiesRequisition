namespace FacilitiesRequisition.Models;

public class College {
    public int Id { get; set; }
    public string Name { get; set; }
}
public class Faculties {
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string PasswordHash { get; set; }
}

public class Info {
    public int Id { get; set; }
    public Faculties Faculties { get; set; }
    public College College { get; set; }
}