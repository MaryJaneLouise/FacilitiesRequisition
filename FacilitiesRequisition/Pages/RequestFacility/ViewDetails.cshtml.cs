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
    public class DetailsModel : PageModel {
        private readonly DatabaseContext _context;

        public DetailsModel(DatabaseContext context) {
            _context = context;
        }

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
    }
}
