using FacilitiesRequisition.Data;
using FacilitiesRequisition.Models;
using FacilitiesRequisition.Models.Administrators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FacilitiesRequisition.Pages.Users.Administrators; 

public class IndexModel : PageModel {
    private readonly FacilitiesRequisition.Data.DatabaseContext _context;
    
    public User? AssistantDean { get; set; } = default!;
    public User? Dean { get; set; } = default!;
    public User? BuildingManager { get; set; } = default!;
    public User? AdminServicesDirector { get; set; } = default!;
    public User? StudentAffairsDirector { get; set; } = default!;
    public User? CampusFacilitiesDirector { get; set; } = default!;
    public User? AccountingOfficeDirector { get; set; } = default!;
    public User? VicePresidentAA { get; set; } = default!;
    public User? VicePresidentAdmin { get; set; } = default!;
    
    public string UserInfo { get; set; }

    

    public IndexModel(DatabaseContext context) {
        _context = context;
    }

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
            case "Super Administrator" :
                AssistantDean = _context.GetAdministrator(AdministratorPosition.AssistantDean);
                Dean = _context.GetAdministrator(AdministratorPosition.Dean);
                BuildingManager = _context.GetAdministrator(AdministratorPosition.BuildingManager);
                AdminServicesDirector = _context.GetAdministrator(AdministratorPosition.AdminServicesDirector);
                StudentAffairsDirector = _context.GetAdministrator(AdministratorPosition.StudentAffairsDirector);
                CampusFacilitiesDirector = _context.GetAdministrator(AdministratorPosition.CampusFacilitiesDevelopmentDirector);
                AccountingOfficeDirector = _context.GetAdministrator(AdministratorPosition.AccountingOfficeDirector);
                VicePresidentAA = _context.GetAdministrator(AdministratorPosition.VicePresidentAcademicAffairs);
                VicePresidentAdmin = _context.GetAdministrator(AdministratorPosition.VicePresidentAdministration);
        
                return Page();
            default:
                return RedirectToPage("/Dashboard/Index");
        }
    }
    
    public IActionResult OnPostBackToUsers() {
        return RedirectToPage("/Users/Index");
    }

    public IActionResult OnPostUpdateAdministrators() {
        return RedirectToPage("./UpdateAdministrators");
    }
}