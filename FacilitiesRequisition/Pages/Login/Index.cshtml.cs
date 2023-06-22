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
using Microsoft.IdentityModel.Tokens;

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
    
    
    public string ErrorUsernamePassword { get; set; }
    
    public string UsernameBlankError { get; set; }
    public string PasswordBlankError { get; set; }

    public void OnGet() {
        HttpContext.Session.Logout();
    }
    
    public IActionResult OnPost() {
        var isError = false;

        if (Username.IsNullOrEmpty()) {
            UsernameBlankError = $"Username required";
            isError = true;
        }

        if (Password.IsNullOrEmpty()) {
            PasswordBlankError = $"Password required";
            isError = true;
        }
        
        if (isError) {
            return Page();
            
        }
        
        var user = _databaseContext.GetUsers().FirstOrDefault(x => x.Username == Username);
        var passwordHash = Password.ComputeHash(Convert.FromBase64String(user?.PasswordSalt ?? ""));
        
        if (user == null || user.PasswordHash != passwordHash) {
            ErrorUsernamePassword = $"Incorrect username or password";
            isError = true;
        }
        
        if (isError) {
            return Page();
            
        } else {
            HttpContext.Session.Login(user);
        
            return RedirectToPage("../Dashboard/Index");   
        }
    }
}