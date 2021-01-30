using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace MyAddresses.Webapi.Extensions
{
    public class ValidationProblemDetails: ProblemDetails
    {
        public IEnumerable<string> Errors { get; set; }
    }
}