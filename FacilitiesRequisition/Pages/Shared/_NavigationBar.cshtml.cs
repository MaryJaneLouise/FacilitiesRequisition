using System.ComponentModel.DataAnnotations;
using FacilitiesRequisition.Data;
using FacilitiesRequisition.Models;
using FacilitiesRequisition.Models.Administrators;
using FacilitiesRequisition.Models.Officers;
using FacilitiesRequisition.Models.Faculties;
using FacilitiesRequisition.Models.FacilityRequests;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;



namespace FacilitiesRequisition.Pages.Shared {
    public class _NavigationBarModel : PageModel  {
        private readonly DatabaseContext _context;
    
        public _NavigationBarModel(DatabaseContext context)  {
            _context = context;
        }
    
        [BindProperty]
        public string Name { get; set; }
    
        public string UserType { get; set; }
        public string IsSuperAdmin { get; set; }
        public string UserRole { get; set; }
        public string ActivePage { get; set; }
        public string UserInfo { get; set; }

        
        public IList<FacilityRequest> FacilityRequest { get;set; } = default!;
        public IList<Organization> Organizations { get;set; } = default!;
        public IList<User?> Presidents { get;set; } = default!;

        public IList<OfficerRole> OfficerRoles { get; set; } = default!;

        public IActionResult OnGet()  {
            var user = HttpContext.Session.GetLoggedInUser(_context);

            if (user == null) {
                return RedirectToPage("/Login/Index");
            }
            
            bool isSuperAdministrator = user.Type == Models.UserType.Administrator &&
                _context.GetAdministratorRoles(user).Any(x => x.Position == AdministratorPosition.SuperAdmin);
            var userType = isSuperAdministrator ? "Super Administrator" :
                user.Type == Models.UserType.Administrator ? "Administrator" :
                user.Type == Models.UserType.Faculty ? "Faculty" : "Organization Officer";
            
            var currentPage = PageContext.RouteData.Values["page"].ToString();
            var facilityRequestsJson = JsonConvert.SerializeObject(FacilityRequest);
            ViewData["FacilityRequestsJson"] = facilityRequestsJson;
            

            switch (user) {
                case null:
                    return RedirectToPage("../Login/Index");
                default:
                    Name = $"{user.FirstName} {user.LastName}";
                    UserType = $"{userType}";
                    UserInfo = $"{user.Id}";
                    switch (UserType) {
                        case "Super Administrator" :
                            ActivePage = currentPage;
                            FacilityRequest = _context.GetFacilityRequests();
                            
                            Organizations = _context.GetOrganizations();
                            break;
                        case "Administrator" :
                            ActivePage = currentPage;
                            FacilityRequest = _context.GetFacilityRequests();
                            Organizations = _context.GetOrganizations();
                            break;
                        default:
                            ActivePage = currentPage;
                            Organizations = _context.GetOfficerOrganizations(user).ToList();
                            FacilityRequest = _context.GetFacilityRequestsRequested(user).ToList();
                            break;
                    }
                    return Page();
            }
        }
        
        public static class EnumExtensions {
            public static string GetDisplayName(Enum value) {
                var displayAttribute = value.GetType()
                    .GetField(value.ToString())
                    .GetCustomAttributes(typeof(DisplayAttribute), false)
                    .FirstOrDefault() as DisplayAttribute;

                return displayAttribute?.GetName() ?? value.ToString();
            }
        }
        
        public IActionResult OnPostCreateAccount()  {
            return RedirectToPage("../Users/CreateUser");
        }
        
        public IActionResult OnPostCreateOrganization()  {
            return RedirectToPage("../Organizations/CreateOrganization");
        }

        public IActionResult OnPostCreateFacilityRequest() {
            return RedirectToPage("../RequestFacility/CreateRequest");
        }
    
        public IActionResult OnPostManageUserAccount() {
            return RedirectToPage("../Users/Index");
        }
        
        public IActionResult OnPostManageOrganization() {
            return RedirectToPage("../Organizations/Index");
        }

        public IActionResult OnPostManageFacilityRequest() {
            return RedirectToPage("../RequestFacility/Index");
        }
        
        public IActionResult OnPostHistoryFacilityRequest() {
            return RedirectToPage("../RequestFacility/History");
        }
    
        public IActionResult OnPostViewProfile() {
            return RedirectToPage("../ManageUsers/ViewDetails");
        }
        
        public IActionResult OnPostLogout() {
            HttpContext.Session.Logout();
            return RedirectToPage("../Login/Index");
        }
    
        public IActionResult OnPostRequestPaper() {
            return RedirectToPage("../CreateRequestPaper/Index");
        }

        public IActionResult OnPostAvailableFacility() {
            return RedirectToPage("../AvailableFacility/Index");
        }
    
        public IActionResult OnPostHome() {
            return RedirectToPage("../Dashboard/Index");
        }
    }
}

