using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Npgsql;
using RideSharing.RideApi.Model;

namespace RideSharing.RideApi.DataAccess
{
    public class RideEventRepository
    {
        private const string ConnectionString =
            "Host=localhost;Username=postgres;Password=thepassword;Database=postgres;Search Path=schooled";
        
        public IEnumerable<EventStoreItem> GetAll()
        {
            var entities = new List<EventStoreItem>();
            using (var sqlConnection = new NpgsqlConnection(ConnectionString))
            {
                sqlConnection.Open();

                var command = "SELECT db_id, id, event_type, event, timestamp FROM ridesharing.ride";
                using (var sqlCommand = new Npgsql.NpgsqlCommand(command, sqlConnection))
                {
                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var eventType = Convert.ToString(reader["event_type"]);
                            var entity = 
                                new EventStoreItem
                                {
                                    Id = Guid.Parse(Convert.ToString(reader["id"])),
                                    EventType = eventType,
                                    TimeStamp = DateTimeOffset.Parse(Convert.ToString(reader["timestamp"])),
                                    Event = 
                                        (RideEvent)JsonConvert.DeserializeObject(
                                            Convert.ToString(reader["event"]), 
                                            RideEvents.EventTypeLookup.Single(x=> x.Value == eventType).Key)
                                };
                            entities.Add(entity);
                        }
                    }
                }
            }
            
            return entities;
        }
    }
}