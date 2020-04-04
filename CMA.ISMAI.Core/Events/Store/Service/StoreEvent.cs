using CMA.ISMAI.Core.Events.Store.Interface;
using EventStore.ClientAPI;
using System;
using System.Net;
using System.Text;

namespace CMA.ISMAI.Core.Events.Store.Service
{
    public class StoreEvent : IEventStore { 

        private readonly IEventStoreConnection _connection;
        public StoreEvent()
        {
            var settings = ConnectionSettings.Create().KeepReconnecting();
            _connection = EventStoreConnection.Create(settings,
                new IPEndPoint(
                    IPAddress.Parse("127.0.0.1"),
                    2113));
            _connection.ConnectAsync().Wait(); 
        }

        public void SaveToEventStore(Event @event)
        {
            _connection.AppendToStreamAsync(
                     @event.GetType().FullName,
                    ExpectedVersion.Any,
                    new EventData(
                        Guid.NewGuid(),
                        @event.GetType().FullName,
                        false,
                        Encoding.UTF8.GetBytes(@event.ToString()),
                        new byte[] { }
                    )
                ).Wait();
        }
    }
}
