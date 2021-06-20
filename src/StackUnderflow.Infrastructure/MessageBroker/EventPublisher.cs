﻿using MassTransit;
using StackUnderflow.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StackUnderflow.Infrastructure.MessageBroker
{
    public class EventPublisher : IEventPublisher, IEventRegister, IRegisteredEventPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private IList<Func<IEventPublisher, Task>> _eventActionsToPublish;

        public EventPublisher(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
            _eventActionsToPublish = new List<Func<IEventPublisher, Task>>();
        }

        public async Task PublishEvent<T>(object eventToPublish) where T : class
        {
            await _publishEndpoint.Publish<T>(eventToPublish);
        }

        public void RegisterEvent<T>(object newEvent) where T : class
        {
            _eventActionsToPublish.Add(async eventPublisher => await eventPublisher.PublishEvent<T>(newEvent));
        }

        public async Task PublishAll()
        {
            foreach (var action in _eventActionsToPublish)
            {
                await action(this);
            }
        }
    }
}