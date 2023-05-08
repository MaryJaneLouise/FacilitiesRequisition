using FacilitiesRequisition.Models.Administrators;
using FacilitiesRequisition.Models.Officers;
using FacilitiesRequisition.Models.Faculties;
using FacilitiesRequisition.Models;
using Microsoft.EntityFrameworkCore;

namespace FacilitiesRequisition.Data; 

public class DatabaseContext : DbContext {
    public const string CONNECTION_STRING =
        @"Server=(localdb)\mssqllocaldb;Database=FacilitiesRequisitions;Trusted_Connection=True";

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) {
        
    }
    private DbSet<Administrator> Administrators { get; set; }
    private DbSet<AdministratorRole> AdministratorRoles { get; set; }
    
    private DbSet<Officer> Officers { get; set; }
    private DbSet<OfficerRole> OfficerRoles { get; set; }
    private DbSet<Organization> Organizations { get; set; }
    
    private DbSet<Faculty> Faculties { get; set; }
    private DbSet<FacultyRole> FacultyRoles { get; set; }
    private DbSet<College> Colleges { get; set; }
    
    private DbSet<SchoolTag> SchoolTags { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.UseSqlServer(CONNECTION_STRING);
    }

    public List<User> GetUsers() {
        var users = Administrators.Select(x => x as User).ToList();
        var officers = Officers.Select(x => x as User).ToList();
        var faculties = Faculties.Select(x => x as User).ToList();
        
        users.AddRange(officers);
        return users;
    }
    public List<Administrator> GetAdministrators() {
        return Administrators.ToList();
    }

    public Administrator? GetAdministrator(int Id) {
        return Administrators.FirstOrDefault(x => x.Id == Id);
    }

    public bool AddAdministrator(Administrator administrator) {
        Administrators.Add(administrator);
        var changesSaved = SaveChanges();
        return changesSaved > 0;
    }

    public List<AdministratorRole> GetAdministratorRoles(Administrator administrator) {
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

    public List<OfficerRole> GetOfficerRoles(Officer officer) {
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

    public bool AddOrganization(Organization organization) {
        Organizations.Add(organization);
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