using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FacilitiesRequisition.Data;
using FacilitiesRequisition.Models;
using FacilitiesRequisition.Models.Officers;
using FacilitiesRequisition.Models.FacilityRequests;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FacilitiesRequisition.Pages.RequestFacility
{
    public class IndexModel : PageModel {
        private readonly DatabaseContext _context;

        public IndexModel(DatabaseContext context) {
            _context = context;
        }
        
        public IList<Organization> Organizations { get; set; } = default!;

        public IList<FacilityRequest> FacilityRequest { get;set; } = default!;
        
        [BindProperty]
        public FacilityRequest FacilityRequests { get; set; } = default!;

        public string UserInfo { get; set; } = default!;

        public IActionResult OnGet() {
            var officer = HttpContext.Session.GetLoggedInUser(_context)!;
            UserInfo = $"{officer.Type}";
            switch (UserInfo) {
                case "Administrator" :
                    FacilityRequest = _context.GetFacilityRequests();
                    Organizations = _context.GetOrganizations();
                    break;
                default:
                    Organizations = _context.GetOfficerOrganizations(officer).ToList();
                    FacilityRequest = _context.GetFacilityRequestsRequested(officer).ToList();
                    break;
            }
            return Page();
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
