using CMA.ISMAI.Core.Events.Store.Interface;
using CMA.ISMAI.Logging.Interface;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using System;
using System.Text;

namespace CMA.ISMAI.Core.Events.Store.Service
{
    public class StoreEvent : IEventStore { 

        private IEventStoreConnection _connection = null;
        private readonly ILog _log;

        public StoreEvent(ILog log)
        {
            _log = log;
        }
        
        public void SaveToEventStore(Event @event)
        {
            try
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
            catch(Exception ex)
            {
                _log.Fatal(ex.ToString());
            }
        }
         
        private void BuildConnection()
        {
            if (_connection != null)
                return;
            _connection = EventStoreConnection.Create(new Uri(BaseConfiguration.ReturnSettingsValue("EventStore", "IPAddress")));
            _connection.ConnectAsync().Wait();
        }
    }
}
