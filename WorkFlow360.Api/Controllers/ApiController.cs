using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkFlow360.Application.Common.Results;

namespace WorkFlow360.Api.Controllers
{
    [ApiController]
    public abstract class ApiController : ControllerBase
    {
        protected IActionResult Problem(Error error)
        {
            return error.Type switch
            {
                ErrorType.Validation => BadRequest(CreateProblemDetails( StatusCodes.Status400BadRequest, "Validation error", error)),

                ErrorType.NotFound => NotFound(CreateProblemDetails( StatusCodes.Status404NotFound, "Resource not found", error)),

                ErrorType.Conflict => Conflict(CreateProblemDetails( StatusCodes.Status409Conflict, "Conflict", error)),

                _ => StatusCode( StatusCodes.Status500InternalServerError, CreateProblemDetails( StatusCodes.Status500InternalServerError, "Server error", error))
            };
        }

        private static ProblemDetails CreateProblemDetails( int status, string title, Error error)
        {
            var problemDetails = new ProblemDetails
            {
                Status = status,
                Title = title,
                Detail = error.Message
            };

            problemDetails.Extensions["errorCode"] = error.Code;

            return problemDetails;
        }
    }
}
