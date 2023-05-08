using System.ComponentModel.DataAnnotations;
using FacilitiesRequisition.Data;
using FacilitiesRequisition.Models;
using FacilitiesRequisition.Models.Administrators;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;


namespace FacilitiesRequisition.Pages.Dashboard; 

[Authorize]
public class IndexModel : PageModel  {
    private readonly DatabaseContext _context;

    public IndexModel(DatabaseContext context)  {
        _context = context;
    }

    [BindProperty]
    public string Name { get; set; }

    public void OnGet()  {
        var userId = HttpContext.Session.GetInt32(SessionUser.UserIdKey);
        if (userId == null) return;
        var user = _context.GetUser((int)userId);
        if (user == null) return;
        Name = $"{user.FirstName} {user.LastName}";
    }
    
    public async Task<IActionResult> OnPostAsync()  {
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);
        
        HttpContext.Session.SetInt32(SessionUser.UserIdKey, -1);

        return RedirectToPage("../Index");
    }
}