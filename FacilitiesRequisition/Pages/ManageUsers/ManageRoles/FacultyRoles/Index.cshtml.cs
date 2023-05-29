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

namespace FacilitiesRequisition.Pages.FacultyRoles
{
    public class IndexModel : PageModel
    {
        private readonly FacilitiesRequisition.Data.DatabaseContext _context;

        public IndexModel(FacilitiesRequisition.Data.DatabaseContext context)
        {
            _context = context;
        }

        public IList<FacultyRole> FacultyRoles { get;set; } = default!;

        [BindProperty] 
        public User Faculty { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.GetUsers() == null)
            {
                return NotFound();
            }

            var user = _context.GetUser((int)id);
            if (user == null)
            {
                return NotFound();
            }

            Faculty = user;
            FacultyRoles = _context.GetFacultyRoles(Faculty);

            return Page();
        }
    }
}
