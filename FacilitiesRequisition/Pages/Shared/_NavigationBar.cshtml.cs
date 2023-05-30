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


namespace FacilitiesRequisition.Pages.Shared {
    public class _NavigationBarModel : PageModel  {
        private readonly DatabaseContext _context;
    
        public _NavigationBarModel(DatabaseContext context)  {
            _context = context;
        }
    
        [BindProperty]
        public string Name { get; set; }
    
        public string UserType { get; set; }
        public string IsSuperAdmin { get; set; }
    
        public IActionResult OnGet()  {
            var user = HttpContext.Session.GetLoggedInUser(_context);
            bool isSuperAdministrator = user.Type == Models.UserType.Administrator &&
                _context.GetAdministratorRoles(user).Any(x => x.Position == AdministratorPosition.SuperAdmin);
            var userType = isSuperAdministrator ? "Super Administrator" : 
                user.Type == Models.UserType.Administrator ? "Administrator" :
                user.Type == Models.UserType.Faculty ? "Faculty" : "Organization Officer";
            
            switch (user) {
                case null:
                    return RedirectToPage("../Login/Index");
                default:
                    Name = $"{user.FirstName} {user.LastName}";
                    UserType = $"{userType}";
                    return Page();
            }
        }
        
        public IActionResult OnPostCreateAccount()  {
            return RedirectToPage("../Users/CreateUser");
        }
        
        public IActionResult OnPostCreateOrganization()  {
            return RedirectToPage("../Organizations/CreateOrganization");
        }
    
        public IActionResult OnPostManageUserAccount() {
            return RedirectToPage("../Users/Index");
        }
        
        public IActionResult OnPostManageOrganization() {
            return RedirectToPage("../Organizations/Index");
        }
    
        public IActionResult OnPostViewProfile() {
            return RedirectToPage("../ManageUsers/ViewDetails");
        }
        
        public IActionResult OnPostLogout() {
            HttpContext.Session.Logout();
            return RedirectToPage("../Login/Index");
        }
    
        public IActionResult OnPostRequestPaper() {
            return RedirectToPage("../CreateRequestPaper/Index");
        }
    
        public IActionResult OnPostHome() {
            return RedirectToPage("../Dashboard/Index");
        }
    }
}

