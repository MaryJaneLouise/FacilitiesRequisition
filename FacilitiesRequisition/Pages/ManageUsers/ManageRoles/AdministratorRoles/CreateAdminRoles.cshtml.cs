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
        

        public IActionResult OnGet(int? id) {
            var user = _context.GetUser(id ?? -1);
            if (user == null) {
                return NotFound();
            }

            Administrator = user;
            return Page();
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
            
            return RedirectToPage("./Index");
        }
    }
}
