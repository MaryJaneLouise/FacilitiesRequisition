using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FacilitiesRequisition.Data;
using FacilitiesRequisition.Models.Administrators;
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
        
        [BindProperty] [RegularExpression(@"^\d{11}$", ErrorMessage = "Please enter a valid 11-digit number.")] public string ContactNumber { get; set; }
        
        [BindProperty] public DateTime? StartDateRequested { get; set; }
        
        [BindProperty] public DateTime? EndDateRequested { get; set; }
        
        [BindProperty] public Venues VenueRequested { get; set; }
        
        [BindProperty] public string? Comments { get; set; }
        
        [BindProperty] public string OrganizationId { get; set; }
        
        public string UserInfo { get; set; }
        
        public bool CanCreateRequests { get; set; }
        public bool AreCollegeAdminsSet { get; set; }

        public IActionResult OnGet() {
            var user = HttpContext.Session.GetLoggedInUser(_context);
            
            if (user == null) {
                return RedirectToPage("/Login/Index");
            }
            
            bool isSuperAdministrator = user.Type == Models.UserType.Administrator &&
                                        _context.GetAdministratorRoles(user).Any(x => x.Position == AdministratorPosition.SuperAdmin);
            var userType = isSuperAdministrator ? "Super Administrator" :
                user.Type == Models.UserType.Administrator ? "Administrator" :
                user.Type == Models.UserType.Faculty ? "Faculty" : "Organization Officer";

            UserInfo = $"{userType}";

            switch (UserInfo) {
                case "Super Administrator" :
                    return RedirectToPage("/Dashboard/Index");
                case "Administrator" :
                    return RedirectToPage("/Dashboard/Index");
                default:
                    CanCreateRequests = _context.CanCreateRequests(user);
                    AreCollegeAdminsSet = _context.AreCollegeAdminsSet();
                    
                    switch (CanCreateRequests) {
                        case true:
                            switch (AreCollegeAdminsSet) {
                                case true:
                                    Organizations = _context.GetOfficerOrganizations(user).Select(organization =>
                                        new SelectListItem {
                                            Value = organization.Id.ToString(),
                                            Text = organization.Name,
                                            Selected = OrganizationId == organization.Id.ToString()
                                        });
                                    return Page();
                                default:
                                    return RedirectToPage("/RequestFacility/Index");
                            }
                        default:
                            return RedirectToPage("/RequestFacility/Index");
                    }
            }
        }
        
        public IActionResult OnPost() {
            OnGet();
            var overlappingRequest = _context.GetFacilityRequests().FirstOrDefault(x =>
                x.VenueRequested == VenueRequested &&
                ((x.StartDateRequested <= StartDateRequested && x.EndDateRequested >= StartDateRequested) ||
                 (x.StartDateRequested <= EndDateRequested && x.EndDateRequested >= EndDateRequested) ||
                 (x.StartDateRequested >= StartDateRequested && x.EndDateRequested <= EndDateRequested)));

            if (overlappingRequest != null) {
                ModelState.AddModelError(string.Empty, $"The selected venue is not available for the specified date range. " + 
                                         $"Please pick other dates for specified venue requested.");
            }
            
            if (!ModelState.IsValid) {
                var officer = HttpContext.Session.GetLoggedInUser(_context)!;
                var organizations = _context.GetOfficerOrganizations(officer);
                Organizations = organizations.Select(organization =>
                    new SelectListItem
                    {
                        Value = organization.Id.ToString(),
                        Text = organization.Name,
                        Selected = OrganizationId == organization.Id.ToString()
                    });
                return Page();
            }
            
            var facilityRequest = new FacilityRequest {
                DateFiled = DateFiled,
                Requester = _context.GetOrganization(Convert.ToInt32(OrganizationId))!,
                NameActivity = NameActivity,
                ContactNumber = ContactNumber,
                StartDateRequested = StartDateRequested,
                EndDateRequested = EndDateRequested,
                VenueRequested = VenueRequested,
                AdditionalComments = Comments   
            };

            _context.AddFacilityRequest(facilityRequest, new List<Expense>());

            return RedirectToPage("./Index");
        }
    }
}
