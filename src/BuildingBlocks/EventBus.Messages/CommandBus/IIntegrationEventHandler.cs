using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventBus.Messages.Events.Core;

namespace EventBus.Messages.CommandBus
{
    public interface IIntegrationEventHandler<in T> where T : IntegrationEvent
    {
        Task Handle(T @event);
    }

    public interface IIntegrationEventHandler
    {
    }
}