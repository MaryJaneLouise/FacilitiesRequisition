using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FacilitiesRequisition.Data;
using FacilitiesRequisition.Models;
using FacilitiesRequisition.Models.Officers;
using FacilitiesRequisition.Models.Faculties;

namespace FacilitiesRequisition.Pages.FacultyRoles {
    public class CreateModel : PageModel {
        private readonly DatabaseContext _context;

        public CreateModel(DatabaseContext context) {
            _context = context;
        }
        
        public User Faculty { get; set; }
        public IEnumerable<SelectListItem> Organizations { get; set; } = default!;
        
        [BindProperty]
        public string OrganizationId { get; set; }
        
        [BindProperty]
        public OrganizationPosition Position { get; set; }


        public IActionResult OnGet(int? id) {
            var user = _context.GetUser(id ?? -1);
            if (user == null) {
                return NotFound();
            }

            Faculty = user;
            Organizations = _context.GetOrganizations().Select(organization =>
                new SelectListItem
                {
                    Value = organization.Id.ToString(),
                    Text = organization.Name
                });
            return Page();
        }
        
        public IActionResult OnPost(int? id) {
            var user = _context.GetUser(id ?? -1);
            if (user == null) {
                return NotFound();
            }

            Faculty = user;
            
            if (!ModelState.IsValid) {
                return Page();
            }

            var facultyRole = new FacultyRole() {
                Faculty = Faculty,
                Organization = _context.GetOrganization(Convert.ToInt32(OrganizationId))!,
                Position = Position
            };
            _context.AddFacultyRole(facultyRole);

            return RedirectToPage("./Index", new {id = Faculty.Id}); 
        }

        public IActionResult OnPostBackToIndex() {
            return RedirectToPage("./Index", new { id = Faculty.Id });
        }
    }
}
