using FacilitiesRequisition.Models;
using FacilitiesRequisition.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FacilitiesRequisition.Pages;

public class IndexModel : PageModel {
    private readonly ILogger<IndexModel> _logger;
    private readonly DatabaseContext _context;

    public IndexModel(ILogger<IndexModel> logger, DatabaseContext context) {
        _logger = logger;
        _context = context;
    }

    public IActionResult OnGet() {
        return RedirectToPage(!_context.HasSuperAdministrator()
            ? "./Users/CreateUser"
            : HttpContext.Session.IsLoggedIn()
                ? "./Dashboard/Index"
                : "./Login/Index");
    }
}