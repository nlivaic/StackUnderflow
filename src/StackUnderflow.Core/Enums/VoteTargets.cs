using StackUnderflow.Core.Models;

namespace StackUnderflow.Core.Enums
{
    public class VoteTargets
    {
        public string Name { get; private set; }
        public VoteTargetEnum Target { get; set; }

        private VoteTargets(string name, VoteTargetEnum target)
        {
            Name = name;
            Target = target;
        }

        public static VoteTargets Question => new VoteTargets("Question", VoteTargetEnum.Question);
        public static VoteTargets Answer => new VoteTargets("Answer", VoteTargetEnum.Answer);
        public static VoteTargets Comment => new VoteTargets("Comment", VoteTargetEnum.Comment);
    }
}
