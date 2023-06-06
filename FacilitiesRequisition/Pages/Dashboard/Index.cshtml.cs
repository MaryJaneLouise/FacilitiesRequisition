using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using FacilitiesRequisition.Data;
using FacilitiesRequisition.Models;
using FacilitiesRequisition.Models.Administrators;
using FacilitiesRequisition.Models.Faculties;
using FacilitiesRequisition.Models.FacilityRequests;
using FacilitiesRequisition.Models.Officers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FacilitiesRequisition.Pages.Dashboard; 

public class IndexModel : PageModel {
    private readonly DatabaseContext _context;

    public IndexModel(DatabaseContext context) {
        _context = context;
    }
        
    public IList<FacilityRequest> FacilityRequest { get;set; } = default!;
    public IList<Organization> Organizations { get;set; } = default!;

    public IList<OfficerRole> OfficerRolesOrganization { get; set; } = default!;
    
    public string OfficerRole { get; set; }

    public async Task OnGetAsync() {
        FacilityRequest = _context.GetFacilityRequests();
        Organizations = _context.GetOrganizations();
        
        var president = OfficerRolesOrganization.Select(role => role.Position == OrganizationPosition.President && role.Organization == Organizations);
        OfficerRole = president.ToString();
    }
}
