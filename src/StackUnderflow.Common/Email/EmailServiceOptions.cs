namespace StackUnderflow.Common.Email
{
    public class EmailServiceOptions
    {
        public string DoNotReplyEmail { get; set; } // _configuration["EmailSettings:DoNotReply"]
        public string Host { get; set; }       // _configuration["EmailSettings:Host"]
        public int Port { get; set; }   // _configuration["EmailSettings:Port"]
        public string Username { get; set; }    // _configuration["EmailSettings:Username"]
        public string Password { get; set; }    // _configuration["EmailSettings:Password"]
    }
}
