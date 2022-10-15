using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventBus.Messages.CommandBus;
using EventBus.Messages.Events.Core;

namespace EventBus.Messages.RabbitMQ
{
    public interface IEventBusRabbitMQ
    {
        public interface IEventBusRabbitMQ
        {
            void Publish(IntegrationEvent @event);

            void Subscribe<T, TH>()
                where T : IntegrationEvent
                where TH : IIntegrationEventHandler<T>;

            void Unsubscribe<T, TH>()
                where TH : IIntegrationEventHandler<T>
                where T : IntegrationEvent;
        }
    }
}