using FacilitiesRequisition.Data;
using FacilitiesRequisition.Models;
using FacilitiesRequisition.Models.Administrators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FacilitiesRequisition.Pages.Users.Administrators; 

public class UpdateAdministratorsModel : PageModel {
    private readonly DatabaseContext _context;
    
    public IEnumerable<SelectListItem> Admins { get; set; } = default!;

    public UpdateAdministratorsModel(DatabaseContext context) {
        _context = context;
        Admins = _context.GetAdministrators().Select(admin =>
            new SelectListItem {
                Value = admin.Id.ToString(),
                Text = $"{admin.FullName}"
            });
    }

    [BindProperty] public string AssistantDeanId { get; set; } = default!;
    [BindProperty] public string DeanId { get; set; } = default!;
    [BindProperty] public string BuildingManagerId { get; set; } = default!;
    [BindProperty] public string AdminServicesDirectorId { get; set; } = default!;
    [BindProperty] public string StudentAffairsDirectorId { get; set; } = default!;
    [BindProperty] public string CampusFacilitiesDirectorId { get; set; } = default!;
    [BindProperty] public string AccountingOfficeDirectorId { get; set; } = default!;
    [BindProperty] public string VicePresidentAAId { get; set; } = default!;
    [BindProperty] public string VicePresidentAdminId { get; set; } = default!;
    
    public string UserInfo { get; set; }

    public IActionResult OnGet() {
        var userLoggedIn = HttpContext.Session.GetLoggedInUser(_context);

        if (userLoggedIn == null) {
            return RedirectToPage("/Login/Index");
        }
        
        bool isSuperAdministrator = userLoggedIn.Type == Models.UserType.Administrator &&
                                    _context.GetAdministratorRoles(userLoggedIn).Any(x => x.Position == AdministratorPosition.SuperAdmin);
        var userType = isSuperAdministrator ? "Super Administrator" :
            userLoggedIn.Type == Models.UserType.Administrator ? "Administrator" :
            userLoggedIn.Type == Models.UserType.Faculty ? "Faculty" : "Organization Officer";
        
        UserInfo = $"{userType}";

        switch (UserInfo) {
            case "Super Administrator":
                AssistantDeanId = _context.GetAdministrator(AdministratorPosition.AssistantDean).Id.ToString();
                DeanId = _context.GetAdministrator(AdministratorPosition.Dean).Id.ToString();
                BuildingManagerId = _context.GetAdministrator(AdministratorPosition.BuildingManager).Id.ToString();
                AdminServicesDirectorId = _context.GetAdministrator(AdministratorPosition.AdminServicesDirector).Id.ToString();
                StudentAffairsDirectorId = _context.GetAdministrator(AdministratorPosition.StudentAffairsDirector).Id.ToString();
                CampusFacilitiesDirectorId = _context.GetAdministrator(AdministratorPosition.CampusFacilitiesDevelopmentDirector).Id.ToString();
                AccountingOfficeDirectorId = _context.GetAdministrator(AdministratorPosition.AccountingOfficeDirector).Id.ToString();
                VicePresidentAAId = _context.GetAdministrator(AdministratorPosition.VicePresidentAcademicAffairs).Id.ToString();
                VicePresidentAdminId = _context.GetAdministrator(AdministratorPosition.VicePresidentAdministration).Id.ToString();
                
                return Page();
            default:
                return RedirectToPage("/Dashboard/Index");
        }
    }

    public IActionResult OnPost() {
        if (!ModelState.IsValid) {
            return Page();
        }
        
        var assistantDean = _context.GetUser(Convert.ToInt32(AssistantDeanId));
        var dean = _context.GetUser(Convert.ToInt32(DeanId));
        var buildingManager = _context.GetUser(Convert.ToInt32(BuildingManagerId));
        var adminServicesDirector = _context.GetUser(Convert.ToInt32(AdminServicesDirectorId));
        var studentAffairsDirector = _context.GetUser(Convert.ToInt32(StudentAffairsDirectorId));
        var campusFacilitiesDirector = _context.GetUser(Convert.ToInt32(CampusFacilitiesDirectorId));
        var accountingOfficeDirector = _context.GetUser(Convert.ToInt32(AccountingOfficeDirectorId));
        var vicePresidentAA = _context.GetUser(Convert.ToInt32(VicePresidentAAId));
        var vicePresidentAdmin = _context.GetUser(Convert.ToInt32(VicePresidentAdminId));
        
        _context.AddAdministratorRole(new AdministratorRole {
            Administrator = assistantDean!,
            Position = AdministratorPosition.AssistantDean
        });

        _context.AddAdministratorRole(new AdministratorRole {
            Administrator = dean!,
            Position = AdministratorPosition.Dean
        });
        
        _context.AddAdministratorRole(new AdministratorRole {
            Administrator = buildingManager!,
            Position = AdministratorPosition.BuildingManager
        });

        _context.AddAdministratorRole(new AdministratorRole {
            Administrator = adminServicesDirector!,
            Position = AdministratorPosition.AdminServicesDirector
        });

        _context.AddAdministratorRole(new AdministratorRole {
            Administrator = studentAffairsDirector!,
            Position = AdministratorPosition.StudentAffairsDirector
        });
        
        _context.AddAdministratorRole(new AdministratorRole {
            Administrator = campusFacilitiesDirector!,
            Position = AdministratorPosition.CampusFacilitiesDevelopmentDirector
        });

        _context.AddAdministratorRole(new AdministratorRole {
            Administrator = accountingOfficeDirector!,
            Position = AdministratorPosition.AccountingOfficeDirector
        });

        _context.AddAdministratorRole(new AdministratorRole {
            Administrator = vicePresidentAA!,
            Position = AdministratorPosition.VicePresidentAcademicAffairs
        });

        _context.AddAdministratorRole(new AdministratorRole {
            Administrator = vicePresidentAdmin!,
            Position = AdministratorPosition.VicePresidentAdministration
        });

        return RedirectToPage("./Index");
    }

    public IActionResult OnPostBackToIndex() {
        return RedirectToPage("./Index");
    }
}