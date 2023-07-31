using API.Extensions;
using Application.Exceptions;
using Domain.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace API.Filters
{
    public class ExceptionAsync : IAsyncExceptionFilter
    {
        public Task OnExceptionAsync(ExceptionContext context)
        {
            var exception = context.Exception;

            var error = new ErrorDetails()
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Message = $"{AppMessages.INTERNAL_SERVER}"
            };

            if (exception is BadRequestException)
            {
                error.StatusCode = (int)HttpStatusCode.BadRequest;
                error.Message = $"{AppMessages.BAD_REQUEST}: {exception.Message}";
            }
            else if (exception is Application.Exceptions.UnauthorizedAccessException)
            {
                error.StatusCode = (int)HttpStatusCode.Unauthorized;
                error.Message = $"{AppMessages.UNAUTHORIZED}: {exception.Message}";
            }
            else if (exception is NotFoundException)
            {
                error.StatusCode = (int)HttpStatusCode.NotFound;
                error.Message = $"{AppMessages.NOT_FOUNT}: {exception.Message}";
            }
            //Logs your technical exception with stack trace below
            context.Result = new JsonResult(error);
            return Task.CompletedTask;
        }
    }
}