namespace Talabat.APIs.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public ApiResponse(int statusCode, string? message = null)
        {
            StatusCode = statusCode;
            Message = message?? GetMessageFromCode(statusCode);
        }
        private string GetMessageFromCode(int statuscode)
        {
            return statuscode switch
            {
                400 => "Bad Request.",
                401 => "Autherized Error.",
                404 => "Resources not found.",
                500 => "Server Error.",
                _ => null
            };
        }

    }
}
