using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FacilitiesRequisition.Data;
using FacilitiesRequisition.Models;
using FacilitiesRequisition.Models.Administrators;

namespace FacilitiesRequisition.Pages.AdministratorRoles  {
    public class CreateAdminRolesModel : PageModel  {
        private readonly DatabaseContext _context;

        public CreateAdminRolesModel(DatabaseContext context)  {
            _context = context;
        }
        
        public User Administrator { get; set; }
        
        [BindProperty]
        public AdministratorPosition Position { get; set; }
        
        public string UserInfo { get; set; }
        

        public IActionResult OnGet(int? id) {
            var user = _context.GetUser(id ?? -1);
            var userLoggedIn = HttpContext.Session.GetLoggedInUser(_context)!;
            bool isSuperAdministrator = userLoggedIn.Type == Models.UserType.Administrator &&
                                        _context.GetAdministratorRoles(userLoggedIn).Any(x => x.Position == AdministratorPosition.SuperAdmin);
            var userType = isSuperAdministrator ? "Super Administrator" :
                userLoggedIn.Type == Models.UserType.Administrator ? "Administrator" :
                userLoggedIn.Type == Models.UserType.Faculty ? "Faculty" : "Organization Officer";

            UserInfo = $"{userType}";

            switch (UserInfo) {
                case "Super Administrator":
                    if (user == null) {
                        return NotFound();
                    }

                    Administrator = user;
                    return Page();
                default:
                    return RedirectToPage("/Dashboard/Index");
            }
        }
        
        public IActionResult OnPost(int? id) {
            var user = _context.GetUser(id ?? -1);
            if (user == null)  {
                return NotFound();
            }

            Administrator = user;
            
            if (!ModelState.IsValid) {
                return Page();
            }

            var adminRole = new AdministratorRole {
                Administrator = Administrator,
                Position = Position
            };
            _context.AddAdministratorRole(adminRole);
            
            return RedirectToPage("./Index", new {id = Administrator.Id}); 
        }

        public IActionResult OnPostBackToIndex() {
            return RedirectToPage("./Index", new {id = Administrator.Id}); 
        }
    }
}
