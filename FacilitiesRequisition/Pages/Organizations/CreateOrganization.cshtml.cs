using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FacilitiesRequisition.Data;
using FacilitiesRequisition.Models.Officers;

namespace FacilitiesRequisition.Pages.Organizations {
    public class CreateModel : PageModel {
        private readonly DatabaseContext _context;

        public CreateModel(DatabaseContext context) {
            _context = context;
        }

        public IActionResult OnGet() {
            PageTitle = "Create Organization";
            return Page();
        }
        
        public string PageTitle { get; set; }
        
        [BindProperty]
        public Organization Organization { get; set; } = default!;
        
        public async Task<IActionResult> OnPostAsync() {
          if (!ModelState.IsValid || _context.GetOrganizations() == null || Organization == null) {
                return Page();
          }

          _context.AddOrganization(Organization);
          await _context.SaveChangesAsync();

          return RedirectToPage("./Index");
        }

        public IActionResult OnPostBackToIndex() {
            return RedirectToPage("./Index");
        }
    }
}
