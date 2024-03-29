﻿using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;


namespace MVC5RealWorld.Util.ActionAtribute
{
    public class AddHeaderAttribute : ResultFilterAttribute
    {
        private readonly string _name;
        private readonly string _value;

        public AddHeaderAttribute(string name, string value)
        {
            _name = name;
            _value = value;
        }

        
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            context.HttpContext.Response.Headers.Add(_name, new string[] { _value });

            //if (!context.ModelState.IsValid)
            //{
            //    context.Result = new Microsoft.AspNetCore.Mvc.BadRequestObjectResult(modelState: context.ModelState);
            //}

            base.OnResultExecuting(context);
        }
    }



}
