using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using FacilitiesRequisition.Data;
using FacilitiesRequisition.Models;
using FacilitiesRequisition.Models.Administrators;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FacilitiesRequisition.Pages.Login; 

public class IndexModel : PageModel {
    private readonly DatabaseContext _databaseContext;

    public IndexModel(DatabaseContext databaseContext) {
        _databaseContext = databaseContext;
    }
    
    [BindProperty]
    [Display(Name = "Username")]
    public string Username { get; set; }
    
    [BindProperty]
    [Display(Name = "Password")]
    public string Password { get; set; }
    
    public IActionResult OnPostAsync() {
        if (!ModelState.IsValid) {
            return Page();
        }

        var user = _databaseContext.GetUsers().FirstOrDefault(x => x.Username == Username);
        var passwordHash = Password.ComputeHash(Convert.FromBase64String(user?.PasswordSalt ?? ""));

        if (user == null || user.PasswordHash != passwordHash) return Page();
        
        HttpContext.Session.Login(user);
        
        return RedirectToPage("../Dashboard/Index");
    }
}