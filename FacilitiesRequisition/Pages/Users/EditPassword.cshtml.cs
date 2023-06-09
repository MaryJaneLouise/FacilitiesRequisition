using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FacilitiesRequisition.Data;
using FacilitiesRequisition.Models;
using Microsoft.IdentityModel.Tokens;

namespace FacilitiesRequisition.Pages.Users {
    public class EditPasswordModel : PageModel {
        private readonly DatabaseContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;


        public EditPasswordModel(DatabaseContext context, IWebHostEnvironment hostEnvironment) {
            _context = context;
            webHostEnvironment = hostEnvironment;
        }

        public User User { get; set; } = default!;

        [BindProperty]
        public string? Password { get; set; }
        
        [BindProperty]
        public string? RepeatPassword { get; set; }
        
        
        public string NullPassword { get; set; }
        public string NotSamePassword { get; set; }
        
        public async Task<IActionResult> OnGetAsync(int? id) {
            var user = _context.GetUser((int)id);
            
            if (id == null || user == null) {
                return NotFound();
            }
            
            User = user;
            return Page();
        }
        
        public async Task<IActionResult> OnPostAsync(int? id, string userId) {
            var user = _context.GetUser((int)id);
            
            if (id == null || user == null) {
                return NotFound();
            }
            User = user;

            if (Password.IsNullOrEmpty() || RepeatPassword.IsNullOrEmpty()) {
                NullPassword = "This field cannot be empty.";
                return Page();
            }

            if (Password != RepeatPassword) {
                NotSamePassword = "The written passwords are not the same. Please try again.";
                return Page();
            }
            
            if (!ModelState.IsValid) {
                return Page();
            }
            
            var passwordSalt = PasswordHash.GenerateSalt();
            var passwordHash = Password.ComputeHash(passwordSalt);
            
            
            User.PasswordHash = passwordHash;
            User.PasswordSalt = Convert.ToBase64String(passwordSalt);
            
            _context.Attach(User).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!UserExists(User.Id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return RedirectToPage("/Users/ViewDetails", new { id = userId }); 
        }

        private bool UserExists(int id) {
          return _context.GetUsers().Any(e => e.Id == id);
        }

        public IActionResult OnPostBackToIndex() {
            return RedirectToPage("./Index");
        }
    }
}
