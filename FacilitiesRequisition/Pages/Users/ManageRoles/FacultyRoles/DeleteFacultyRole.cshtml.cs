using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FacilitiesRequisition.Data;
using FacilitiesRequisition.Models.Faculties;

namespace FacilitiesRequisition.Pages.FacultyRoles {
    public class DeleteModel : PageModel {
        private readonly DatabaseContext _context;

        public DeleteModel(DatabaseContext context) {
            _context = context;
        }

        [BindProperty]
        public FacultyRole FacultyRole { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id) {
            if (id == null) {
                return NotFound();
            }

            var facultyrole = _context.GetFacultyRole((int)id);

            if (facultyrole == null) {
                return NotFound();
            } else {
                FacultyRole = facultyrole;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id) {
            if (id == null) {
                return NotFound();
            }
            var facultyrole = _context.GetFacultyRole((int)id);

            if (facultyrole != null) {
                FacultyRole = facultyrole;
                _context.RemoveFacultyRole(FacultyRole);
            }

            return RedirectToPage("/Users/Index"); 
        }

        public IActionResult OnPostBackToIndex() {
            return RedirectToPage("/Users/Index");
        }
    }
}
