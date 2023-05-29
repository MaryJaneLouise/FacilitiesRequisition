using FacilitiesRequisition.Models;
using FacilitiesRequisition.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FacilitiesRequisition.Pages.ManageUsers; 

public class IndexModel : PageModel {
    private readonly DatabaseContext _context;

    public IndexModel(DatabaseContext databaseContext) {
        _context = databaseContext;
    }

    public IEnumerable<User> Users { get; set; } = new List<User>();
    [BindProperty(SupportsGet = true)]
    public string SearchQuery { get; set; }

    public void OnGet() {
        Users = _context.GetUsers();

        if (!string.IsNullOrEmpty(SearchQuery)) {
            Users = Users.Where(u => u.FirstName.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) 
                                     || u.LastName.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }

    public IActionResult OnPostBackToDashboard() {
        return RedirectToPage("../Dashboard/Index");
    }

    public IActionResult OnPostCreateUser() {
        return RedirectToPage("../CreateUsers/Index");
    }

}