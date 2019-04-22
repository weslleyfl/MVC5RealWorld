using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC5RealWorld.Util.ActionAtribute
{
    public class LogTimeException : ActionFilterAttribute, IExceptionFilter
    {
        
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controllerActionDescriptor = filterContext.ActionDescriptor as ControllerActionDescriptor;
            string message =  $" {controllerActionDescriptor?.ControllerName ?? string.Empty } " +
                              $" &  { controllerActionDescriptor?.ActionName ?? string.Empty } " +
                              $" = OnActionExecuting { DateTime.Now.ToString("F") + Environment.NewLine} ";

            // Log Complete Detail In a File  
            Log(message);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var controllerActionDescriptor = filterContext.ActionDescriptor as ControllerActionDescriptor;
            string message = controllerActionDescriptor?.ControllerName ?? string.Empty +
                             " & " + controllerActionDescriptor?.ActionName ?? string.Empty +
                             @" = OnActionExecuted     " + DateTime.Now.ToString("F") + Environment.NewLine;

            // Log Complete Detail In a File  
            Log(message);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            string message = filterContext.RouteData.Values["controller"] +
                             " & " + filterContext.RouteData.Values["action"] +
                             @" = OnResultExecuting     " + DateTime.Now.ToString("F") + Environment.NewLine;
            Log(message);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            string message = filterContext.RouteData.Values["controller"] +
                             " & " + filterContext.RouteData.Values["action"] +
                             @" = OnResultExecuted      " + DateTime.Now.ToString("F") + Environment.NewLine;
            Log(message);
        }

        public void OnException(ExceptionContext filterContext)
        {
            string message = filterContext.RouteData.Values["controller"] +
                             " & " + filterContext.RouteData.Values["action"] +
                             " & " + filterContext.Exception.Message + @"     " +
                             @"= OnResultExecuted     " + DateTime.Now.ToString("F") + Environment.NewLine;
            Log(message);
        }

        private void Log(string message)
        {
            // Here we're logging the detail in Text File  
            File.AppendAllText(@"c:\Temp\Data.txt", message, encoding: Encoding.UTF8);
        }
    }
}
