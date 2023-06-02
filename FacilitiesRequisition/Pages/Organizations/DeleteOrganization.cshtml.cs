using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FacilitiesRequisition.Data;
using FacilitiesRequisition.Models.Officers;

namespace FacilitiesRequisition.Pages.Organizations {
    public class DeleteModel : PageModel {
        private readonly FacilitiesRequisition.Data.DatabaseContext _context;

        public DeleteModel(FacilitiesRequisition.Data.DatabaseContext context) {
            _context = context;
        }
        
        public string PageTitle { get; set; }
        
        [BindProperty]
        public Organization Organization { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id) {
            if (id == null || _context.GetOrganizations() == null) {
                return NotFound();
            }

            var organization = _context.GetOrganization((int)id);

            if (organization == null) {
                return NotFound();
            } else {
                Organization = organization;
            }

            PageTitle = "Delete organization";
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id) {
            if (id == null || _context.GetOrganizations() == null) {
                return NotFound();
            }
            var organization = _context.GetOrganization((int)id);

            if (organization != null) {
                Organization = organization;
                _context.RemoveOrganization(Organization);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }

        public IActionResult OnPostBackToIndex() {
            return RedirectToPage("./Index");
        }
    }
}
