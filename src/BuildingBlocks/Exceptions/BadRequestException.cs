
namespace BuildingBlocks.Exceptions
{
    public class BadRequestException:Exception
    {
        public BadRequestException(string Message) : base(Message) { }

        public BadRequestException(string Message, string? details) : base(Message)
        {
            Details = details;
        }

        public string? Details { get; set; }
    }
}
