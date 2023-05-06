namespace FacilitiesRequisition.Models;

public class Adviser {
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string PasswordHash { get; set; }
} 
public class Organization {
    public int Id { get; set; }
    public string Name { get; set; }
    public Adviser Adviser { get; set; }
    public bool isStudentCouncil { get; set; }
}

public enum Position {
    President,
    VicePresident,
    Secretary,
    Treasurer,
    Auditor,
    PublicRelationsOfficer
}

public class Officer {
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string PasswordHash { get; set; }
}

public class Role {
    public int Id { get; set; }
    public Officer Officer { get; set; }
    public Organization Organization { get; set; }
    public Position Position { get; set; }
}