using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FacilitiesRequisition.Data;
using FacilitiesRequisition.Models.Officers;

namespace FacilitiesRequisition.Pages.Organizations {
    public class EditModel : PageModel {
        private readonly DatabaseContext _context;

        public EditModel(DatabaseContext context) {
            _context = context;
        }
        
        public string PageTitle { get; set; }
        
        public IEnumerable<SelectListItem> Administrators { get; set; } = default!;
        
        [BindProperty]
        public Organization Organization { get; set; } = default!;

        [BindProperty] 
        public string AdviserId { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id) {
            if (id == null || _context.GetOrganizations() == null) {
                return NotFound();
            }
            
            Administrators = _context.GetAdministrators().Select(adminstrator =>
                new SelectListItem {
                    Value = adminstrator.Id.ToString(),
                    Text = $"{adminstrator.FirstName} {adminstrator.LastName}"
                });

            var organization =  _context.GetOrganization((int)id);
            
            if (organization == null) {
                return NotFound();
            }
            Organization = organization;
            return Page();
            
            PageTitle = "Update organization details";
        }
        
        public async Task<IActionResult> OnPostAsync() {
            if (!ModelState.IsValid) {
                return Page();
            }

            _context.Attach(Organization).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!OrganizationExists(Organization.Id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool OrganizationExists(int id) {
          return _context.GetOrganization(id) != null;
        }

        public IActionResult OnPostBackToIndex() {
            return RedirectToPage("./Index");
        }
    }
}
