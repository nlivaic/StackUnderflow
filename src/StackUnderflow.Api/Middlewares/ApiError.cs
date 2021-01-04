namespace StackUnderflow.Api.Middlewares
{
    public class ApiError
    {
        public string Id { get; set; }
        public short Status { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
    }
}
