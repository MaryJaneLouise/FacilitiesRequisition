using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FacilitiesRequisition.Data;
using FacilitiesRequisition.Models;
using FacilitiesRequisition.Models.Administrators;
using FacilitiesRequisition.Models.Officers;

namespace FacilitiesRequisition.Pages.OfficerRoles {
    public class CreateModel : PageModel {
        private readonly DatabaseContext _context;

        public CreateModel(DatabaseContext context) {
            _context = context;
        }
        
        public User Officer { get; set; }
        public IEnumerable<SelectListItem> Organizations { get; set; } = default!;
        
        [BindProperty]
        public string OrganizationId { get; set; }
        
        [BindProperty]
        public OrganizationPosition Position { get; set; }
        
        public string UserInfo { get; set; }

        public IActionResult OnGet(int? id) {
            var user = _context.GetUser(id ?? -1);
            var userLoggedIn = HttpContext.Session.GetLoggedInUser(_context)!;
            bool isSuperAdministrator = userLoggedIn.Type == Models.UserType.Administrator &&
                                        _context.GetAdministratorRoles(userLoggedIn).Any(x => x.Position == AdministratorPosition.SuperAdmin);
            var userType = isSuperAdministrator ? "Super Administrator" :
                userLoggedIn.Type == Models.UserType.Administrator ? "Administrator" :
                userLoggedIn.Type == Models.UserType.Faculty ? "Faculty" : "Organization Officer";

            UserInfo = $"{userType}";

            switch (UserInfo) {
                case "Super Administrator":
                    if (user == null) {
                        return NotFound();
                    }

                    Officer = user;
                    Organizations = _context.GetOrganizations().Select(organization =>
                        new SelectListItem
                        {
                            Value = organization.Id.ToString(),
                            Text = organization.Name
                        });
                    return Page();
                default:
                    return RedirectToPage("/Dashboard/Index");
            }
        }
        
        public IActionResult OnPost(int? id) {
            var user = _context.GetUser(id ?? -1);
            if (user == null) {
                return NotFound();
            }

            Officer = user;
            
            if (!ModelState.IsValid) {
                return Page();
            }

            var officerRole = new OfficerRole {
                Officer = Officer,
                Organization = _context.GetOrganization(Convert.ToInt32(OrganizationId))!,
                Position = Position
            };
            _context.AddOfficerRole(officerRole);


            return RedirectToPage("./Index", new {id = Officer.Id}); 
        }

        public IActionResult OnPostBackToIndex(int? id) {
            var user = _context.GetUser(id ?? -1);
            if (user == null) {
                return NotFound();
            }

            Officer = user;
            return RedirectToPage("./Index", new {id = Officer.Id}); 
        }
    }
}
