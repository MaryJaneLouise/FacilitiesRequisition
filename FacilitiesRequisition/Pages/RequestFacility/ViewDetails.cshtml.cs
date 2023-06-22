using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FacilitiesRequisition.Data;
using FacilitiesRequisition.Models;
using FacilitiesRequisition.Models.Users;
using FacilitiesRequisition.Models.FacilityRequests;

namespace FacilitiesRequisition.Pages.RequestFacility {
    public class DetailsModel : PageModel {
        private readonly DatabaseContext _context;

        public DetailsModel(DatabaseContext context) {
            _context = context;
        }
        
        public User User { get; set; } = default!;
        public FacilityRequest FacilityRequest { get; set; } = default!; 
        
        public Signatures Signatories { get; set; } = default!;
        
        public string Status { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id) {
            var user = HttpContext.Session.GetLoggedInUser(_context);
            
            if (user == null) {
                return RedirectToPage("/Login/Index");
            }
            
            var facilityrequest = _context.GetFacilityRequest(id ?? -1);

            if (facilityrequest == null || user == null) {
                return NotFound();
            }

            User = user;
            FacilityRequest = facilityrequest;
            Signatories = _context.GetSignatures(facilityrequest);
            
            return Page();
        }

        public IActionResult OnPostSign(int? facilityRequestId, int? signatoryId, bool isAdmin) {
            var user = HttpContext.Session.GetLoggedInUser(_context);
            var facilityRequest = _context.GetFacilityRequest(facilityRequestId ?? -1);
            var signatory = _context.GetSignatory(signatoryId ?? -1, isAdmin);

            if (user == null || facilityRequest == null || signatory == null) {
                return NotFound();
            }

            User = user;
            FacilityRequest = facilityRequest;
            Signatories = _context.GetSignatures(facilityRequest);
            
            signatory.IsSigned = true;
            _context.SaveChanges();
            
            return Page();
        }
        
        public IActionResult OnPostUnsign(int? facilityRequestId, int? signatoryId, bool isAdmin) {
            var user = HttpContext.Session.GetLoggedInUser(_context);
            var facilityRequest = _context.GetFacilityRequest(facilityRequestId ?? -1);
            var signatory = _context.GetSignatory(signatoryId ?? -1, isAdmin);

            if (user == null || facilityRequest == null || signatory == null) {
                return NotFound();
            }

            User = user;
            FacilityRequest = facilityRequest;
            Signatories = _context.GetSignatures(facilityRequest);
            
            signatory.IsSigned = false;
            _context.SaveChanges();
            
            return Page();
        }
    }
}
