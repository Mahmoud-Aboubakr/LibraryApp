//using API.Extensions;
//using Application.Exceptions;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;
//using System;
//using System.Net;
//using System.Web.Http.ExceptionHandling;
//using System.Web.Http.Filters;
//using ExceptionContext = Microsoft.AspNetCore.Mvc.Filters.ExceptionContext;
//using NotImplementedException = Application.Exceptions.NotImplementedException;
//using UnauthorizedAccessException = Application.Exceptions.UnauthorizedAccessException;

//namespace API.Filters
//{
//    public class CustomExceptionFilterAttribute : System.Web.Http.Filters.ExceptionFilterAttribute
//    {
        //public bool AllowMultiple => throw new System.NotImplementedException();

        //public Task ExecuteExceptionFilterAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        //{
        //    throw new System.NotImplementedException();
        //}

        //public override void OnException(HttpActionExecutedContext context)
        //{
        //    var exceptionType = context.Exception.GetType();
        //    if (exceptionType == typeof(UnauthorizedAccessException))
        //    {
        //        context.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
        //    }
        //    else if (exceptionType == typeof(NotFoundException))
        //    {
        //        context.Response = new HttpResponseMessage(HttpStatusCode.NotFound);
        //    }
        //    else if (exceptionType == typeof(BadRequestException))
        //    {
        //        context.Response = new HttpResponseMessage(HttpStatusCode.BadRequest);
        //    }
        //    else if (exceptionType == typeof(NotImplementedException))
        //    {
        //        context.Response = new HttpResponseMessage(HttpStatusCode.NotImplemented);
        //    }
        //    else
        //    {
        //        context.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
        //    }
        //    base.OnException(context);
        //}

        //public override void OnException(HttpActionExecutedContext context)
        //{
        //    if (context.Exception is NotImplementedException)
        //    {
        //        context.Response = new HttpResponseMessage(HttpStatusCode.NotImplemented);
        //    }
        //}
//    }
//}
