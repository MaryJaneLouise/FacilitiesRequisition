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

namespace FacilitiesRequisition.Pages.ManageUsers; 

public class EditDetailsModel : PageModel {
    private readonly DatabaseContext _databaseContext;

    public EditDetailsModel(DatabaseContext context) {
        _databaseContext = context;
    }
    
    public User SelectedUser { get; set; }
    public string PageTitle { get; set; }
    public bool HasSuperAdmin { get; set; }
    
    public string Type { get; set; }

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
    
    public IActionResult OnGet(int id) {
        SelectedUser = _databaseContext.GetUser(id);
        if (SelectedUser == null) {
            return NotFound();
        }
        PageTitle = "Edit user details";
        return Page();
    }

    public IActionResult OnPost() {
        var isUsernameDuplicate = _databaseContext.GetUsers().Any(x => x.Username == Username);
        
        if (isUsernameDuplicate || Password != RepeatPassword) {
            return Page();
        }

        var selectedUser = SelectedUser;
        
        selectedUser.Type = SelectedUser.Type;
        selectedUser.FirstName = SelectedUser.FirstName;
        selectedUser.MiddleName = SelectedUser.MiddleName;
        selectedUser.LastName = SelectedUser.LastName;
        selectedUser.Username = SelectedUser.Username;

        if (!string.IsNullOrEmpty(Password)) {
            var passwordSalt = PasswordHash.GenerateSalt();
            var passwordHash = Password.ComputeHash(passwordSalt);
            selectedUser.PasswordHash = passwordHash;
            selectedUser.PasswordSalt = passwordSalt.ToString();
        }

        _databaseContext.Update(selectedUser); // Update the selected user
        _databaseContext.SaveChanges();

        return RedirectToPage("../ManageUsers/Index");
    }
    
    public void SetUserType(string userType) {
        UserType = userType;
    }

    public IActionResult OnPostBackToDashboard() {
        return RedirectToPage("../ManageUsers/Index");
    }
}