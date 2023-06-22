using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FacilitiesRequisition.Data;
using FacilitiesRequisition.Models.Administrators;
using FacilitiesRequisition.Models.Officers;

namespace FacilitiesRequisition.Pages.Organizations {
    public class IndexModel : PageModel {
        private readonly DatabaseContext _context;

        public IndexModel(DatabaseContext context) {
            _context = context;
        }
        
        [BindProperty] 
        public string SmallDetails { get; set; }
        public IList<Organization> Organizations { get;set; } = default!;
        
        [BindProperty]
        public Organization Organization { get; set; } = default!;
        
        public string UserInfo { get; set; }

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
                case "Super Administrator":
                    Organizations = _context.GetOrganizations();
                    return Page();
                default:
                    return RedirectToPage("/Dashboard/Index");
            }
        }

        public IActionResult OnPostBackToDashboard() {
            return RedirectToPage("../Dashboard/Index");
        }

        public IActionResult OnPostCreateOrganization() {
            return RedirectToPage("./CreateOrganization");
        }

        public IActionResult OnPostDeleteOrganization(int? id) {
            if (id == null) {
                return NotFound();
            }
            var organization = _context.GetOrganization((int)id);

            if (organization != null) {
                Organization = organization;
                _context.RemoveOrganization(Organization);
                _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
    
    public static class EnumExtensions {
        public static string GetDisplayName(this Enum value) {
            var displayAttribute = value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DisplayAttribute), false)
                .FirstOrDefault() as DisplayAttribute;

            return displayAttribute?.GetName() ?? value.ToString();
        }
    }
}
