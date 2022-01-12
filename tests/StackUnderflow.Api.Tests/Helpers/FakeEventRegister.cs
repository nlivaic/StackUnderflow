﻿using System;
using System.Threading.Tasks;
using SparkRoseDigital.Infrastructure.MessageBroker;

namespace StackUnderflow.Api.Tests.Helpers
{
    class FakeEventRegister : IEventRegister, IRegisteredEventPublisher
    {
        public Task PublishAll() => Task.CompletedTask;
        public void RegisterEvent<T>(object newEvent) where T : class
        {
        }
    }
}
