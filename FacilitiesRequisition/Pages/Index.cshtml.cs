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

    public async Task<IActionResult> OnGetAsync() {
        if (!_context.HasSuperAdministrator()) {
            return RedirectToPage("./CreateUsers/Administrators/Index");
        }
        
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);

        var userId = HttpContext.Session.GetInt32(SessionUser.UserIdKey);
        if (userId == null) return RedirectToPage("./Login/Index");
        var user = _context.GetUser((int)userId);
        if (user == null) return RedirectToPage("./Login/Index");
        return RedirectToPage("./Dashboard/Index");
    }
}