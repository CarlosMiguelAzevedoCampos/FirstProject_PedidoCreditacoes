using CMA.ISMAI.Core.Events.Store.Interface;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using System;
using System.Text;

namespace CMA.ISMAI.Core.Events.Store.Service
{
    public class StoreEvent : BaseConfiguration, IEventStore { 

        private IEventStoreConnection _connection;
        
        public void SaveToEventStore(Event @event)
        {
            BuildConnection();
            _connection.AppendToStreamAsync(
                     @event.GetType().FullName,
                    ExpectedVersion.Any,
                    new EventData(
                        Guid.NewGuid(),
                        @event.GetType().FullName,
                        false,
                        Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event)),
                        new byte[] { }
                    )
                ).Wait();
        }
         
        private void BuildConnection()
        {
            if (_connection != null)
                return;
            _connection = EventStoreConnection.Create(new Uri(ReturnSettingsValue("EventStore", "IPAddress")));
            _connection.ConnectAsync().Wait();
        }
    }
}
