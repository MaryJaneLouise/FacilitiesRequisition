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
namespace FacilitiesRequisition.Pages.ManageUsers; 

public class ViewProfileModel : PageModel {
    private readonly DatabaseContext _context;

    public ViewProfileModel(DatabaseContext context)  {
        _context = context;
    }
    
    public string PageTitle { get; set; }
    
    [BindProperty]
    public string Name { get; set; }
    public string Username { get; set; }
    
    [BindProperty]
    [Display(Name = "Type of User")]
    public string UserType { get; set; }
    
    public IActionResult OnGet()  {
        var user = HttpContext.Session.GetLoggedInUser(_context);
        bool isSuperAdministrator = user.Type == Models.UserType.Administrator &&
                                    _context.GetAdministratorRoles(user).Any(x => x.Position == AdministratorPosition.SuperAdmin);
        var userType = isSuperAdministrator ? "Super Administrator" : 
            user.Type == Models.UserType.Administrator ? "Administrator" :
            user.Type == Models.UserType.Faculty ? "Faculty" : "Organization Officer";
        
        PageTitle = "Your Profile";
        
        switch (user) {
            case null:
                return RedirectToPage("../Login/Index");
            default:
                Username = $"{user.Username}";
                if (user.MiddleName == null || user.MiddleName == " ") {
                    Name = $"{user.FirstName} {user.LastName}";
                } else {
                    Name = $"{user.FirstName} {user.MiddleName} {user.LastName}";
                }
                UserType = $"{userType}";
                return Page();
        }
    }

    public IActionResult OnPostBackToDashboard() {
        return RedirectToPage("../Dashboard/Index");
    }

    public IActionResult OnPostEditProfile() {
        return RedirectToPage("../ManageUsers/EditDetails");
    }
}