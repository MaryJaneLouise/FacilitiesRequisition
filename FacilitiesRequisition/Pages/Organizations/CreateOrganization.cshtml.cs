using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FacilitiesRequisition.Data;
using FacilitiesRequisition.Models.Administrators;
using FacilitiesRequisition.Models.Officers;

namespace FacilitiesRequisition.Pages.Organizations {
    public class CreateModel : PageModel {
        private readonly DatabaseContext _context;

        public CreateModel(DatabaseContext context) {
            _context = context;
        }
        
        public string PageTitle { get; set; }
        
        public IEnumerable<SelectListItem> Administrators { get; set; } = default!;
        public IEnumerable<SelectListItem> Officers { get; set; } = default!;

        [BindProperty] public Organization Organization { get; set; } = default!;
        [BindProperty] public string AdviserId { get; set; } = default!;
        [BindProperty] public string PresidentId { get; set; } = default!;
        [BindProperty] public string VicePresidentId { get; set; } = default!;
        [BindProperty] public string SecretaryId { get; set; } = default!;
        [BindProperty] public string TreasurerId { get; set; } = default!;
        [BindProperty] public string AuditorId { get; set; } = default!;
        [BindProperty] public string PublicRelationsOfficerId { get; set; } = default!;
        
        public string UserInfo { get; set; }
        
        public IActionResult OnGet() {
            var user = HttpContext.Session.GetLoggedInUser(_context)!;
            bool isSuperAdministrator = user.Type == Models.UserType.Administrator &&
                                        _context.GetAdministratorRoles(user).Any(x => x.Position == AdministratorPosition.SuperAdmin);
            var userType = isSuperAdministrator ? "Super Administrator" :
                user.Type == Models.UserType.Administrator ? "Administrator" :
                user.Type == Models.UserType.Faculty ? "Faculty" : "Organization Officer";

            UserInfo = $"{userType}";

            switch (UserInfo) {
                case "Super Administrator":
                    Administrators = _context.GetAdministrators().Select(adminstrator =>
                        new SelectListItem {
                            Value = adminstrator.Id.ToString(),
                            Text = $"{adminstrator.FirstName} {adminstrator.LastName}"
                        });
                    Officers = _context.GetOfficers().Select(officer =>
                        new SelectListItem {
                            Value = officer.Id.ToString(),
                            Text = $"{officer.FirstName} {officer.LastName}"
                        });
                    PageTitle = "Create Organization";
                    return Page();
                
                default:
                    PageTitle = "Create Organization";
                    return RedirectToPage("/Dashboard/Index");
            }
        }
        
        
        
        public async Task<IActionResult> OnPostAsync() {
            if (!ModelState.IsValid) {
                return Page();
            }

            Organization.Adviser = _context.GetUser(Convert.ToInt32(AdviserId));
          
            _context.AddOrganization(Organization);
            await _context.SaveChangesAsync();
            
            var president = _context.GetOfficer(Convert.ToInt32(PresidentId));
            var vicePresident = _context.GetOfficer(Convert.ToInt32(VicePresidentId));
            var secretary = _context.GetOfficer(Convert.ToInt32(SecretaryId));
            var treasurer = _context.GetOfficer(Convert.ToInt32(TreasurerId));
            var auditor = _context.GetOfficer(Convert.ToInt32(AuditorId));
            var publicRelationsOfficer = _context.GetOfficer(Convert.ToInt32(PublicRelationsOfficerId));

            _context.AddOfficerRole(new OfficerRole {
                Officer = president!,
                Organization = Organization,
                Position = OrganizationPosition.President
            });
            _context.AddOfficerRole(new OfficerRole {
                Officer = vicePresident!,
                Organization = Organization,
                Position = OrganizationPosition.VicePresident
            });
            _context.AddOfficerRole(new OfficerRole {
                Officer = secretary!,
                Organization = Organization,
                Position = OrganizationPosition.Secretary
            });
            _context.AddOfficerRole(new OfficerRole {
                Officer = treasurer!,
                Organization = Organization,
                Position = OrganizationPosition.Treasurer
            });
            _context.AddOfficerRole(new OfficerRole {
                Officer = auditor!,
                Organization = Organization,
                Position = OrganizationPosition.Auditor
            });
            _context.AddOfficerRole(new OfficerRole {
                Officer = publicRelationsOfficer!,
                Organization = Organization,
                Position = OrganizationPosition.PublicRelationsOfficer
            });

            return RedirectToPage("./Index");
        }

        public IActionResult OnPostBackToIndex() {
            return RedirectToPage("./Index");
        }
    }
}
