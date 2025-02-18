using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace MVCDHProject.Controllers
{
    public class ErrorController : Controller
    {
        [Route("ClientError/{Statuscode}")]
        public IActionResult ClientErrorHandle(int Statuscode)
        {
            switch (Statuscode)
            {
                case 400:
                    ViewBag.ErrorTitle = "Bad Request";
                    ViewBag.ErrorMessage = "The server can’t return a response due to an error on the client’s end.";
                    break;
                case 402:
                    ViewBag.ErrorTitle = "Payment Required";
                    ViewBag.ErrorMessage = "Processing the request is not possible due to lack of required funds.";
                    break;
                case 403:
                    ViewBag.ErrorTitle = "Forbidden";
                    ViewBag.ErrorMessage = "You are attempting to access the resource that you don’t have permission to view.";
                    break;
                case 404:
                    ViewBag.ErrorTitle = "Not Found";
                    ViewBag.ErrorMessage = "The requested resource does not exist, and server does not know if it ever existed.";
                    break;
                case 405:
                    ViewBag.ErrorTitle = "Method Not Allowed";
                    ViewBag.ErrorMessage = "Hosting server supports the method received, but the target resource doesn’t.";
                    break;
                default:
                    ViewBag.ErrorTitle = "Client Error Occured";
                    ViewBag.ErrorMessage = "There is a Client-Error in the page, re-check the input you supplied.";
                    break;
            }
            return View("ClientErrorView");
        }


        [Route("ServerError")]
        public IActionResult ServerErrorHandle()
        {
            var ExceptionDetail = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            ViewBag.ErrorTitle = ExceptionDetail.Error.GetType().Name;
            ViewBag.path = ExceptionDetail.Path;
            ViewBag.Details = ExceptionDetail.Error.Message;
            return View("ServerErrorView");
        }
    }
}
