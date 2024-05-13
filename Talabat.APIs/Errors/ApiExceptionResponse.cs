namespace Talabat.APIs.Errors
{
    public class ApiExceptionResponse : ApiErrorResponse
    {
        public string? Details { get; set; }
        public ApiExceptionResponse(int statusCode, string? Message = null, string? details = null) : base(statusCode)
        {
            this.Details = details;
        }
    }
}
