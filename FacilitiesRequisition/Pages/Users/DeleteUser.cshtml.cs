using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FacilitiesRequisition.Data;
using FacilitiesRequisition.Models;

namespace FacilitiesRequisition.Pages.Users {
    public class DeleteModel : PageModel {
        private readonly DatabaseContext _context;

        public DeleteModel(DatabaseContext context) {
            _context = context;
        }

        [BindProperty]
        public User User { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id) {
            if (id == null || _context.GetUsers() == null) {
                return NotFound();
            }

            var user = _context.GetUser((int)id);

            if (user == null) {
                return NotFound();
            } else {
                User = user;
            }
            return Page();
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
