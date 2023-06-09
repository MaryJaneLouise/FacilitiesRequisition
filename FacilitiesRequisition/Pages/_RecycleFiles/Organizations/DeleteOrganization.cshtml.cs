using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FacilitiesRequisition.Data;
using FacilitiesRequisition.Models.Administrators;
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
        
        public string UserInfo { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id) {
            if (id == null || _context.GetOrganizations() == null) {
                return NotFound();
            }

            var organization = _context.GetOrganization((int)id);
            var user = HttpContext.Session.GetLoggedInUser(_context)!;
            bool isSuperAdministrator = user.Type == Models.UserType.Administrator &&
                                        _context.GetAdministratorRoles(user).Any(x => x.Position == AdministratorPosition.SuperAdmin);
            var userType = isSuperAdministrator ? "Super Administrator" :
                user.Type == Models.UserType.Administrator ? "Administrator" :
                user.Type == Models.UserType.Faculty ? "Faculty" : "Organization Officer";

            UserInfo = $"{userType}";

            switch (UserInfo) {
                case "Super Administrator":
                    if (organization == null) {
                        return NotFound();
                    } else {
                        Organization = organization;
                    }

                    PageTitle = "Delete organization";
                    return Page();
                default:
                    PageTitle = "Delete organization";
                    return RedirectToPage("/Dashboard/Index");
            }
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
