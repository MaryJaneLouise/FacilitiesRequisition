using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FacilitiesRequisition.Data;
using FacilitiesRequisition.Models.Administrators;
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
        
        public string UserInfo { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id) {
            var user = HttpContext.Session.GetLoggedInUser(_context);
            
            if (user == null) {
                return RedirectToPage("/Login/Index");
            }
            
            if (id == null || _context.GetOrganizations() == null) {
                return NotFound();
            }
            
            var organization =  _context.GetOrganization((int)id);

            bool isSuperAdministrator = user.Type == Models.UserType.Administrator &&
                                        _context.GetAdministratorRoles(user).Any(x => x.Position == AdministratorPosition.SuperAdmin);
            var userType = isSuperAdministrator ? "Super Administrator" :
                user.Type == Models.UserType.Administrator ? "Administrator" :
                user.Type == Models.UserType.Faculty ? "Faculty" : "Organization Officer";

            UserInfo = $"{userType}";

            switch (UserInfo) {
                case "Super Administrator":
                    Administrators = _context.GetAdministrators().Select(adminstrator =>
                        new SelectListItem {
                            Value = adminstrator.Id.ToString(),
                            Text = $"{adminstrator.FirstName} {adminstrator.LastName}"
                        });
                    
                    if (organization == null) {
                        return NotFound();
                    }
                    Organization = organization;
                    PageTitle = "Update organization details";
                    return Page();
                    
                default:
                    PageTitle = "Update organization details";
                    return RedirectToPage("/Dashboard/Index");
            }
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
