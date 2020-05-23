namespace AspNetBearerAuthentication.Auth
{
    public sealed class ErrorMessage
    {
        public ErrorMessage(string message, int errorCode)
        {
            Message = message;
            ErrorCode = errorCode;
        }

        public string Message { get; }

        public int ErrorCode { get; }
    }
}