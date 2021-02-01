using System;
using System.Net;

namespace MyAddresses.Domain.Exceptions
{
    public class AppValidationException: Exception
    {
        public int StatusCode { get; set; }
        public string Detail { get; set; }

        public AppValidationException(HttpStatusCode statusCode, string message)
            :base(message)
        {
            StatusCode = (int)statusCode;
        }

        public AppValidationException(HttpStatusCode statusCode, string message, string detail)
            :base(message)
        {
            StatusCode = (int)statusCode;
            Detail = detail;
        }
    }
}