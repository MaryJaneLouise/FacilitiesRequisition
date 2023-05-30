using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FacilitiesRequisition.Data;
using FacilitiesRequisition.Models.Officers;

namespace FacilitiesRequisition.Pages.OfficerRoles
{
    public class EditModel : PageModel
    {
        private readonly FacilitiesRequisition.Data.DatabaseContext _context;

        public EditModel(FacilitiesRequisition.Data.DatabaseContext context)
        {
            _context = context;
        }

        [BindProperty]
        public OfficerRole OfficerRole { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var officerRole =  _context.GetOfficerRole((int)id);
            if (officerRole == null)
            {
                return NotFound();
            }
            OfficerRole = officerRole;
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

            _context.Attach(OfficerRole).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OfficerRoleExists(OfficerRole.Id))
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

        private bool OfficerRoleExists(int id)
        {
            return _context.GetOfficerRole(id) != null;        
        }
    }
}