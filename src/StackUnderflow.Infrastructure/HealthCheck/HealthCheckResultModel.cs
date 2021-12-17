using System.Collections.Generic;

namespace StackUnderflow.Infrastructure.HealthCheck
{
    internal class HealthCheckResultModel
    {
        public string Key { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public IEnumerable<string> Data { get; set; }
    }
}
