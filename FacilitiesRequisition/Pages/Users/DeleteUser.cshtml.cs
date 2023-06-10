using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FacilitiesRequisition.Data;
using FacilitiesRequisition.Models;
using FacilitiesRequisition.Models.Administrators;

namespace FacilitiesRequisition.Pages.Users {
    public class DeleteModel : PageModel {
        private readonly DatabaseContext _context;

        public DeleteModel(DatabaseContext context) {
            _context = context;
        }

        [BindProperty]
        public User User { get; set; } = default!;
        
        public string UserInfo { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id) {
            if (id == null || _context.GetUsers() == null) {
                return NotFound();
            }

            var user = _context.GetUser((int)id);
            var userLoggedIn = HttpContext.Session.GetLoggedInUser(_context)!;
            bool isSuperAdministrator = userLoggedIn.Type == Models.UserType.Administrator &&
                                        _context.GetAdministratorRoles(userLoggedIn).Any(x => x.Position == AdministratorPosition.SuperAdmin);
            var userType = isSuperAdministrator ? "Super Administrator" :
                userLoggedIn.Type == Models.UserType.Administrator ? "Administrator" :
                userLoggedIn.Type == Models.UserType.Faculty ? "Faculty" : "Organization Officer";

            UserInfo = $"{userType}";

            switch (UserInfo) {
                case "Super Administrator":
                    if (user == null) {
                        return NotFound();
                    } 
                    
                    User = user;
                    return Page();
                default:
                    return RedirectToPage("/Dashboard/Index");
            }
        }

        public async Task<IActionResult> OnPostAsync(int? id) {
            if (id == null || _context.GetUsers() == null) {
                return NotFound();
            }
            var user = _context.GetUser((int)id);

            if (user != null) {
                User = user;
                _context.RemoveUser(User);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("../Users/Index");
        }
        
        public IActionResult OnPostBackToIndex() {
            return RedirectToPage("../Users/Index");
        }
    }
}
