using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FacilitiesRequisition.Models;

namespace FacilitiesRequisition.Pages.Users {
    public class IndexModel : PageModel {
        private readonly Data.DatabaseContext _context;

        public IndexModel(Data.DatabaseContext context) {
            _context = context;
        }

        public List<User> User { get;set; } = default!;
        public User Users { get; set; } = default!;


        public async Task OnGetAsync() {
            if (_context.GetUsers() != null) {
                User = _context.GetUsers();
            }
        }
        
        public IActionResult OnPostBackToDashboard() {
            return RedirectToPage("../Dashboard/Index");
        }

        public IActionResult OnPostCreateUser() {
            return RedirectToPage("./CreateUser");
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