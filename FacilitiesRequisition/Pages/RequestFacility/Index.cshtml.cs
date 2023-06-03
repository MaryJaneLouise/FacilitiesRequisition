using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FacilitiesRequisition.Data;
using FacilitiesRequisition.Models.FacilityRequests;

namespace FacilitiesRequisition.Pages.RequestFacility
{
    public class IndexModel : PageModel {
        private readonly DatabaseContext _context;

        public IndexModel(DatabaseContext context) {
            _context = context;
        }
        
        public IList<FacilityRequest> FacilityRequest { get;set; } = default!;
        
        public string SortStartDate { get; set; }
        
        public async Task OnGetAsync(string sortDate) {
            //SortStartDate = sortDate == 
            FacilityRequest = _context.GetFacilityRequests();
        }
    }
}
