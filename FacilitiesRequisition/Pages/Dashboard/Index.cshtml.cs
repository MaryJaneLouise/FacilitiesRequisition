using System.ComponentModel.DataAnnotations;
using FacilitiesRequisition.Data;
using FacilitiesRequisition.Models;
using FacilitiesRequisition.Models.Administrators;
using FacilitiesRequisition.Models.Faculties;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FacilitiesRequisition.Pages.Dashboard; 

public class IndexModel : PageModel {
    public void OnGet() {
        
    }
}

public class NavigationBarModel
{
    private readonly DatabaseContext _context;

    public NavigationBarModel(DatabaseContext context)
    {
        _context = context;
    }

    // Add your properties and methods here that require database access
}
