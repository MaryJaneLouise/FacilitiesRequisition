using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FacilitiesRequisition.Data;
using FacilitiesRequisition.Models;
using FacilitiesRequisition.Models.Faculties;

namespace FacilitiesRequisition.Pages.FacultyRoles {
    public class IndexModel : PageModel {
        private readonly DatabaseContext _context;

        public IndexModel(DatabaseContext context) {
            _context = context;
        }

        public IList<FacultyRole> FacultyRoles { get;set; } = default!;

        [BindProperty] 
        public User Faculty { get; set; } = default!;
        
        [BindProperty]
        public FacultyRole FacultyRole { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id) {
            if (id == null || _context.GetUsers() == null) {
                return NotFound();
            }

            var user = _context.GetUser((int)id);
            if (user == null) {
                return NotFound();
            }

            Faculty = user;
            FacultyRoles = _context.GetFacultyRoles(Faculty);

            return Page();
        }

        public IActionResult OnPostBackToUserIndex() {
            return RedirectToPage("/Users/Index");
        }

        public IActionResult OnPostDeleteRole(int? id) {
            if (id == null) {
                return NotFound();
            }
            var facultyrole = _context.GetFacultyRole((int)id);

            if (facultyrole != null) {
                FacultyRole = facultyrole;
                _context.RemoveFacultyRole(FacultyRole);
                _context.SaveChanges();
            }

            return RedirectToPage("/Users/Index"); 
        }
    }
}
