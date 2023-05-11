using System.ComponentModel.DataAnnotations;
using FacilitiesRequisition.Data;
using FacilitiesRequisition.Models;
using FacilitiesRequisition.Models.Administrators;
using FacilitiesRequisition.Models.Faculties;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;


namespace FacilitiesRequisition.Pages.Dashboard; 

public class IndexModel : PageModel  {
    private readonly DatabaseContext _context;

    public IndexModel(DatabaseContext context)  {
        _context = context;
    }

    [BindProperty]
    public string Name { get; set; }

    public string UserType { get; set; }
    public string IsSuperAdmin { get; set; }

    public IActionResult OnGet()  {
        var user = HttpContext.Session.GetLoggedInUser(_context);
        bool isSuperAdmin = user is Administrator administrator && _context.GetAdministratorRoles(administrator).Any(x => x.Position == AdministratorPosition.SuperAdmin);
        var userType = isSuperAdmin ? "Super Administrator" : user is Administrator ? "Administrator" : user is Faculty ? "Faculty" : "Organization Officer";
        switch (user) {
            case null:
                return RedirectToPage("../Login/Index");
            default:
                Name = $"{user.FirstName} {user.LastName}";
                UserType = $"{userType}";
                return Page();
        }
    }
    
    public async Task<IActionResult> OnPostCreateAccount()  {
        return RedirectToPage("../CreateUsers/Index");
    }
    
    public async Task<IActionResult> OnPostLogout() {
        HttpContext.Session.Logout();
        return RedirectToPage("../Login/Index");
    }
}