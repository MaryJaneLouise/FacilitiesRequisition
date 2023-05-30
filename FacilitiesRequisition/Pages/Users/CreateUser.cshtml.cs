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


namespace FacilitiesRequisition.Pages.Users;

public class CreateModel : PageModel {
    private readonly DatabaseContext _databaseContext;

    public CreateModel(DatabaseContext databaseContext) {
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

    [BindProperty] 
    public string? UserType { get; set; }


    public IActionResult OnGet() {
        HasSuperAdmin = _databaseContext.HasSuperAdministrator();

        if (HasSuperAdmin) {
            var user = HttpContext.Session.GetLoggedInUser(_databaseContext);
            if (user == null) return RedirectToPage("../Login/Index");

            if (user.Type != Models.UserType.Administrator ||
                _databaseContext.GetAdministratorRoles(user).All(x => x.Position != AdministratorPosition.SuperAdmin))
                return RedirectToPage("../Dashboard/Index");
        }

        PageTitle = !HasSuperAdmin ? "Create super administrator" : "Create user";
        return Page();
    }

    public IActionResult OnPost() {
        var isUsernameDuplicate = _databaseContext.GetUsers().Any(x => x.Username == Username);
        HasSuperAdmin = _databaseContext.HasSuperAdministrator();
        
        if (!ModelState.IsValid || isUsernameDuplicate || Password != RepeatPassword) {
            return Page();
        }

        var passwordSalt = PasswordHash.GenerateSalt();
        var passwordHash = Password.ComputeHash(passwordSalt);

        var user = new User {
            Type = UserType == "admin" || !HasSuperAdmin ? Models.UserType.Administrator : UserType == "officer" ? Models.UserType.Officer : Models.UserType.Faculty,
            FirstName = FirstName,
            MiddleName = MiddleName,
            LastName = LastName,
            Username = Username,
            PasswordHash = passwordHash,
            PasswordSalt = Convert.ToBase64String(passwordSalt)
        };

        _databaseContext.AddUser(user);

        if (!HasSuperAdmin) {
            var superAdminRole = new AdministratorRole {
                Administrator = user,
                Position = AdministratorPosition.SuperAdmin
            };
            _databaseContext.AddAdministratorRole(superAdminRole);
        }

        _databaseContext.SaveChanges();

        var isNotLoggedIn = !HttpContext.Session.IsLoggedIn();
        if (isNotLoggedIn) {
            HttpContext.Session.Login(user);
        }

        return RedirectToPage("../Users/Index");
    }

    public void SetUserType(string userType) {
        UserType = userType;
    }

    public IActionResult OnPostBackToDashboard() {
        return RedirectToPage("../Dashboard/Index");
    }
}