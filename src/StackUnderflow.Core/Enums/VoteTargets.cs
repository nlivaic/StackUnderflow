namespace StackUnderflow.Core.Enums
{
    public class VoteTargets
    {
        private VoteTargets(string name, VoteTargetEnum target)
        {
            Name = name;
            Target = target;
        }

        public static VoteTargets Answer => new ("Answer", VoteTargetEnum.Answer);
        public static VoteTargets Comment => new ("Comment", VoteTargetEnum.Comment);

        public static VoteTargets Question => new ("Question", VoteTargetEnum.Question);
        public string Name { get; private set; }
        public VoteTargetEnum Target { get; set; }
    }
}
