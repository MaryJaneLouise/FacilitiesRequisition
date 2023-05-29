using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FacilitiesRequisition.Data;
using FacilitiesRequisition.Models.Administrators;

namespace FacilitiesRequisition.Pages.AdministratorRoles
{
    public class EditModel : PageModel
    {
        private readonly FacilitiesRequisition.Data.DatabaseContext _context;

        public EditModel(FacilitiesRequisition.Data.DatabaseContext context)
        {
            _context = context;
        }

        [BindProperty]
        public AdministratorRole AdministratorRole { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adminRole =  _context.GetAdministratorRole((int)id);
            if (adminRole == null)
            {
                return NotFound();
            }
            AdministratorRole = adminRole;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(AdministratorRole).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdministratorRoleExists(AdministratorRole.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool AdministratorRoleExists(int id)
        {
          return _context.GetAdministratorRole(id) != null;
        }
    }
}
