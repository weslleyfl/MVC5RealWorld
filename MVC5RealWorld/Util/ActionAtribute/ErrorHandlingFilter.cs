using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MVC5RealWorld.Util.ActionAtribute
{
    public class ErrorHandlingFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            //log your exception here

            Debug.WriteLine($" ERROR DO OnExceptionAsync -  { exception?.Message ?? "Erro no exception igual a NULL " }");

            context.ExceptionHandled = false; //optional 
            
        }
       
    }
}
