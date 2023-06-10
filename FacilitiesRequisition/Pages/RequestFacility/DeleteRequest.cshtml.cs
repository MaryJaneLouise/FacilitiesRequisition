using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FacilitiesRequisition.Data;
using FacilitiesRequisition.Models.Administrators;
using FacilitiesRequisition.Models.FacilityRequests;

namespace FacilitiesRequisition.Pages.RequestFacility {
    public class DeleteModel : PageModel {
        private readonly DatabaseContext _context;

        public DeleteModel(DatabaseContext context) {
            _context = context;
        }

        [BindProperty]
        public FacilityRequest FacilityRequest { get; set; } = default!;
        
        public string UserInfo { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id) {
            var facilityRequest = _context.GetFacilityRequest(id ?? -1);
            
            var user = HttpContext.Session.GetLoggedInUser(_context)!;
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
                    if (facilityRequest == null) {
                        return NotFound();
                    } 
                    FacilityRequest = facilityRequest;
            
                    return Page();
            }
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
