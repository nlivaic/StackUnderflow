using StackUnderflow.Common.Base;

namespace StackUnderflow.Data.Models
{
    public class LimitsKeyValuePair : BaseEntity<int>
    {
        public string LimitKey { get; set; }
        public int LimitValue { get; set; }
    }
}
