using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FacilitiesRequisition.Data;
using FacilitiesRequisition.Helpers;
using FacilitiesRequisition.Models;
using FacilitiesRequisition.Models.Users;
using FacilitiesRequisition.Models.FacilityRequests;
using FacilitiesRequisition.Models.FacilityRequests.PrintPapers;
using SelectPdf;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FacilitiesRequisition.Pages.PrintRequest; 

public class IndexModel : PageModel {
    private readonly DatabaseContext _context;
    private readonly IServiceProvider _serviceProvider;
    private readonly IViewRenderService _viewRenderService;

    public IndexModel(DatabaseContext context, IServiceProvider serviceProvider, IViewRenderService viewRenderService) {
        _context = context;
        _serviceProvider = serviceProvider;
        _viewRenderService = viewRenderService;
    }
    
    public User User { get; set; } = default!;
    public FacilityRequest FacilityRequest { get; set; } = default!; 
        
    public Signatures Signatories { get; set; } = default!;
        
    public string Status { get; set; }
    
    public TestPrint DateFiled { get; set; }
    public TestPrint Requester { get; set; }
    public TestPrint NameActivity { get; set; }
    public TestPrint ContactNumber { get; set; }
    public TestPrint StartDateRequested { get; set; }
    public TestPrint EndDateRequested { get; set; }
    public TestPrint VenueRequested { get; set; }

    public void OnGet(int? id) {
        var user = HttpContext.Session.GetLoggedInUser(_context);
        var facilityrequest = _context.GetFacilityRequest(id ?? -1);

        if (facilityrequest == null || user == null) {
            return ;
        }

        User = user;    
        FacilityRequest = facilityrequest;
        Signatories = _context.GetSignatures(facilityrequest);
        
        
    }

    public async Task<IActionResult> OnPostDownloadFile(int? requestId) {
        var facilityrequest = _context.GetFacilityRequest(requestId ?? -1);
        string _HTML = _viewRenderService.RenderToString("Pages/PrintRequest/_pdfView.cshtml", facilityrequest.Id).Result;


        PdfPageSize pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize),
            "A4", true);

        PdfPageOrientation pdfOrientation =
            (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation),
                "Portrait", true);

        int webPageWidth = 1024;
        int webPageHeight = 1024;

        // instantiate a html to pdf converter object
        HtmlToPdf converter = new HtmlToPdf();

        // set converter options
        converter.Options.PdfPageSize = pageSize;
        converter.Options.PdfPageOrientation = pdfOrientation;
        converter.Options.WebPageWidth = webPageWidth;
        converter.Options.WebPageHeight = webPageHeight;

        // create a new pdf document converting an url
        PdfDocument doc = converter.ConvertHtmlString(_HTML, "https://localhost:43368/");

        // save pdf document
        byte[] pdf = doc.Save();

        // close pdf document
        doc.Close();

        FileResult fileResult = new FileContentResult(pdf, "application/pdf");
        fileResult.FileDownloadName = "Document.pdf";

        return fileResult;
    }
}