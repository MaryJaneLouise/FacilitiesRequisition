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
using FacilitiesRequisition.Models.FacilityRequests;

namespace FacilitiesRequisition.Pages.RequestFacility {
    public class EditModel : PageModel {
        private readonly DatabaseContext _context;

        public EditModel(DatabaseContext context) {
            _context = context;
        }

        [BindProperty]
        public FacilityRequest FacilityRequest { get; set; } = default!;
        
        [BindProperty] public Venues VenueRequested { get; set; }
        
        public string UserInfo { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id) {
            var user = HttpContext.Session.GetLoggedInUser(_context);
            
            if (user == null) {
                return RedirectToPage("/Login/Index");
            }
            
            var facilityrequest =  _context.GetFacilityRequest(id ?? -1);
            
            bool isSuperAdministrator = user.Type == Models.UserType.Administrator &&
                                        _context.GetAdministratorRoles(user).Any(x => x.Position == AdministratorPosition.SuperAdmin);
            var userType = isSuperAdministrator ? "Super Administrator" :
                user.Type == Models.UserType.Administrator ? "Administrator" :
                user.Type == Models.UserType.Faculty ? "Faculty" : "Organization Officer";

            UserInfo = $"{userType}";

            switch (UserInfo) {
                case "Super Administrator" :
                    return RedirectToPage("/Dashboard/Index");
                default:
                    if (facilityrequest == null) {
                        return NotFound();
                    }
            
                    FacilityRequest = facilityrequest;
                    return Page();
            }
        }
        
        public async Task<IActionResult> OnPostAsync() {
            var overlappingRequest = _context.GetFacilityRequests().FirstOrDefault(x =>
                x.VenueRequested == VenueRequested &&
                ((x.StartDateRequested <= FacilityRequest.StartDateRequested && x.EndDateRequested >= FacilityRequest.StartDateRequested) ||
                 (x.StartDateRequested <= FacilityRequest.EndDateRequested && x.EndDateRequested >= FacilityRequest.EndDateRequested) ||
                 (x.StartDateRequested >= FacilityRequest.StartDateRequested && x.EndDateRequested <= FacilityRequest.EndDateRequested)));

            if (overlappingRequest != null) {
                ModelState.AddModelError(string.Empty, $"The selected venue is not available for the specified date range. " + 
                                                       $"Please pick other dates for specified venue requested.");
            }

            _context.Attach(FacilityRequest).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!FacilityRequestExists(FacilityRequest.Id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool FacilityRequestExists(int id) {
          return _context.GetFacilityRequest(id) != null;
        }
    }
}
