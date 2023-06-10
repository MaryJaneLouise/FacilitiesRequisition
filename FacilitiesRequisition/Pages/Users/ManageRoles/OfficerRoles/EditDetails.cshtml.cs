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

namespace FacilitiesRequisition.Pages.OfficerRoles {
    public class EditModel : PageModel {
        private readonly DatabaseContext _context;

        public EditModel(DatabaseContext context) {
            _context = context;
        }

        [BindProperty]
        public OfficerRole OfficerRole { get; set; } = default!;
        
        public string UserInfo { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id) {
            if (id == null) {
                return NotFound();
            }

            var officerRole =  _context.GetOfficerRole((int)id);
            var userLoggedIn = HttpContext.Session.GetLoggedInUser(_context)!;
            bool isSuperAdministrator = userLoggedIn.Type == Models.UserType.Administrator &&
                                        _context.GetAdministratorRoles(userLoggedIn).Any(x => x.Position == AdministratorPosition.SuperAdmin);
            var userType = isSuperAdministrator ? "Super Administrator" :
                userLoggedIn.Type == Models.UserType.Administrator ? "Administrator" :
                userLoggedIn.Type == Models.UserType.Faculty ? "Faculty" : "Organization Officer";

            UserInfo = $"{userType}";

            switch (UserInfo) {
                case "Super Administrator":
                    if (officerRole == null) {
                        return NotFound();
                    }
                    
                    OfficerRole = officerRole;
                    return Page();
                default:
                    return RedirectToPage("/Dashboard/Index");
            }
        }
        
        public async Task<IActionResult> OnPostAsync() {
            if (!ModelState.IsValid) {
                return Page();
            }

            _context.Attach(OfficerRole).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!OfficerRoleExists(OfficerRole.Id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool OfficerRoleExists(int id) {
            return _context.GetOfficerRole(id) != null;        
        }
    }
}
