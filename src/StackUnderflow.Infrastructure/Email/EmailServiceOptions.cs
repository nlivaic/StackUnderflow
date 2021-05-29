namespace StackUnderflow.Infrastructure.Email
{
    public class EmailServiceOptions
    {
        public string DoNotReplyEmail { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
