using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FacilitiesRequisition.Data;
using FacilitiesRequisition.Models;

namespace FacilitiesRequisition.Pages.Users {
    public class EditModel : PageModel {
        private readonly DatabaseContext _context;

        public EditModel(DatabaseContext context) {
            _context = context;
        }

        [BindProperty]
        public User User { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id) {
            if (id == null || _context.GetUsers() == null) {
                return NotFound();
            }

            var user =  _context.GetUser((int)id);
            if (user == null) {
                return NotFound();
            }
            User = user;
            return Page();
        }
        
        public async Task<IActionResult> OnPostAsync() {
            if (!ModelState.IsValid) {
                return Page();
            }

            _context.Attach(User).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!UserExists(User.Id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return RedirectToPage("../Users/Index");
        }

        private bool UserExists(int id) {
          return _context.GetUsers().Any(x => x.Id == id);
        }

        public IActionResult OnPostBackToIndex() {
            return RedirectToPage("../Users/Index");
        }
    }
}
