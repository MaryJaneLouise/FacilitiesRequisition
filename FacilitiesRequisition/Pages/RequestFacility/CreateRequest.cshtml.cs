using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FacilitiesRequisition.Data;
using FacilitiesRequisition.Models.FacilityRequests;

namespace FacilitiesRequisition.Pages.RequestFacility {
    public class CreateModel : PageModel {
        private readonly DatabaseContext _context;

        public CreateModel(DatabaseContext context) {
            _context = context;
        }
        
        public IEnumerable<SelectListItem> Organizations { get; set; } = default!;

        [BindProperty] public string DateFiled { get; set; } = DateTime.Now.ToShortDateString();

        [BindProperty] public string NameActivity { get; set; }
        
        [BindProperty] public string ContactNumber { get; set; }
        
        [BindProperty] public DateTime? StartDateRequested { get; set; }
        
        [BindProperty] public DateTime? EndDateRequested { get; set; }
        
        [BindProperty] public Venues VenueRequested { get; set; }
        
        [BindProperty] public string OrganizationId { get; set; }

        public IActionResult OnGet() {
            var officer = HttpContext.Session.GetLoggedInUser(_context)!;
            Organizations = _context.GetOfficerOrganizations(officer).Select(organization =>
                new SelectListItem {
                    Value = organization.Id.ToString(),
                    Text = organization.Name
                });
            return Page();
        }
        
        public IActionResult OnPost() {
            var overlappingRequest = _context.GetFacilityRequests().FirstOrDefault(x =>
                x.VenueRequested == VenueRequested &&
                ((x.StartDateRequested <= StartDateRequested && x.EndDateRequested >= StartDateRequested) ||
                 (x.StartDateRequested <= EndDateRequested && x.EndDateRequested >= EndDateRequested) ||
                 (x.StartDateRequested >= StartDateRequested && x.EndDateRequested <= EndDateRequested)));

            if (overlappingRequest != null) {
                ModelState.AddModelError(string.Empty, "The selected venue is not available for the specified date range.");
                return Page();
            }
            
            if (!ModelState.IsValid) {
                return Page();
            }

            var facilityRequest = new FacilityRequest {
                DateFiled = DateFiled,
                Requester = _context.GetOrganization(Convert.ToInt32(OrganizationId))!,
                NameActivity = NameActivity,
                ContactNumber = ContactNumber,
                StartDateRequested = StartDateRequested,
                EndDateRequested = EndDateRequested,
                VenueRequested = VenueRequested
            };

            _context.AddFacilityRequest(facilityRequest, new List<Expense>());

            return RedirectToPage("./Index");
        }
    }
}
