using Domain.Constants;

namespace Application.Handlers
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMsgForStatusCode(statusCode);
        }

        private string GetDefaultMsgForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => AppMessages.BAD_REQUEST,
                401 => AppMessages.UNAUTHORIZED,
                404 => AppMessages.NOT_FOUNT,
                500 => AppMessages.INTERNAL_SERVER,
                201 => AppMessages.INSERTED,
                _ => null
            };
        }
    }
}