using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FacilitiesRequisition.Data;
using FacilitiesRequisition.Models.Administrators;
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
        
        public string UserInfo { get; set; }
        
        public IActionResult OnGet() {
            var user = HttpContext.Session.GetLoggedInUser(_context)!;
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
                    PageTitle = "Create Organization";
                    return Page();
                
                default:
                    PageTitle = "Create Organization";
                    return RedirectToPage("/Dashboard/Index");
            }
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
