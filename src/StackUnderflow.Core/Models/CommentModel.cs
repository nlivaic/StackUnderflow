namespace StackUnderflow.Core.Models
{
    public class CommentModel
    {
        public string Username { get; set; }
        public string Body { get; set; }
        public string CreatedOn { get; set; }
        public int VotesSum { get; set; }
    }
}
