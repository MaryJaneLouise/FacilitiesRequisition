using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using FacilitiesRequisition.Data;
using FacilitiesRequisition.Models;
using FacilitiesRequisition.Models.Administrators;
using FacilitiesRequisition.Models.Faculties;
using FacilitiesRequisition.Models.Officers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FacilitiesRequisition.Pages.CreateOrganizations; 

public class IndexModel : PageModel {
    private readonly DatabaseContext _databaseContext;

    public IndexModel(DatabaseContext databaseContext) {
        _databaseContext = databaseContext;
    }

    public string PageTitle { get; set; }
    
    [BindProperty]
    [Display(Name = "Name of the Organization")]
    public string Name { get; set; }
    
    [BindProperty]
    [Display(Name = "Organization's Adviser")]
    public string? Adviser { get; set; }
    
    [BindProperty]
    [Display(Name = "Organization's Description")]
    public string? Description { get; set; }

    public void OnGet() {
        
    }
}