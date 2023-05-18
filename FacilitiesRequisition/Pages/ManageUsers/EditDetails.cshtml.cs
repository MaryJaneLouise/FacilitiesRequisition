using FacilitiesRequisition.Models;
using FacilitiesRequisition.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FacilitiesRequisition.Pages.ManageUsers; 

public class UserDetailsModel : PageModel {
    private readonly DatabaseContext _context;

    public UserDetailsModel(DatabaseContext context) {
        _context = context;
    }
    
    public User SelectedUser { get; set; }
    public void OnGet(int id) {
        SelectedUser = _context.GetUser(id);
    }
}