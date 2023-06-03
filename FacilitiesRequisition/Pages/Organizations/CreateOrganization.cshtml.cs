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
        
        public string PageTitle { get; set; }
        
        public IEnumerable<SelectListItem> Administrators { get; set; } = default!;
        
        [BindProperty]
        public Organization Organization { get; set; } = default!;

        [BindProperty] 
        public string AdviserId { get; set; } = default!;
        
        public IActionResult OnGet() {
            Administrators = _context.GetAdministrators().Select(adminstrator =>
                new SelectListItem {
                    Value = adminstrator.Id.ToString(),
                    Text = $"{adminstrator.FirstName} {adminstrator.LastName}"
                });
            PageTitle = "Create Organization";
            return Page();
        }
        
        
        
        public async Task<IActionResult> OnPostAsync() {
          if (!ModelState.IsValid || _context.GetOrganizations() == null || Organization == null) {
                return Page();
          }

          Organization.Adviser = _context.GetUser(Convert.ToInt32(AdviserId));
          
          _context.AddOrganization(Organization);
          await _context.SaveChangesAsync();

          return RedirectToPage("./Index");
        }

        public IActionResult OnPostBackToIndex() {
            return RedirectToPage("./Index");
        }
    }
}
