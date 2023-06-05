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
    public class IndexModel : PageModel {
        private readonly DatabaseContext _context;

        public IndexModel(DatabaseContext context) {
            _context = context;
        }
        
        [BindProperty] 
        public string SmallDetails { get; set; }
        public IList<Organization> Organizations { get;set; } = default!;
        
        [BindProperty]
        public Organization Organization { get; set; } = default!;

        public IActionResult OnGet() {
            Organizations = _context.GetOrganizations();
            return Page();
        }

        public IActionResult OnPostBackToDashboard() {
            return RedirectToPage("../Dashboard/Index");
        }

        public IActionResult OnPostCreateOrganization() {
            return RedirectToPage("./CreateOrganization");
        }

        public IActionResult OnPostDeleteOrganization(int? id) {
            if (id == null) {
                return NotFound();
            }
            var organization = _context.GetOrganization((int)id);

            if (organization != null) {
                Organization = organization;
                _context.RemoveOrganization(Organization);
                _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
