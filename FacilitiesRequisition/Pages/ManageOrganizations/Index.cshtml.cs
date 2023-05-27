using FacilitiesRequisition.Models;
using FacilitiesRequisition.Data;
using FacilitiesRequisition.Models.Officers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace FacilitiesRequisition.Pages.ManageOrganizations; 

public class IndexModel : PageModel {
    private readonly DatabaseContext _context;

    public IndexModel(DatabaseContext databaseContext) {
        _context = databaseContext;
    }

    public IEnumerable<Organization> Organizations { get; set; } = new List<Organization>();
    
    [BindProperty(SupportsGet = true)]
    public string SearchQuery { get; set; }
    
    public void OnGet() {
        Organizations = _context.GetOrganizations();
    }
}