using FacilitiesRequisition.Models;
using FacilitiesRequisition.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FacilitiesRequisition.Pages;

public class IndexModel : PageModel {
    private readonly ILogger<IndexModel> _logger;
    private readonly DatabaseContext _context;

    public IndexModel(ILogger<IndexModel> logger, DatabaseContext context) {
        _logger = logger;
        _context = context;
    }

    public IActionResult OnGet() {
        if (!_context.HasSuperAdministrator()) {
            return RedirectToPage("./CreateUsers/Administrators/Index");
        }

        return RedirectToPage("./Login/Index");
    }
    
}