using FacilitiesRequisition.Models.Administrators;
using FacilitiesRequisition.Models.Officers;
using FacilitiesRequisition.Models.Faculties;
using FacilitiesRequisition.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;

namespace FacilitiesRequisition.Data; 

public class DatabaseContext : DbContext {
    public const string CONNECTION_STRING =
        @"Server=LAPTOP-IERTITV7;Database=FacilitiesRequisitions;Trusted_Connection=True;TrustServerCertificate=True";

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) {
        
    }
    
    private DbSet<User> Users { get; set; }
    private DbSet<AdministratorRole> AdministratorRoles { get; set; }
    
    private DbSet<OfficerRole> OfficerRoles { get; set; }
    private DbSet<Organization> Organizations { get; set; }
    
    private DbSet<FacultyRole> FacultyRoles { get; set; }
    private DbSet<College> Colleges { get; set; }
    
    private DbSet<SchoolTag> SchoolTags { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.UseSqlServer(CONNECTION_STRING);
    }

    public IEnumerable<User> GetUsers() {
        var users = Users.ToList();
        return users;
    }

    public User? GetUser(int id) {
        return GetUsers().FirstOrDefault(x => x.Id == id);
    }

    public bool AddUser(User user) {
        Users.Add(user);
        var changesSaved = SaveChanges();
        return changesSaved > 0;
    }

    public List<User> GetAdministrators() {
        return Users.Where(x => x.Type == UserType.Administrator).ToList();
    }

    public List<AdministratorRole> GetAdministratorRoles(User administrator) {
        return AdministratorRoles.Where(x => x.Administrator == administrator).ToList();
    }

    public bool HasSuperAdministrator() {
        return AdministratorRoles.Any(x => x.Position == AdministratorPosition.SuperAdmin);
    }

    public AdministratorRole? GetAdministratorRole(int id) {
        return AdministratorRoles.FirstOrDefault(x => x.Id == id);
    }

    public bool AddAdministratorRole(AdministratorRole administratorRole) {
        AdministratorRoles.Add(administratorRole);
        var changesSaved = SaveChanges();
        return changesSaved > 0;
    }

    public List<User> GetOfficers() {
        return Users.Where(x => x.Type == UserType.Officer).ToList();
    }

    public List<OfficerRole> GetOfficerRoles(User officer) {
        return OfficerRoles.Where(x => x.Officer == officer).ToList();
    }

    public OfficerRole? GetOfficerRole(int id) {
        return OfficerRoles.FirstOrDefault(x => x.Id == id);
    }

    public bool AddOfficerRole(OfficerRole officerRole) {
        OfficerRoles.Add(officerRole);
        var changesSaved = SaveChanges();
        return changesSaved > 0;
    }

    public List<Organization> GetOrganizations() {
        return Organizations.ToList();
    }

    public Organization? GetOrganization(int id) {
        return Organizations.FirstOrDefault(x => x.Id == id);
    }

    public bool AddOrganization(Organization organization) {
        Organizations.Add(organization);
        var changesSaved = SaveChanges();
        return changesSaved > 0;
    }

    public List<User> GetFaculties() {
        return Users.Where(x => x.Type == UserType.Faculty).ToList();
    }

    public List<FacultyRole> GetFacultyRoles(User faculty) {
        return FacultyRoles.Where(x => x.Faculty == faculty).ToList();
    }

    public FacultyRole? GetFacultyRole(int id) {
        return FacultyRoles.FirstOrDefault(x => x.Id == id);
    }

    public bool AddFacultyRole(FacultyRole facultyRole) {
        FacultyRoles.Add(facultyRole);
        var changesSaved = SaveChanges();
        return changesSaved > 0;
    }

    public List<College> GetColleges() {
        return Colleges.ToList();
    }

    public College? GetCollege(int id) {
        return Colleges.FirstOrDefault(x => x.Id == id);
    }

    public bool AddCollege(College college) {
        Colleges.Add(college);
        var changesSaved = SaveChanges();
        return changesSaved > 0;
    }

    public SchoolTag? GetSchoolTag() {
        return SchoolTags.LastOrDefault();
    }

    public bool SetSchoolTag(SchoolTag schoolTag) {
        SchoolTags.Add(schoolTag);
        var changesSaved = SaveChanges();
        return changesSaved > 0;
    }
}