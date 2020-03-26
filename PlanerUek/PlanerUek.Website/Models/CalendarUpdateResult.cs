namespace PlanerUek.Website.Models
{
    public class CalendarUpdateResult
    {
        public CalendarUpdateResult(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }
        public bool IsSuccess { get; }
    }
}