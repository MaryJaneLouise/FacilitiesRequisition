

//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Abstractions;
//using Microsoft.AspNetCore.Mvc.Infrastructure;
//using Microsoft.AspNetCore.Mvc.ModelBinding;
//using Microsoft.AspNetCore.Mvc.Razor;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.AspNetCore.Mvc.ViewEngines;
//using Microsoft.AspNetCore.Mvc.ViewFeatures;
//using Microsoft.AspNetCore.Routing;
//using System.Diagnostics;
//using System.Text.Encodings.Web;

//public interface IViewRenderService
//{
//    Task<string> RenderToStringAsync(string viewName, object model);
//}

//public class ViewRenderService : IViewRenderService
//{
//    private readonly IRazorViewEngine _razorViewEngine;
//    private readonly ITempDataProvider _tempDataProvider;
//    private readonly IServiceProvider _serviceProvider;
//    private readonly IHttpContextAccessor _httpContext;
//    private readonly IActionContextAccessor _actionContext;
//    private readonly IRazorPageActivator _activator;


//    public ViewRenderService(IRazorViewEngine razorViewEngine,
//        ITempDataProvider tempDataProvider,
//        IServiceProvider serviceProvider,
//        IHttpContextAccessor httpContext,
//        IRazorPageActivator activator,
//        IActionContextAccessor actionContext)
//    {
//        _razorViewEngine = razorViewEngine;
//        _tempDataProvider = tempDataProvider;
//        _serviceProvider = serviceProvider;

//        _httpContext = httpContext;
//        _actionContext = actionContext;
//        _activator = activator;

//    }

//    public async Task<string> RenderToStringAsync(string viewName, object model)
//    {
//        var httpContext = new DefaultHttpContext { RequestServices = _serviceProvider };
//        var actionContext = new ActionContext(httpContext, new Microsoft.AspNetCore.Routing.RouteData(), new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor());

//        using (var sw = new StringWriter())
//        {
//            var viewResult = _razorViewEngine.FindPage(actionContext, viewName);

//            if (viewResult.Page == null)
//            {
//                throw new ArgumentNullException($"{viewName} does not match any available view");
//            }

//            var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
//            {
//                Model = model
//            };

//            var view = new RazorView(_razorViewEngine,
//                           _activator,
//                           new List<IRazorPage>(),
//                           viewResult.Page,
//                           HtmlEncoder.Default,
//                           new DiagnosticListener("ViewRenderService"));


//            var viewContext = new ViewContext(
//                   actionContext,
//                   view,
//                   viewDictionary,
//                   new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
//                   sw,
//                   new HtmlHelperOptions()
//               );


//            var page = ((Page)viewResult.Page);

//            page.PageContext = new Microsoft.AspNetCore.Mvc.RazorPages.PageContext
//            {
//                ViewData = viewContext.ViewData

//            };

//            page.ViewContext = viewContext;


//            _activator.Activate(page, viewContext);

//            await page.ExecuteAsync();


//            return sw.ToString();

//        }
//    }
//}
