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

    public IActionResult OnGet() {
        return Page();
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