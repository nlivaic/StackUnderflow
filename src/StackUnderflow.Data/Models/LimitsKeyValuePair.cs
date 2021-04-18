using StackUnderflow.Common.Base;

namespace StackUnderflow.Data.Models
{
    internal class LimitsKeyValuePair : BaseEntity<int>
    {
        public string LimitKey { get; set; }
        public int LimitValue { get; set; }
    }
}
