namespace PlanerUek.Website.Models
{
    public class CalendarUpdateResult
    {
        public bool IsSuccess { get; }
        public string ErrorMessage { get; }
        public string AuthorizationEndpoint { get; set; }
        
        public CalendarUpdateResult(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }

        public CalendarUpdateResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}