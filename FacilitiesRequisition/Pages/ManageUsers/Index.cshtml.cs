using FacilitiesRequisition.Models;
using FacilitiesRequisition.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FacilitiesRequisition.Pages.ManageUsers; 

public class IndexModel : PageModel {
    private readonly DatabaseContext _context;

    public IndexModel(DatabaseContext databaseContext) {
        _context = databaseContext;
    }

    public IEnumerable<User> Users { get; set; }
    
    
    public void OnGet() {
        Users = _context.GetUsers();
    }
    
    public IActionResult OnPostBackToDashboard() {
        return RedirectToPage("../Dashboard/Index");
    }
}