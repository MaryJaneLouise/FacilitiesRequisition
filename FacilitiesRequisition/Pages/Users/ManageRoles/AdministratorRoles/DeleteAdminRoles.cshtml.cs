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
    public class DeleteModel : PageModel {
        private readonly DatabaseContext _context;

        public DeleteModel(DatabaseContext context) {
            _context = context;
        }
        
        public User Administrator { get; set; }

        [BindProperty]
        public AdministratorRole AdministratorRole { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)  {
            if (id == null) {
                return NotFound();
            }

            var adminRole = _context.GetAdministratorRole((int)id);
            var user = _context.GetUser(id ?? -1);
            

            if (adminRole == null) {
                return NotFound();
            } else  {
                AdministratorRole = adminRole;
                Administrator = user;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id) {
            if (id == null) {
                return NotFound();
            }
            var adminRole = _context.GetAdministratorRole((int)id);

            if (adminRole != null) {
                AdministratorRole = adminRole;
                _context.RemoveAdministratorRole(AdministratorRole);
            }

            //return RedirectToPage("./Index", new {id = Administrator.Id});
            return RedirectToPage("/Users/Index");
        }

        public IActionResult OnPostBackToIndex() {
            return RedirectToPage("/Users/Index");
        }
    }
}
