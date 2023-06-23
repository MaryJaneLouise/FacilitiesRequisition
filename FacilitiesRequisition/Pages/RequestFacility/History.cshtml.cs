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
using FacilitiesRequisition.Models.FacilityRequests;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FacilitiesRequisition.Pages.RequestFacility
{
    public class HistoryModel : PageModel {
        private readonly DatabaseContext _context;

        public HistoryModel(DatabaseContext context) {
            _context = context;
        }
        
        public IList<Organization> Organizations { get; set; } = default!;
        public IList<FacilityRequest> FacilityRequest { get;set; } = default!;
        [BindProperty] public FacilityRequest FacilityRequests { get; set; } = default!;
        public List<bool> IsApproved { get; set; } = new();

        public string UserInfo { get; set; } = default!;
        public new User User { get; set; } = default!;
        public string ForSuperAdmins { get; set; }
        
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
                user.Type == UserType.Administrator ? "Administrator" :
                user.Type == UserType.Faculty ? "Faculty" : "Organization Officer";
            
            UserInfo = $"{userType}";
            switch (UserInfo) {
                case "Super Administrator" :
                    return RedirectToPage("/Dashboard/Index");
                    ForSuperAdmins = $"Do you think you can access this page just because you have super administrator role? " +
                                     $"You're out of your mind.";
                    break;
                case "Administrator" :
                    FacilityRequest = _context.GetFacilityRequests();
                    Organizations = _context.GetOrganizations();
                    User = user;
                    foreach (var request in FacilityRequest) {
                        IsApproved.Add(_context.IsApproved(request));
                    }
                    break;
                default:
                    Organizations = _context.GetOfficerOrganizations(user).ToList();
                    FacilityRequest = _context.GetFacilityRequestsRequested(user).ToList();
                    CanCreateRequests = _context.CanCreateRequests(user);
                    AreCollegeAdminsSet = _context.AreCollegeAdminsSet();
                    User = user;
                    foreach (var request in FacilityRequest) {
                        IsApproved.Add(_context.IsApproved(request));
                    }
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
