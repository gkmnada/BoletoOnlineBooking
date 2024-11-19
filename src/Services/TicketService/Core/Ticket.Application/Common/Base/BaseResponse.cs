namespace Ticket.Application.Common.Base
{
    public class BaseResponse
    {
        public object Data { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public List<string> Errors { get; set; }
    }
}
