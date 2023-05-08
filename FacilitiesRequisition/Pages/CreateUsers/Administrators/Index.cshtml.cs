using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using FacilitiesRequisition.Data;
using FacilitiesRequisition.Models;
using FacilitiesRequisition.Models.Administrators;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;


namespace FacilitiesRequisition.Pages.CreateUsers.Administrators; 

public class IndexModel : PageModel {
    private readonly DatabaseContext _databaseContext;

    public IndexModel(DatabaseContext databaseContext) {
        _databaseContext = databaseContext;
    }
    
    [BindProperty]
    [Display(Name = "First name")]
    public string FirstName { get; set; }
    
    [BindProperty]
    [Display(Name = "Middle name")]
    public string? MiddleName { get; set; }
    
    [BindProperty]
    [Display(Name = "Last name")]
    public string LastName { get; set; }
    
    [BindProperty]
    [Display(Name = "Username")]
    public string Username { get; set; }
    
    [BindProperty]
    [Display(Name = "Password")]
    public string Password { get; set; }

    public async Task<IActionResult> OnPostAsync() {
        var isUsernameDuplicate = _databaseContext.GetUsers().Any(x => x.Username == Username);
        
        if (!ModelState.IsValid || isUsernameDuplicate) {
            return Page();
        }

        var passwordSalt = PasswordHash.GenerateSalt();
        var passwordHash = Password.ComputeHash(passwordSalt);
        
        var superAdmin = new Administrator() {
            FirstName = FirstName,
            MiddleName = MiddleName,
            LastName = LastName,
            Username = Username,
            PasswordHash = passwordHash,
            PasswordSalt = Convert.ToBase64String(passwordSalt)
        };
        
        _databaseContext.AddAdministrator(superAdmin);
        await _databaseContext.SaveChangesAsync();
        
        var superAdminRole = new AdministratorRole() {
            Administrator = superAdmin,
            Position = AdministratorPosition.SuperAdmin
        };
        
        _databaseContext.AddAdministratorRole(superAdminRole);
        await _databaseContext.SaveChangesAsync();

        if (superAdmin.PasswordHash == passwordHash) {
            var claims = new List<Claim> {
                new("UserId", superAdmin.Id.ToString())
            };
            
            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);
            
            var authProperties = new AuthenticationProperties {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(1440),
                IsPersistent = true,
                IssuedUtc = DateTimeOffset.UtcNow,
                //RedirectUri = <string>
                // The full path or absolute URI to be used as an http 
                // redirect response value.
            };
            
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            HttpContext.Session.SetInt32(SessionUser.UserIdKey, superAdmin.Id);

            return RedirectToPage("../Dashboard/Index");
        }
        return Page();
    }
}