using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;
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

namespace FacilitiesRequisition.Pages.CreateRequestPaper; 

public class IndexModel : PageModel {
    private readonly DatabaseContext _databaseContext;

    public IndexModel(DatabaseContext databaseContext) {
        _databaseContext = databaseContext;
    }
    
    public string PageTitle { get; set; }

    [BindProperty]
    [Display(Name = "Date Filed")]
    public string DateFiled { get; set; } = DateTime.Now.ToShortDateString();
    
    [BindProperty]
    [Display(Name = "Name of Requester or Organization")]
    public string NameRequester { get; set; }

    [BindProperty]
    [Display(Name = "Classification")]
    public string? Classification { get; set; }

    [BindProperty]
    [Display(Name = "Name of Activity")]
    public string NameActivity { get; set; }

    [BindProperty]
    [Display(Name = "Requested Dates")]
    public string RequestedDates { get; set; }

    [BindProperty]
    [Display(Name = "Total Number of Hours")]
    public string NumberOfHours { get; set; }

    [BindProperty]
    [Display(Name = "Venue Requested")]
    public string VenueRequested { get; set; }
    
    [BindProperty]
    [Display(Name = "Equipment Needed")]
    public string EquipmentNeeded { get; set; }
    
    public IActionResult OnGet() {
        PageTitle = "Create a Facility Request";
        return Page();
    }

    public IActionResult OnPost() {
        return RedirectToPage("../Dashboard/Index");
    }
}