using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FacilitiesRequisition.Data;
using FacilitiesRequisition.Models.Administrators;
using FacilitiesRequisition.Models.Faculties;

namespace FacilitiesRequisition.Pages.FacultyRoles {
    public class DeleteModel : PageModel {
        private readonly DatabaseContext _context;

        public DeleteModel(DatabaseContext context) {
            _context = context;
        }

        [BindProperty]
        public FacultyRole FacultyRole { get; set; } = default!;
        
        public string UserInfo { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id) {
            if (id == null) {
                return NotFound();
            }

            var facultyrole = _context.GetFacultyRole((int)id);
            var userLoggedIn = HttpContext.Session.GetLoggedInUser(_context)!;
            bool isSuperAdministrator = userLoggedIn.Type == Models.UserType.Administrator &&
                                        _context.GetAdministratorRoles(userLoggedIn).Any(x => x.Position == AdministratorPosition.SuperAdmin);
            var userType = isSuperAdministrator ? "Super Administrator" :
                userLoggedIn.Type == Models.UserType.Administrator ? "Administrator" :
                userLoggedIn.Type == Models.UserType.Faculty ? "Faculty" : "Organization Officer";

            UserInfo = $"{userType}";

            switch (UserInfo) {
                case "Super Administrator":
                    if (facultyrole == null) {
                        return NotFound();
                    } 
                    
                    FacultyRole = facultyrole;
                    return Page();
                default:
                    return RedirectToPage("/Dashboard/Index");
            }
        }

        public async Task<IActionResult> OnPostAsync(int? id) {
            if (id == null) {
                return NotFound();
            }
            var facultyrole = _context.GetFacultyRole((int)id);

            if (facultyrole != null) {
                FacultyRole = facultyrole;
                _context.RemoveFacultyRole(FacultyRole);
            }

            return RedirectToPage("/Users/Index"); 
        }

        public IActionResult OnPostBackToIndex() {
            return RedirectToPage("/Users/Index");
        }
    }
}
