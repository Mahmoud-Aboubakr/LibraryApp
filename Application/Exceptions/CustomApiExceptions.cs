using Application.Exceptions;
using Domain.Aggregates;
using Domain.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Application.Handlers
{
    public class CustomApiExceptions : IAsyncExceptionFilter
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
                error.Message = $"{AppMessages.BAD_REQUEST}: {exception.StackTrace}";
            }
            else if (exception is DivideByZeroException)
            {
                error.StatusCode = (int)HttpStatusCode.NotAcceptable;
                error.Message = $"{AppMessages.NOTACCEPTABLE}: {exception.StackTrace}";
            }
            else if (exception is NotFoundException)
            {
                error.StatusCode = (int)HttpStatusCode.NotFound;
                error.Message = $"{AppMessages.NOT_FOUNT}: {exception.StackTrace}";
            }
            //Logs your technical exception with stack trace below

            context.Result = new JsonResult(error);
            return Task.CompletedTask;
        }
    }
}