using System.Collections.Generic;
using System.Linq;
using GreenPipes;
using Microsoft.Extensions.DependencyInjection;

namespace StackUnderflow.Infrastructure.MessageBroker.Middlewares.ErrorLogging
{
    public class ExceptionLoggerSpecification<T> : IPipeSpecification<T>
        where T : class, PipeContext
    {
        private readonly IServiceCollection _serviceCollection;

        public ExceptionLoggerSpecification(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }

        public IEnumerable<ValidationResult> Validate() => Enumerable.Empty<ValidationResult>();

        public void Apply(IPipeBuilder<T> builder) => builder.AddFilter(new ExceptionLoggerFilter<T>(_serviceCollection));
    }
}
