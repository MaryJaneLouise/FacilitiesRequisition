using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FacilitiesRequisition.Data;
using FacilitiesRequisition.Models.Administrators;

namespace FacilitiesRequisition.Pages.AdministratorRoles {
    public class DeleteModel : PageModel {
        private readonly FacilitiesRequisition.Data.DatabaseContext _context;

        public DeleteModel(FacilitiesRequisition.Data.DatabaseContext context)
        {
            _context = context;
        }

        [BindProperty]
        public AdministratorRole AdministratorRole { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)  {
            if (id == null) {
                return NotFound();
            }

            var adminRole = _context.GetAdministratorRole((int)id);

            if (adminRole == null) {
                return NotFound();
            } else  {
                AdministratorRole = adminRole;
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

            return RedirectToPage("./Index");
        }
    }
}
