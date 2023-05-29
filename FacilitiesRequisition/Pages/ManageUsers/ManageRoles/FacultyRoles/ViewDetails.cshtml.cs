using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FacilitiesRequisition.Data;
using FacilitiesRequisition.Models.Faculties;

namespace FacilitiesRequisition.Pages.FacultyRoles
{
    public class DetailsModel : PageModel
    {
        private readonly FacilitiesRequisition.Data.DatabaseContext _context;

        public DetailsModel(FacilitiesRequisition.Data.DatabaseContext context)
        {
            _context = context;
        }

        public FacultyRole FacultyRole { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var facultyrole = _context.GetFacultyRole((int)id);
            if (facultyrole == null)
            {
                return NotFound();
            }
            
            FacultyRole = facultyrole;
            
            return Page();
        }
    }
}
