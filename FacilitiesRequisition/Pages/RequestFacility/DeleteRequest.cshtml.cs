using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FacilitiesRequisition.Data;
using FacilitiesRequisition.Models.FacilityRequests;

namespace FacilitiesRequisition.Pages.RequestFacility {
    public class DeleteModel : PageModel {
        private readonly DatabaseContext _context;

        public DeleteModel(DatabaseContext context) {
            _context = context;
        }

        [BindProperty]
        public FacilityRequest FacilityRequest { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id) {
            var facilityrequest = _context.GetFacilityRequest(id ?? -1);

            if (facilityrequest == null) {
                return NotFound();
            } else {
                FacilityRequest = facilityrequest;
            }
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            var facilityrequest = _context.GetFacilityRequest(id ?? -1);

            if (facilityrequest != null)
            {
                FacilityRequest = facilityrequest;
                _context.RemoveFacilityRequest(FacilityRequest);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }

        public IActionResult OnPostDeleteRequest(int? id) {
            var facilityrequest = _context.GetFacilityRequest(id ?? -1);

            if (facilityrequest != null) {
                FacilityRequest = facilityrequest;
                _context.RemoveFacilityRequest(FacilityRequest);
                _context.SaveChanges();
            }

            return RedirectToPage("./Index");
        }
    }
}
