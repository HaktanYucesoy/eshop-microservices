

namespace BuildingBlocks.Exceptions
{
    public class InternalServerException:Exception
    {
        public InternalServerException(string Message) : base(Message) { }

        public InternalServerException(string Message,string? details):base(Message)
        {
            Details= details;
        }

        public string? Details { get; set; }

    }
}
