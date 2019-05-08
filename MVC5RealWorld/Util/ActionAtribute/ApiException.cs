using System;
using System.Collections.Generic;

namespace MVC5RealWorld.Util.ActionAtribute
{
    public class ApiException : Exception
    {
        public int StatusCode { get; set; }

        public List<string> Errors { get; set; }

        public ApiException(string message,
                            int statusCode = 500,
                            List<string> errors = null) :
            base(message)
        {
            StatusCode = statusCode;
            Errors = errors;
        }
        public ApiException(Exception ex, int statusCode = 500) : base(ex.Message)
        {
            StatusCode = statusCode;
        }
    }







}
