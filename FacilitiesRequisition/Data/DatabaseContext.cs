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
        @"Server=(localdb)\mssqllocaldb;Database=FacilitiesRequisitions;Trusted_Connection=True";

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
    
    public bool IsSuperAdmin(User user) {
        return AdministratorRoles.Any(adminRole => adminRole.Administrator == user);
    }

    public bool CanCreateRequests(User user) {
        var organizations = GetOfficerOrganizations(user);
        return user.Type == UserType.Officer && organizations.Count > 0;
    }

    public bool AreCollegeAdminsSet() {
        var collegeAdmins = GetCollegeAdmins();
        return collegeAdmins.ToList().All(admin => admin != null);
    }

    public User? GetUser(string username) {
        return Users.FirstOrDefault(user => user.Username == username);
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
    
    public CollegeAdministrators GetCollegeAdmins() {
        var assistantDean = AdministratorRoles
    .Where(adminRole => adminRole.Position == AdministratorPosition.AssistantDean)
    .Include(adminRole => adminRole.Administrator)
    .OrderBy(adminRole => adminRole.Id)
    .LastOrDefault()
    ?.Administrator;

    var dean = AdministratorRoles
        .Where(adminRole => adminRole.Position == AdministratorPosition.Dean)
        .Include(adminRole => adminRole.Administrator)
        .OrderBy(adminRole => adminRole.Id)
        .LastOrDefault()
        ?.Administrator;

    var buildingManager = AdministratorRoles
        .Where(adminRole => adminRole.Position == AdministratorPosition.BuildingManager)
        .Include(adminRole => adminRole.Administrator)
        .OrderBy(adminRole => adminRole.Id)
        .LastOrDefault()
        ?.Administrator;

    var adminServicesDirector = AdministratorRoles
        .Where(adminRole => adminRole.Position == AdministratorPosition.AdminServicesDirector)
        .Include(adminRole => adminRole.Administrator)
        .OrderBy(adminRole => adminRole.Id)
        .LastOrDefault()
        ?.Administrator;

    var studentAffairsDirector = AdministratorRoles
        .Where(adminRole => adminRole.Position == AdministratorPosition.StudentAffairsDirector)
        .Include(adminRole => adminRole.Administrator)
        .OrderBy(adminRole => adminRole.Id)
        .LastOrDefault()
        ?.Administrator;

    var campusFacilitiesDevelopmentDirector = AdministratorRoles
        .Where(adminRole => adminRole.Position == AdministratorPosition.CampusFacilitiesDevelopmentDirector)
        .Include(adminRole => adminRole.Administrator)
        .OrderBy(adminRole => adminRole.Id)
        .LastOrDefault()
        ?.Administrator;

    var accountingOfficeDirector = AdministratorRoles
        .Where(adminRole => adminRole.Position == AdministratorPosition.AccountingOfficeDirector)
        .Include(adminRole => adminRole.Administrator)
        .OrderBy(adminRole => adminRole.Id)
        .LastOrDefault()
        ?.Administrator;

    var vicePresidentAcademicAffairs = AdministratorRoles
        .Where(adminRole => adminRole.Position == AdministratorPosition.VicePresidentAcademicAffairs)
        .Include(adminRole => adminRole.Administrator)
        .OrderBy(adminRole => adminRole.Id)
        .LastOrDefault()
        ?.Administrator;

    var vicePresidentAdministration = AdministratorRoles
        .Where(adminRole => adminRole.Position == AdministratorPosition.VicePresidentAdministration)
        .Include(adminRole => adminRole.Administrator)
        .OrderBy(adminRole => adminRole.Id)
        .LastOrDefault()
        ?.Administrator;

        return new CollegeAdministrators(
            assistantDean,
            dean,
            buildingManager,
            adminServicesDirector,
            studentAffairsDirector,
            campusFacilitiesDevelopmentDirector,
            accountingOfficeDirector,
            vicePresidentAcademicAffairs,
            vicePresidentAdministration);
    }

    public void SetCollegeAdmins(CollegeAdministrators collegeAdmins) {
        var oldCollegeAdmins = GetCollegeAdmins();

        if (collegeAdmins.AssistantDean != null && collegeAdmins.AssistantDean != oldCollegeAdmins.AssistantDean) {
            AdministratorRoles.Add(new AdministratorRole {
                Administrator = collegeAdmins.AssistantDean,
                Position = AdministratorPosition.AssistantDean,
            });
        }

    if (collegeAdmins.Dean != null && collegeAdmins.Dean != oldCollegeAdmins.Dean) {
        AdministratorRoles.Add(new AdministratorRole {
            Administrator = collegeAdmins.Dean,
            Position = AdministratorPosition.Dean,
        });
    }

    if (collegeAdmins.BuildingManager != null && collegeAdmins.BuildingManager != oldCollegeAdmins.BuildingManager) {
        AdministratorRoles.Add(new AdministratorRole {
            Administrator = collegeAdmins.BuildingManager,
            Position = AdministratorPosition.BuildingManager,
        });
    }

    if (collegeAdmins.AdminServicesDirector != null && collegeAdmins.AdminServicesDirector != oldCollegeAdmins.AdminServicesDirector) {
        AdministratorRoles.Add(new AdministratorRole {
            Administrator = collegeAdmins.AdminServicesDirector,
            Position = AdministratorPosition.AdminServicesDirector,
        });
    }

    if (collegeAdmins.StudentAffairsDirector != null && collegeAdmins.StudentAffairsDirector != oldCollegeAdmins.StudentAffairsDirector) {
        AdministratorRoles.Add(new AdministratorRole {
            Administrator = collegeAdmins.StudentAffairsDirector,
            Position = AdministratorPosition.StudentAffairsDirector,
        });
    }

    if (collegeAdmins.CampusFacilitiesDevelopmentDirector != null && collegeAdmins.CampusFacilitiesDevelopmentDirector != oldCollegeAdmins.CampusFacilitiesDevelopmentDirector) {
        AdministratorRoles.Add(new AdministratorRole {
            Administrator = collegeAdmins.CampusFacilitiesDevelopmentDirector,
            Position = AdministratorPosition.CampusFacilitiesDevelopmentDirector,
        });
    }

    if (collegeAdmins.AccountingOfficeDirector != null && collegeAdmins.AccountingOfficeDirector != oldCollegeAdmins.AccountingOfficeDirector) {
        AdministratorRoles.Add(new AdministratorRole {
            Administrator = collegeAdmins.AccountingOfficeDirector,
            Position = AdministratorPosition.AccountingOfficeDirector,
        });
    }

    if (collegeAdmins.VicePresidentAcademicAffairs != null && collegeAdmins.VicePresidentAcademicAffairs != oldCollegeAdmins.VicePresidentAcademicAffairs) {
        AdministratorRoles.Add(new AdministratorRole {
            Administrator = collegeAdmins.VicePresidentAcademicAffairs,
            Position = AdministratorPosition.VicePresidentAcademicAffairs,
        });
    }

    if (collegeAdmins.VicePresidentAdministration != null && collegeAdmins.VicePresidentAdministration != oldCollegeAdmins.VicePresidentAdministration) {
        AdministratorRoles.Add(new AdministratorRole {
            Administrator = collegeAdmins.VicePresidentAdministration,
            Position = AdministratorPosition.VicePresidentAdministration,
        });
    }

    SaveChanges();
    }

    public AdministratorRole? GetAdministratorRole(AdministratorPosition position) {
        return AdministratorRoles
            .Where(adminRole => adminRole.Position == position)
            .Include(adminRole => adminRole.Administrator)
            .OrderBy(adminRole => adminRole.Id)
            .LastOrDefault();
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

    public User? GetOfficer(int id) {
        return Users.FirstOrDefault(user => user.Id == id);
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

    public List<OfficerRole> GetOfficerRoles(Organization organization) {
        return OfficerRoles.Where(officerRole => officerRole.Organization == organization).ToList();
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
            ? Organizations
                .Where(organization => organization.Adviser == user)
                .Include(organization => organization.Adviser)
                .Distinct()
                .ToList()
            : GetOfficerOrganizations(user);
    }

    public List<Organization> GetOfficerOrganizations(User officer) {
        return OfficerRoles
            .Where(officerRole => officerRole.Officer == officer)
            .Include(officerRole => officerRole.Organization.Adviser)
            .Select(x => x.Organization)
            .Distinct()
            .ToList();
    }

    public Organization? GetOrganization(int id) {
        return Organizations
            .Where(x => x.Id == id)
            .Include(x => x.Adviser)
            .FirstOrDefault();
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
    
    public User? GetAdministrator(AdministratorPosition position) {
        return AdministratorRoles
            .Where(x => x.Position == position)
            .Include(x => x.Administrator)
            .OrderBy(x => x.Id)
            .LastOrDefault()?
            .Administrator;
    }

    public FacilityRequest? GetFacilityRequest(int id) {
        return FacilityRequests
            .Where(facilityRequest => facilityRequest.Id == id)
            .Include(facilityRequest => facilityRequest.Requester)
            .Include(facilityRequest => facilityRequest.Requester.Adviser)
            .FirstOrDefault();
    }
    
    public List<FacilityRequest> GetFacilityRequestsRequested(User user) {
        var organizations = GetOrganizations(user);
        return FacilityRequests.Include(request => request.Requester)
            .Where(request => organizations.Contains(request.Requester))
            .ToList();
    }

    public List<FacilityRequest> GetFacilityRequests() {
        return FacilityRequests 
            .Include(requester => requester.Requester).ToList();
    }

    public List<FacilityRequest> GetFacilityRequests(User user) {
        if (user.Type == UserType.Officer) {
            var organizations = GetOrganizations(user);
            return FacilityRequests.Where(facilityRequest => organizations
                    .Contains(facilityRequest.Requester))
                    .ToList();
        }

        var facilityRequests = AdministratorSignatories
            .Where(signatory => signatory.Role.Administrator == user)
            .Include(signatory => signatory.FacilityRequest.Requester)
            .Select(signatory => signatory.FacilityRequest)
            .Distinct()
            .ToList();

        var showFacilityRequests = new List<FacilityRequest>();
        foreach (var facilityRequest in facilityRequests) {
            var signatories = GetSignatures(facilityRequest);
            var signingStage = signatories.GetSignatureStage();

            if ((signingStage >= SignatureStage.Organization && facilityRequest.Requester.Adviser == user) ||
                (signingStage >= SignatureStage.Deans &&
                 (user == signatories.AssistantDean.User || user == signatories.Dean.User)) ||
                (signingStage >= SignatureStage.BuildingManager && user == signatories.BuildingManager.User) ||
                (signingStage >= SignatureStage.Directors && (user == signatories.AdminServicesDirector.User ||
                                                              user == signatories.StudentAffairsDirector.User ||
                                                              user == signatories.CampusFacilitiesDevelopmentDirector.User ||
                                                              user == signatories.AccountingOfficeDirector.User)) ||
                (signingStage >= SignatureStage.VicePresidents && (user == signatories.VicePresidentAdministration.User ||
                                                                   user == signatories.VicePresidentAcademicAffairs.User))) {
                showFacilityRequests.Add(facilityRequest);
            }   
        }

        return showFacilityRequests;
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

    private Signatures GetDefaultSignatures(FacilityRequest facilityRequest) {
        var officerRoles = OfficerRoles.Where(officerRole => officerRole.Organization == facilityRequest.Requester)
            .Include(officerRole => officerRole.Officer);

        return new Signatures(
            President: new OfficerSignatory {
                FacilityRequest = facilityRequest,
                IsSigned = false,
                Role = officerRoles.FirstOrDefault(officerRole => officerRole.Position == OrganizationPosition.President)
            },
            Adviser: new AdministratorSignatory {
                FacilityRequest = facilityRequest,
                IsSigned = false,
                Role = AdministratorRoles
                    .FirstOrDefault(officerRole => officerRole.Position == AdministratorPosition.Adviser && 
                                                   officerRole.Administrator == facilityRequest.Requester.Adviser)
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

    public List<AdministratorSignatory> GetAdministratorSignatories(FacilityRequest facilityRequest) {
        return AdministratorSignatories
            .Where(adminSignatory => adminSignatory.FacilityRequest == facilityRequest)
            .Include(adminSignatory => adminSignatory.Role)
            .Include(adminSignatory => adminSignatory.Role.Administrator)
            .ToList();
    }

    public List<OfficerSignatory> GetOfficerSignatories(FacilityRequest facilityRequest) {
        return OfficerSignatories
            .Where(officerSignatory => officerSignatory.FacilityRequest == facilityRequest)
            .Include(officerSignatory => officerSignatory.Role)
            .Include(officerSignatory => officerSignatory.Role.Officer)
            .ToList();
    }

    public List<FacultySignatory> GetFacultySignatories(FacilityRequest facilityRequest) {
        return FacultySignatories
            .Where(facultySignatory => facultySignatory.FacilityRequest == facilityRequest)
            .Include(facultySignatory => facultySignatory.Role)
            .Include(facultySignatory => facultySignatory.Role.Faculty)
            .ToList();
    }

    public Signature? GetSignatory(int id, bool isAdmin) {
        return isAdmin
            ? AdministratorSignatories
                .Where(adminSignatory => adminSignatory.Id == id)
                .Include(adminSignatory => adminSignatory.Role)
                .FirstOrDefault()
            : OfficerSignatories
                .Where(officerSignatory => officerSignatory.Id == id)
                .Include(officerSignatory => officerSignatory.Role)
                .FirstOrDefault();
    }
    
    public bool IsApproved(FacilityRequest facilityRequest) {
        var signatories = GetSignatures(facilityRequest);
        return signatories.ToList().All(signatory => signatory.IsSigned);
    }
    
    public Signatures GetSignatures(FacilityRequest facilityRequest) {
        var officerSignatories = GetOfficerSignatories(facilityRequest);
        var adminSignatories = GetAdministratorSignatories(facilityRequest);
        
        var signatories =  new Signatures(
            President: officerSignatories.First(signatory => signatory.Role?.Position == OrganizationPosition.President),
            Adviser: adminSignatories.First(signatory => signatory.Role?.Administrator == facilityRequest.Requester.Adviser),
            
            AssistantDean: adminSignatories.First(signatory => signatory.Role?.Position == AdministratorPosition.AssistantDean),
            Dean: adminSignatories.First(signatory => signatory.Role?.Position == AdministratorPosition.Dean),
            
            BuildingManager: adminSignatories.First(signatory => signatory.Role?.Position == AdministratorPosition.BuildingManager),
            
            AdminServicesDirector: adminSignatories.First(signatory => signatory.Role?.Position == AdministratorPosition.AdminServicesDirector),
            StudentAffairsDirector: adminSignatories.First(signatory => signatory.Role?.Position == AdministratorPosition.StudentAffairsDirector),
            CampusFacilitiesDevelopmentDirector: adminSignatories.First(signatory => signatory.Role?.Position == AdministratorPosition.CampusFacilitiesDevelopmentDirector),
            AccountingOfficeDirector: adminSignatories.First(signatory => signatory.Role?.Position == AdministratorPosition.AccountingOfficeDirector),
            
            VicePresidentAcademicAffairs: adminSignatories.First(signatory => signatory.Role?.Position == AdministratorPosition.VicePresidentAcademicAffairs),
            VicePresidentAdministration: adminSignatories.First(signatory => signatory.Role?.Position == AdministratorPosition.VicePresidentAdministration));

        return signatories;
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
}