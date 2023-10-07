using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FacilitiesRequisition.Data;
using FacilitiesRequisition.Models;
using FacilitiesRequisition.Models.Administrators;
using FacilitiesRequisition.Models.Officers;

namespace FacilitiesRequisition.Pages.Organizations
{
    public class DetailsModel : PageModel {
        private readonly DatabaseContext _context;

        public DetailsModel(DatabaseContext context) {
            _context = context;
        }

        public Organization Organization { get; set; } = default!; 
        
        public User? President { get; set; } = default!; 
        
        public User? VicePresident { get; set; } = default!; 
        
        public User? Treasurer { get; set; } = default!; 
        
        public User? Secretary { get; set; } = default!; 
        
        public User? PublicRelationsOfficer { get; set; } = default!; 
        
        public string UserInfo { get; set; }

        public IActionResult OnGet(int? id) {
            var user = HttpContext.Session.GetLoggedInUser(_context);
            
            if (user == null) {
                return RedirectToPage("/Login/Index");
            }
            
            if (id == null || _context.GetOrganizations() == null) {
                return NotFound();
            }
            
            var organization = _context.GetOrganization((int)id);
            
            bool isSuperAdministrator = user.Type == Models.UserType.Administrator &&
                                        _context.GetAdministratorRoles(user).Any(x => x.Position == AdministratorPosition.SuperAdmin);
            var userType = isSuperAdministrator ? "Super Administrator" :
                user.Type == Models.UserType.Administrator ? "Administrator" :
                user.Type == Models.UserType.Faculty ? "Faculty" : "Organization Officer";

            UserInfo = $"{userType}";

            switch (UserInfo) {
                case "Super Administrator":
                    if (organization == null) {
                        return NotFound();
                    }
                    
                    Organization = organization;
                    
                    return Page();
                default:
                    return RedirectToPage("/Dashboard/Index");
            }
        }
    }
}
