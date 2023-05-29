using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FacilitiesRequisition.Data;
using FacilitiesRequisition.Models.Officers;

namespace FacilitiesRequisition.Pages.OfficerRoles
{
    public class DetailsModel : PageModel
    {
        private readonly FacilitiesRequisition.Data.DatabaseContext _context;

        public DetailsModel(FacilitiesRequisition.Data.DatabaseContext context)
        {
            _context = context;
        }

      public OfficerRole OfficerRole { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var officerRole = _context.GetOfficerRole((int)id);
            if (officerRole == null)
            {
                return NotFound();
            }

            OfficerRole = officerRole;
            return Page();
        }
    }
}
