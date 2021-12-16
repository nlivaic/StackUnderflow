using System.Collections.Generic;

namespace StackUnderflow.Infrastructure.HealthCheck
{
    internal class HealthCheckModel
    {
        private readonly List<HealthCheckResultModel> _results = new ();

        public string Status { get; set; }
        public IEnumerable<HealthCheckResultModel> Results => _results;

        public void AddResult(HealthCheckResultModel result) => _results.Add(result);
    }
}
