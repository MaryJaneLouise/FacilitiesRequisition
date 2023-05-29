using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FacilitiesRequisition.Data;
using FacilitiesRequisition.Models.Officers;

namespace FacilitiesRequisition.Pages.Organizations
{
    public class IndexModel : PageModel
    {
        private readonly FacilitiesRequisition.Data.DatabaseContext _context;

        public IndexModel(FacilitiesRequisition.Data.DatabaseContext context)
        {
            _context = context;
        }

        public IList<Organization> Organizations { get;set; } = default!;

        public IActionResult OnGet()
        {
            Organizations = _context.GetOrganizations();
            return Page();
        }
    }
}
