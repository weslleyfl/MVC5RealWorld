using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC5RealWorld.Util.ActionAtribute
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class MeuMiddleware
    {
        private readonly RequestDelegate _next;

        public MeuMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            await _next(httpContext);

        }


    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MeuMiddlewareExtensions
    {
        public static IApplicationBuilder UseMeuMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MeuMiddleware>();
        }
    }


    class MyClass : IApplicationBuilder
    {
        public IServiceProvider ApplicationServices { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IFeatureCollection ServerFeatures => throw new NotImplementedException();

        public IDictionary<string, object> Properties => throw new NotImplementedException();

        public RequestDelegate Build()
        {
            throw new NotImplementedException();
        }

        public IApplicationBuilder New()
        {
            throw new NotImplementedException();
        }

        public IApplicationBuilder Use(Func<RequestDelegate, RequestDelegate> middleware)
        {
            throw new NotImplementedException();
        }


    }



}
