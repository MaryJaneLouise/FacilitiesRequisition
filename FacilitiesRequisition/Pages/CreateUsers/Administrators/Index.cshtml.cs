using System.ComponentModel.DataAnnotations;
using FacilitiesRequisition.Data;
using FacilitiesRequisition.Models;
using FacilitiesRequisition.Models.Administrators;
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
        if (!ModelState.IsValid) {
            return Page();
        }

        var passwordSalt = PasswordHash.GenerateSalt();
        var passwordHash = Password.ComputeHash(passwordSalt);
        
        var superAdmin = new Administrator()
        {
            FirstName = FirstName,
            MiddleName = MiddleName,
            LastName = LastName,
            Username = Username,
            PasswordHash = passwordHash,
            PasswordSalt = Convert.ToBase64String(passwordSalt)
        };
        
        _databaseContext.AddAdministrator(superAdmin);

        var superAdminRole = new AdministratorRole()
        {
            Administrator = superAdmin,
            Position = AdministratorPosition.SuperAdmin
        };
        
        _databaseContext.AddAdministratorRole(superAdminRole);
        
        await _databaseContext.SaveChangesAsync();
        
        return RedirectToPage("./Index");
    }
}