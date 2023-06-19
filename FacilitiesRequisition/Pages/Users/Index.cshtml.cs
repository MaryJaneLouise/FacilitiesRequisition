using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FacilitiesRequisition.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FacilitiesRequisition.Models;
using FacilitiesRequisition.Models.Administrators;

namespace FacilitiesRequisition.Pages.Users {
    public class IndexModel : PageModel {
        private readonly Data.DatabaseContext _context;

        public IndexModel(Data.DatabaseContext context) {
            _context = context;
        }

        public List<User> User { get;set; } = default!;
        public User Users { get; set; } = default!;
        
        public string UserInfo { get; set; }

        public IActionResult OnGet() {
            var userLoggedIn = HttpContext.Session.GetLoggedInUser(_context)!;
            bool isSuperAdministrator = userLoggedIn.Type == Models.UserType.Administrator &&
                                        _context.GetAdministratorRoles(userLoggedIn).Any(x => x.Position == AdministratorPosition.SuperAdmin);
            var userType = isSuperAdministrator ? "Super Administrator" :
                userLoggedIn.Type == Models.UserType.Administrator ? "Administrator" :
                userLoggedIn.Type == Models.UserType.Faculty ? "Faculty" : "Organization Officer";

            UserInfo = $"{userType}";

            switch (UserInfo) {
                case "Super Administrator":
                    if (_context.GetUsers() != null) {
                        User = _context.GetUsers();
                    }

                    return Page();
                default:
                    return RedirectToPage("/Dashboard/Index");
            }
        }
        
        public IActionResult OnPostBackToDashboard() {
            return RedirectToPage("../Dashboard/Index");
        }

        public IActionResult OnPostCreateUser() {
            return RedirectToPage("./CreateUser");
        }
        
        public IActionResult OnPostManageAdmins() {
            return RedirectToPage("/Users/Administrators/Index");
        }

        public IActionResult OnPostDeleteUser(int? id) {
            if (id == null || _context.GetUsers() == null) {
                return NotFound();
            }
            var user = _context.GetUser((int)id);

            if (user != null) {
                Users = user;
                _context.RemoveUser(Users); 
                _context.SaveChanges();
            }

            return RedirectToPage("../Users/Index");
        }
    }
}