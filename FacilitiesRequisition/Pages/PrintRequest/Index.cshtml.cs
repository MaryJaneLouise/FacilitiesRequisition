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
using FacilitiesRequisition.Models;
using FacilitiesRequisition.Models.Users;
using FacilitiesRequisition.Models.FacilityRequests;
using SelectPdf;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FacilitiesRequisition.Pages.PrintRequest; 

public class IndexModel : PageModel {
    private readonly DatabaseContext _context;
    private readonly IServiceProvider _serviceProvider;

    public IndexModel(DatabaseContext context, IServiceProvider serviceProvider) {
        _context = context;
        _serviceProvider = serviceProvider;
    }
    
    public User User { get; set; } = default!;
    public FacilityRequest FacilityRequest { get; set; } = default!; 
        
    public Signatures Signatories { get; set; } = default!;
        
    public string Status { get; set; }
    
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

    /*[HttpPost]
    public async Task<IActionResult> Test() {
        var htmlHelper = GetHtmlHelper();
        //string _HTML = await RenderViewToStringAsync(htmlHelper, "_pdfView", sampleModel);

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
        PdfDocument doc = converter.ConvertHtmlString(_HTML, "https://localhost:43311/");

        // save pdf document
        byte[] pdf = doc.Save();

        // close pdf document
        doc.Close();

        FileResult fileResult = new FileContentResult(pdf, "application/pdf");
        fileResult.FileDownloadName = "Document.pdf";

        return fileResult;
    }
    
    private IHtmlHelper GetHtmlHelper() {
        var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary());
        var httpContext = new DefaultHttpContext { RequestServices = _serviceProvider };
        var actionContext = new ActionContext(httpContext, new Microsoft.AspNetCore.Routing.RouteData(), new PageActionDescriptor(), viewData);

        return new HtmlHelper(actionContext, new ViewDataContainer(viewData));
    }

    private static async Task<string> RenderViewToStringAsync(IHtmlHelper htmlHelper, string viewName, object model) {
        var viewData = new ViewDataDictionary(htmlHelper.ViewData);
        viewData.Model = model;

        using var sw = new StringWriter();
        var viewResult = htmlHelper.ViewEngine.GetView(htmlHelper.ActionContext, viewName, false);
        var viewContext = new ViewContext(htmlHelper.ActionContext, viewResult.View, viewData, htmlHelper.ViewContext.TempData, sw, new HtmlHelperOptions());

        await viewResult.View.RenderAsync(viewContext);

        return sw.ToString();
    }*/
}