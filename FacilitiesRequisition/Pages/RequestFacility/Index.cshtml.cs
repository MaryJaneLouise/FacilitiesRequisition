using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FacilitiesRequisition.Data;
using FacilitiesRequisition.Models.FacilityRequests;

namespace FacilitiesRequisition.Pages.RequestFacility
{
    public class IndexModel : PageModel {
        private readonly DatabaseContext _context;

        public IndexModel(DatabaseContext context) {
            _context = context;
        }
        
        public IList<FacilityRequest> FacilityRequest { get;set; } = default!;
        
        [BindProperty]
        public FacilityRequest FacilityRequests { get; set; } = default!;

        public async Task OnGetAsync() {
            FacilityRequest = _context.GetFacilityRequests();
        }
        
        public IActionResult OnPostDeleteRequest(int? id) {
            var facilityrequest = _context.GetFacilityRequest(id ?? -1);

            if (facilityrequest != null) {
                FacilityRequests = facilityrequest;
                _context.RemoveFacilityRequest(FacilityRequests);
                _context.SaveChanges();
            }

            return RedirectToPage("./Index");
        }
    }
}
