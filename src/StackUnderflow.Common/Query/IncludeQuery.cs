using System.Collections.Generic;
using System.Linq;

namespace StackUnderflow.Common.Query
{
    public class IncludeQuery<TEntity, TPreviousProperty> : IIncludeQuery<TEntity, TPreviousProperty>
    {
        public Dictionary<IIncludeQuery, string> PathMap { get; } = new Dictionary<IIncludeQuery, string>();
        public IncludeVisitor Visitor { get; } = new IncludeVisitor();

        public IncludeQuery(Dictionary<IIncludeQuery, string> pathMap)
        {
            PathMap = pathMap;
        }

        // @nl public HashSet<string> Paths => PathMap.Select(x => x.Value).ToHashSet();
        public List<string> Paths => PathMap.Select(x => x.Value).ToList();
    }
}
