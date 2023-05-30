using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FacilitiesRequisition.Data;
using FacilitiesRequisition.Models;
using FacilitiesRequisition.Models.Administrators;

namespace FacilitiesRequisition.Pages.AdministratorRoles
{
    public class IndexModel : PageModel
    {
        private readonly FacilitiesRequisition.Data.DatabaseContext _context;

        public IndexModel(FacilitiesRequisition.Data.DatabaseContext context)
        {
            _context = context;
        }

        public IList<AdministratorRole> AdministratorRole { get;set; } = default!;
        [BindProperty]
        public User Admin { get; set; } = default!;

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

            Admin = user;
            AdministratorRole = _context.GetAdministratorRoles(Admin);
            return Page();
        }
    }
}
