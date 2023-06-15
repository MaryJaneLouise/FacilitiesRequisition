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
    public class DetailsModel : PageModel
    {
        private readonly DatabaseContext _context;

        public DetailsModel(DatabaseContext context) {
            _context = context;
        }

        public User User { get; set; } = default!; 
        
        public string UserInfo { get; set; }
        
        public string? SignaturePath { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id) {
            if (id == null || _context.GetUsers() == null) {
                return NotFound();
            }

            var user = _context.GetUser((int)id);
            if (user == null) {
                return NotFound();
            } else {
                User = user;
                UserInfo = $"{user.Id}";
                if (user.SignatureFilename != null) {
                    SignaturePath = Path.Combine("\\images", user.SignatureFilename);
                }
            }
            return Page();
        }

        
    }
}
