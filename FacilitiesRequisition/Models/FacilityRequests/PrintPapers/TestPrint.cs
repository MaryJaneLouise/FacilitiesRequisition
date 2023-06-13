using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using SelectPdf;
using System.Net.Mime;
using System.Collections.Generic;
using System.Linq;
using FacilitiesRequisition.Models.Officers;

namespace FacilitiesRequisition.Models.FacilityRequests.PrintPapers; 

public class TestPrint {
    public string DateFiled { get; set; }
    public Organization Requester { get; set; }
    
    public string NameActivity { get; set; }
    
    public string ContactNumber { get; set; }
    
    [DataType(DataType.Date)]
    public DateTime? StartDateRequested { get; set; }
    
    [DataType(DataType.Date)]
    public DateTime? EndDateRequested { get; set; }
    
    [EnumDataType(typeof(Venues))]
    public Venues VenueRequested { get; set; }
}