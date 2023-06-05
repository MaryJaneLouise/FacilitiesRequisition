using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FacilitiesRequisition.Data;
using FacilitiesRequisition.Models;
using FacilitiesRequisition.Models.Administrators;

namespace FacilitiesRequisition.Pages.AdministratorRoles {
    public class IndexModel : PageModel {
        private readonly DatabaseContext _context;

        public IndexModel(DatabaseContext context) {
            _context = context;
        }

        public IList<AdministratorRole> AdministratorRole { get;set; } = default!;
        
        [BindProperty]
        public User Admin { get; set; } = default!;
        
        [BindProperty]
        public AdministratorRole AdministratorRoles { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(int? id) {
            if (id == null || _context.GetUsers() == null) {
                return NotFound();
            }

            var user = _context.GetUser((int)id);
            if (user == null) {
                return NotFound();
            }

            Admin = user;
            AdministratorRole = _context.GetAdministratorRoles(Admin);
            return Page();
        }

        public IActionResult OnPostBackToUserIndex() {
            return RedirectToPage("/Users/Index");
        }

        public IActionResult OnPostCreateRole() {
            return RedirectToPage("./CreateAdminRoles");
        }

        public IActionResult OnPostDeleteRole(int? id) {
            if (id == null) {
                return NotFound();
            }
            var adminRole = _context.GetAdministratorRole((int)id);

            if (adminRole != null) {
                AdministratorRoles = adminRole;
                _context.RemoveAdministratorRole(AdministratorRoles);
            }
            
            return RedirectToPage("/Users/Index");
        }
    }
}
