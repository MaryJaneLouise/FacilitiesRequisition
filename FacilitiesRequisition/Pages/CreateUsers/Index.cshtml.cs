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


namespace FacilitiesRequisition.Pages.CreateUsers.Administrators;

public class IndexModel : PageModel {
    private readonly DatabaseContext _databaseContext;

    public IndexModel(DatabaseContext databaseContext) {
        _databaseContext = databaseContext;
    }

    public string PageTitle { get; set; }
    public bool HasSuperAdmin { get; set; }

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

    [BindProperty]
    [Display(Name = "Repeat password")]
    public string RepeatPassword { get; set; }

    [BindProperty] public string? UserType { get; set; }


    public IActionResult OnGetAsync() {
        HasSuperAdmin = _databaseContext.HasSuperAdministrator();

        if (HasSuperAdmin) {
            var user = HttpContext.Session.GetLoggedInUser(_databaseContext);
            if (user == null) return RedirectToPage("../Login/Index");

            if (user is not Administrator admin ||
                _databaseContext.GetAdministratorRoles(admin).All(x => x.Position != AdministratorPosition.SuperAdmin))
                return RedirectToPage("../Dashboard/Index");
        }

        PageTitle = !HasSuperAdmin ? "Create super admin" : "Create user";
        return Page();
    }

    public async Task<IActionResult> OnPostAsync() {
        var isUsernameDuplicate = _databaseContext.GetUsers().Any(x => x.Username == Username);
        HasSuperAdmin = _databaseContext.HasSuperAdministrator();
        
        if (!ModelState.IsValid || isUsernameDuplicate || Password != RepeatPassword) {
            return Page();
        }

        var passwordSalt = PasswordHash.GenerateSalt();
        var passwordHash = Password.ComputeHash(passwordSalt);

        var user = UserType == "admin" || !HasSuperAdmin
            ? new Administrator {
                FirstName = FirstName,
                MiddleName = MiddleName,
                LastName = LastName,
                Username = Username,
                PasswordHash = passwordHash,
                PasswordSalt = Convert.ToBase64String(passwordSalt)
            }
            : UserType == "faculty"
                ? new Faculty {
                    FirstName = FirstName,
                    MiddleName = MiddleName,
                    LastName = LastName,
                    Username = Username,
                    PasswordHash = passwordHash,
                    PasswordSalt = Convert.ToBase64String(passwordSalt)
                }
                : new Officer {
                    FirstName = FirstName,
                    MiddleName = MiddleName,
                    LastName = LastName,
                    Username = Username,
                    PasswordHash = passwordHash,
                    PasswordSalt = Convert.ToBase64String(passwordSalt)
                } as User;

        _databaseContext.AddUser(user);

        if (!HasSuperAdmin) {
            var superAdminRole = new AdministratorRole {
                Administrator = (user as Administrator)!,
                Position = AdministratorPosition.SuperAdmin
            };
            _databaseContext.AddAdministratorRole(superAdminRole);
        }

        await _databaseContext.SaveChangesAsync();

        var isNotLoggedIn = !HttpContext.Session.IsLoggedIn();
        if (isNotLoggedIn) {
            HttpContext.Session.Login(user);
        }

        return RedirectToPage("../Dashboard/Index");
    }

    public void SetUserType(string userType) {
        UserType = userType;
    }
}