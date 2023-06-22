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
    public class EditSignatureModel : PageModel {
        private readonly DatabaseContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;


        public EditSignatureModel(DatabaseContext context, IWebHostEnvironment hostEnvironment) {
            _context = context;
            webHostEnvironment = hostEnvironment;
        }

        public User User { get; set; } = default!;
        
        
        public string? SignaturePath { get; set; }
        
        [BindProperty]
        public IFormFile? Signature { get; set; }
        
        public string NullSignature { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id) {
            var user = _context.GetUser((int)id);
            
            if (id == null || user == null) {
                return NotFound();
            }
            
            User = user;
            if (user.SignatureFilename != null) {
                SignaturePath = Path.Combine("\\images", user.SignatureFilename);
            }
            return Page();
        }
        
        public async Task<IActionResult> OnPostAsync(int? id, string userId)
        {
            var user = _context.GetUser((int)id);

            if (id == null || user == null) {
                return NotFound();
            }
            User = user;

            if (!ModelState.IsValid) {
                return Page();
            }

            var imageDirectory = Path.Combine(webHostEnvironment.WebRootPath, "images");

            if (!Directory.Exists(imageDirectory)) {
                Directory.CreateDirectory(imageDirectory);
            }

            string? fileName = User.SignatureFilename;

            if (Signature != null) {
                var extension = Signature.FileName.Split(".").LastOrDefault() ?? "jpg";
                fileName = $"{Guid.NewGuid().ToString()}.{extension}";
                var filePath = Path.Combine(imageDirectory, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create)) {
                    await Signature.CopyToAsync(stream);
                }
            }

            User.SignatureFilename = fileName;

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
