using FacilitiesRequisition.Models.Administrators;
using FacilitiesRequisition.Models.Officers;
using FacilitiesRequisition.Models.Faculties;
using FacilitiesRequisition.Models;
using FacilitiesRequisition.Models.FacilityRequests;
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
    
    private DbSet<FacilityRequest> FacilityRequests { get; set; }
    private DbSet<Expense> Expenses { get; set; }
    private DbSet<AdministratorSignatory> AdministratorSignatories { get; set; }
    private DbSet<OfficerSignatory> OfficerSignatories { get; set; }
    private DbSet<FacultySignatory> FacultySignatories { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.UseSqlServer(CONNECTION_STRING);
    }

    public List<User> GetUsers() {
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

    public bool RemoveUser(User user) {
        Users.Remove(user);
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
    
    public bool RemoveAdministratorRole(AdministratorRole administratorRole) {
        AdministratorRoles.Remove(administratorRole);
        var changesSaved = SaveChanges();
        return changesSaved > 0;
    }

    public List<User> GetOfficers() {
        return Users.Where(x => x.Type == UserType.Officer).ToList();
    }

    public List<OfficerRole> GetOfficerRoles(User officer) {
        return OfficerRoles
            .Where(x => x.Officer == officer)
            .Include(officerRole => officerRole.Organization)
            .ToList();    
    }

    public OfficerRole? GetOfficerRole(int id) {
        return OfficerRoles.FirstOrDefault(x => x.Id == id);
    }

    public bool AddOfficerRole(OfficerRole officerRole) {
        OfficerRoles.Add(officerRole);
        var changesSaved = SaveChanges();
        return changesSaved > 0;
    }
    
    public bool RemoveOfficerRole(OfficerRole officerRole) {
        OfficerRoles.Remove(officerRole);
        var changesSaved = SaveChanges();
        return changesSaved > 0;
    }

    public List<Organization> GetOrganizations() {
        return Organizations.ToList();
    }

    public List<Organization> GetOrganizations(User user) {
        return user.Type == UserType.Administrator
            ? Organizations.Where(organization => organization.Adviser == user).ToList()
            : OfficerRoles.Where(officerRole => officerRole.Officer == user).Select(x => x.Organization).ToList();
            //? FacultyRoles.Where(facultyRole => facultyRole.Faculty == user).Select(x => x.Organization).ToList();
    }

    public List<Organization> GetOfficerOrganizations(User officer) {
        return OfficerRoles
            .Where(officerRole => officerRole.Officer == officer)
            .Select(officerRole => officerRole.Organization)
            .ToList();
    }

    public Organization? GetOrganization(int id) {
        return Organizations.FirstOrDefault(x => x.Id == id);
    }

    public bool AddOrganization(Organization organization) {
        Organizations.Add(organization);
        var adviserRole = new AdministratorRole { Administrator = organization.Adviser, Position = AdministratorPosition.Adviser };
        AdministratorRoles.Add(adviserRole);
        var changesSaved = SaveChanges();
        return changesSaved > 0;
    }
    
    public bool RemoveOrganization(Organization organization) {
        Organizations.Remove(organization);
        var changesSaved = SaveChanges();
        return changesSaved > 0;
    }

    public List<User> GetFaculties() {
        return Users.Where(x => x.Type == UserType.Faculty).ToList();
    }

    public List<FacultyRole> GetFacultyRoles(User faculty) {
        return FacultyRoles
            .Where(x => x.Faculty == faculty)
            .Include(facultyRole => facultyRole.Organization)
            .ToList();    
    }

    public FacultyRole? GetFacultyRole(int id) {
        return FacultyRoles.FirstOrDefault(x => x.Id == id);
    }

    public bool AddFacultyRole(FacultyRole facultyRole) {
        FacultyRoles.Add(facultyRole);
        var changesSaved = SaveChanges();
        return changesSaved > 0;
    }
    
    public bool RemoveFacultyRole(FacultyRole facultyRole) {
        FacultyRoles.Remove(facultyRole);
        var changesSaved = SaveChanges();
        return changesSaved > 0;
    }
    
    public User? GetAdmin(AdministratorPosition position)
    {
        return AdministratorRoles.FirstOrDefault(x => x.Position == position)?.Administrator;
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
    
    public bool RemoveCollege(College college) {
        Colleges.Remove(college);
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

    public FacilityRequest? GetFacilityRequest(int id) {
        return FacilityRequests.FirstOrDefault(facilityRequest => facilityRequest.Id == id);
    }
    
    public List<FacilityRequest> GetFacilityRequestsRequested(User user) {
        var organizations = GetOrganizations(user);
        return FacilityRequests.Where(facilityRequest => organizations.Contains(facilityRequest.Requester)).ToList();
    }

    public List<FacilityRequest> GetFacilityRequests() {
        return FacilityRequests
            .Include(requester => requester.Requester).ToList();
    }

    public List<FacilityRequest> GetFacilityRequests(User user) {
        if (user.Type == UserType.Officer || user.Type == UserType.Faculty || user.Type == UserType.Administrator) {
            var facilityRequests = OfficerSignatories
                .Where(signatory => signatory.Role.Officer == user)
                .Select(signatory => signatory.FacilityRequest).ToList();

            if (user.Type == UserType.Faculty) {
                var facultyRequests = FacultySignatories
                    .Where(signatory => signatory.Role.Faculty == user)
                    .Select(signatory => signatory.FacilityRequest).ToList();
                
                facilityRequests.AddRange(facultyRequests);
            } else if (user.Type == UserType.Administrator) {
                var adminRequests = AdministratorSignatories
                    .Where(signatory => signatory.Role.Administrator == user)
                    .Select(signatory => signatory.FacilityRequest).ToList();
            }

            return facilityRequests;
        }

        var showFacilityRequests = new List<FacilityRequest>();
        foreach (var facilityRequest in FacilityRequests) {
            var officerSignatories =
                OfficerSignatories.Where(signatory => signatory.FacilityRequest == facilityRequest);
            var facultySignatories =
                FacultySignatories.Where(signatory => signatory.FacilityRequest == facilityRequest);
            var adminSignatories =
                AdministratorSignatories.Where(signatory => signatory.FacilityRequest == facilityRequest);
            
            var assistantDeanSignatory = adminSignatories
                .FirstOrDefault(signatory => signatory.Role.Position == AdministratorPosition.AssistantDean);
            var deanSignatory = adminSignatories
                .FirstOrDefault(signatory => signatory.Role.Position == AdministratorPosition.Dean);
            
            var buildingManagerDirectorSignatory = adminSignatories
                .FirstOrDefault(signatory => signatory.Role.Position == AdministratorPosition.BuildingManager);
            var adminServicesDirectorSignatory = adminSignatories
                .FirstOrDefault(signatory => signatory.Role.Position == AdministratorPosition.AdminServicesDirector);
            var studentAffairsDirectorSignatory = adminSignatories
                .FirstOrDefault(signatory => signatory.Role.Position == AdministratorPosition.StudentAffairsDirector);
            var campusFacilitiesDevelopmentDirectorSignatory = adminSignatories
                .FirstOrDefault(signatory => signatory.Role.Position == AdministratorPosition.CampusFacilitiesDevelopmentDirector);
            var accountingOfficeDirectorSignatory = adminSignatories
                .FirstOrDefault(signatory => signatory.Role.Position == AdministratorPosition.AccountingOfficeDirector);
            
            var vicePresidentAcademicAffairs = adminSignatories
                .FirstOrDefault(signatory => signatory.Role.Position == AdministratorPosition.VicePresidentAcademicAffairs);
            var vicePresidentAdministration = adminSignatories
                .FirstOrDefault(signatory => signatory.Role.Position == AdministratorPosition.VicePresidentAdministration);
            
            
            var allOfficersHaveSigned = officerSignatories.All(x => x.IsSigned);
            var allFacultiesHaveSigned = facultySignatories.All(x => x.IsSigned);
            
            if ((user == assistantDeanSignatory?.Role.Administrator && allOfficersHaveSigned) ||
                (user == assistantDeanSignatory?.Role.Administrator && allFacultiesHaveSigned) ||
                (user == deanSignatory?.Role.Administrator && assistantDeanSignatory?.IsSigned == true) ||
                (user == buildingManagerDirectorSignatory?.Role.Administrator && deanSignatory?.IsSigned == true) ||
                (user == adminServicesDirectorSignatory?.Role.Administrator && buildingManagerDirectorSignatory?.IsSigned == true) ||
                (user == studentAffairsDirectorSignatory?.Role.Administrator && adminServicesDirectorSignatory?.IsSigned == true) ||
                (user == campusFacilitiesDevelopmentDirectorSignatory?.Role.Administrator && studentAffairsDirectorSignatory?.IsSigned == true) ||
                (user == accountingOfficeDirectorSignatory?.Role.Administrator && campusFacilitiesDevelopmentDirectorSignatory?.IsSigned == true ) ||
                (user == vicePresidentAcademicAffairs?.Role.Administrator && adminServicesDirectorSignatory?.IsSigned == true) || 
                (user == vicePresidentAdministration?.Role.Administrator && vicePresidentAcademicAffairs?.IsSigned == true)) {
                
                showFacilityRequests.Add(facilityRequest);
            }
        }

        return showFacilityRequests;
    }

    private Signatures GetDefaultSignatures(FacilityRequest facilityRequest) {
        var officerRoles = OfficerRoles.Where(officerRole => officerRole.Organization == facilityRequest.Requester);

        return new Signatures(
            President: new OfficerSignatory {
                FacilityRequest = facilityRequest,
                IsSigned = false,
                Role = officerRoles.FirstOrDefault(officerRole => officerRole.Position == OrganizationPosition.President)
            },
            Adviser: new AdministratorSignatory {
                FacilityRequest = facilityRequest,
                IsSigned = false,
                Role = AdministratorRoles.FirstOrDefault(officerRole => officerRole.Position == AdministratorPosition.Adviser && officerRole.Administrator == facilityRequest.Requester.Adviser),
            },
            AssistantDean: new AdministratorSignatory {
                FacilityRequest = facilityRequest,
                IsSigned = false,
                Role = AdministratorRoles.FirstOrDefault(officerRole => officerRole.Position == AdministratorPosition.AssistantDean)
            },
            Dean: new AdministratorSignatory {
                FacilityRequest = facilityRequest,
                IsSigned = false,
                Role = AdministratorRoles.FirstOrDefault(officerRole => officerRole.Position == AdministratorPosition.Dean)
            },
            BuildingManager: new AdministratorSignatory {
                FacilityRequest = facilityRequest,
                IsSigned = false,
                Role = AdministratorRoles.FirstOrDefault(officerRole => officerRole.Position == AdministratorPosition.BuildingManager)
            },
            AdminServicesDirector: new AdministratorSignatory {
                FacilityRequest = facilityRequest,
                IsSigned = false,
                Role = AdministratorRoles.FirstOrDefault(officerRole => officerRole.Position == AdministratorPosition.AdminServicesDirector)
            },
            StudentAffairsDirector: new AdministratorSignatory {
                FacilityRequest = facilityRequest,
                IsSigned = false,
                Role = AdministratorRoles.FirstOrDefault(officerRole => officerRole.Position == AdministratorPosition.StudentAffairsDirector)
            },
            CampusFacilitiesDevelopmentDirector: new AdministratorSignatory {
                FacilityRequest = facilityRequest,
                IsSigned = false,
                Role = AdministratorRoles.FirstOrDefault(officerRole => officerRole.Position == AdministratorPosition.CampusFacilitiesDevelopmentDirector)
            },
            AccountingOfficeDirector: new AdministratorSignatory {
                FacilityRequest = facilityRequest,
                IsSigned = false,
                Role = AdministratorRoles.FirstOrDefault(officerRole => officerRole.Position == AdministratorPosition.AccountingOfficeDirector)
            },
            VicePresidentAcademicAffairs: new AdministratorSignatory {
                FacilityRequest = facilityRequest,
                IsSigned = false,
                Role = AdministratorRoles.FirstOrDefault(officerRole => officerRole.Position == AdministratorPosition.VicePresidentAcademicAffairs)
            },
            VicePresidentAdministration: new AdministratorSignatory {
                FacilityRequest = facilityRequest,
                IsSigned = false,
                Role = AdministratorRoles.FirstOrDefault(officerRole => officerRole.Position == AdministratorPosition.VicePresidentAdministration)
            });
    }

    private bool AddSignatures(Signatures signatures) {
        OfficerSignatories.Add(signatures.President);
        AdministratorSignatories.Add(signatures.Adviser);
        AdministratorSignatories.Add(signatures.AssistantDean);
        AdministratorSignatories.Add(signatures.Dean);
        AdministratorSignatories.Add(signatures.BuildingManager);
        AdministratorSignatories.Add(signatures.AdminServicesDirector);
        AdministratorSignatories.Add(signatures.StudentAffairsDirector);
        AdministratorSignatories.Add(signatures.CampusFacilitiesDevelopmentDirector);
        AdministratorSignatories.Add(signatures.AccountingOfficeDirector);
        AdministratorSignatories.Add(signatures.VicePresidentAcademicAffairs);
        AdministratorSignatories.Add(signatures.VicePresidentAdministration);

        var changesSaved = SaveChanges();
        return changesSaved > 0;
    }

    public bool AddFacilityRequest(FacilityRequest facilityRequest, List<Expense> expenses) {
        FacilityRequests.Add(facilityRequest);
        Expenses.AddRange(expenses);
        AddSignatures(GetDefaultSignatures(facilityRequest));

        var changesSaved = SaveChanges();
        return changesSaved > 0;
    }

    public bool RemoveFacilityRequest(FacilityRequest facilityRequest) {
        FacilityRequests.Remove(facilityRequest);
        //TODO: Deletion of expenses and signatures

        var changesSaved = SaveChanges();
        return changesSaved > 0;
    }

    public List<AdministratorSignatory> GetAdministratorSignatories(FacilityRequest facilityRequest) {
        return AdministratorSignatories
            .Where(adminSignatory => adminSignatory.FacilityRequest == facilityRequest)
            .ToList();
    }

    public List<OfficerSignatory> GetOfficerSignatories(FacilityRequest facilityRequest) {
        return OfficerSignatories
            .Where(officerSignatory => officerSignatory.FacilityRequest == facilityRequest)
            .ToList();
    }

    public Signatures GetSignatures(FacilityRequest facilityRequest) {
        var officerSignatories = GetOfficerSignatories(facilityRequest);
        var adminSignatories = GetAdministratorSignatories(facilityRequest);

        return new Signatures(
            President: officerSignatories.First(signatory => signatory.Role!.Position == OrganizationPosition.President),
            Adviser: adminSignatories.First(signatory => signatory.Role!.Administrator == facilityRequest.Requester.Adviser),
            
            AssistantDean: adminSignatories.First(signatory => signatory.Role!.Position == AdministratorPosition.AssistantDean),
            Dean: adminSignatories.First(signatory => signatory.Role!.Position == AdministratorPosition.Dean),
            
            BuildingManager: adminSignatories.First(signatory => signatory.Role!.Position == AdministratorPosition.BuildingManager),
            
            AdminServicesDirector: adminSignatories.First(signatory => signatory.Role!.Position == AdministratorPosition.AdminServicesDirector),
            StudentAffairsDirector: adminSignatories.First(signatory => signatory.Role!.Position == AdministratorPosition.StudentAffairsDirector),
            CampusFacilitiesDevelopmentDirector: adminSignatories.First(signatory => signatory.Role!.Position == AdministratorPosition.CampusFacilitiesDevelopmentDirector),
            AccountingOfficeDirector: adminSignatories.First(signatory => signatory.Role!.Position == AdministratorPosition.AccountingOfficeDirector),
            
            VicePresidentAcademicAffairs: adminSignatories.First(signatory => signatory.Role!.Position == AdministratorPosition.VicePresidentAcademicAffairs),
            VicePresidentAdministration: adminSignatories.First(signatory => signatory.Role!.Position == AdministratorPosition.VicePresidentAdministration));
    }   
}