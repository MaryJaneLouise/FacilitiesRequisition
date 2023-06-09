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
    public class EditUsernameModel : PageModel {
        private readonly DatabaseContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;


        public EditUsernameModel(DatabaseContext context, IWebHostEnvironment hostEnvironment) {
            _context = context;
            webHostEnvironment = hostEnvironment;
        }

        public User User { get; set; } = default!;
        
        [BindProperty]
        public string Username { get; set; }
        
        public string UsernameDuplicateError { get; set; }
        public string NullUsername { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id) {
            var userLoggedIn = HttpContext.Session.GetLoggedInUser(_context);
            
            if (userLoggedIn == null) {
                return RedirectToPage("/Login/Index");
            }
            
            var user = _context.GetUser((int)id);
            
            if (id == null || user == null) {
                return NotFound();
            }
            
            User = user;
            Username = user.Username;
            return Page();
        }
        
        public async Task<IActionResult> OnPostAsync(int? id, string userId) {
            var user = _context.GetUser((int)id);
            
            if (id == null || user == null) {
                return NotFound();
            }
            User = user;

            if (Username.IsNullOrEmpty()) {
                NullUsername = "This field cannot be empty.";
                return Page();
            }
            
            var isUsernameDuplicate = _context.GetUsers().Any(x => x.Username == Username && Username != user.Username);

            if (isUsernameDuplicate) {
                UsernameDuplicateError =
                    "It seems to have error in your written new username. Please try other possible usernames.";
                return Page();
            }
            if (!ModelState.IsValid) {
                
                return Page();
            }

            User.Username = Username;

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
