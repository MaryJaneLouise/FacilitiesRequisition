using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FacilitiesRequisition.Data;
using FacilitiesRequisition.Models;
using FacilitiesRequisition.Models.Officers;

namespace FacilitiesRequisition.Pages.OfficerRoles {
    public class IndexModel : PageModel {
        private readonly DatabaseContext _context;

        public IndexModel(DatabaseContext context) {
            _context = context;
        }

        public IList<OfficerRole> OfficerRoles { get; set; } = default!;

        [BindProperty]
        public User Officer { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id) {
            if (id == null || _context.GetUsers() == null) {
                return NotFound();
            }

            var user = _context.GetUser((int)id);
            if (user == null) {
                return NotFound();
            }

            Officer = user;
            OfficerRoles = _context.GetOfficerRoles(Officer);
            
            return Page();
        }

        public IActionResult OnPostBackToUserIndex() {
            return RedirectToPage("/Users/Index");
        }
    }
}
