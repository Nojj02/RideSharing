using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.OData;
using Newtonsoft.Json;
using Npgsql;
using NpgsqlTypes;
using RideSharing.RideApi.Model;

namespace RideSharing.RideApi.DataAccess
{
    public class RideRepository
    {
        private const string ConnectionString = "Host=localhost;Username=postgres;Password=thepassword;Database=postgres;Search Path=ridesharing";

        public async Task Save(Ride entity, DateTimeOffset timeStamp)
        {
            using (var sqlConnection = new NpgsqlConnection(ConnectionString))
            {
                sqlConnection.Open();

                using (var transaction = sqlConnection.BeginTransaction())
                {
                    try
                    {
                        foreach (var anEvent in entity.NewEvents)
                        {
                            var command = "INSERT INTO ridesharing.ride (id, version, event_type, event, timestamp) VALUES(@id, @version, @event_type, @event, @timestamp)";
                            using (var sqlCommand = new NpgsqlCommand(command, sqlConnection))
                            {
                                sqlCommand.Parameters.AddWithValue("id", NpgsqlDbType.Uuid,
                                    entity.Id);

                                sqlCommand.Parameters.AddWithValue("version", NpgsqlDbType.Integer,
                                    anEvent.Version);

                                sqlCommand.Parameters.AddWithValue("event_type", NpgsqlDbType.Varchar,
                                    anEvent.GetType());

                                sqlCommand.Parameters.AddWithValue("event", NpgsqlDbType.Jsonb,
                                    JsonConvert.SerializeObject(anEvent));

                                sqlCommand.Parameters.AddWithValue("timestamp", NpgsqlDbType.TimestampTZ,
                                    timeStamp);

                                sqlCommand.ExecuteNonQuery();
                            }
                        }
                        
                        await transaction.CommitAsync();
                    }
                    
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
        }

        public Ride Get(Guid id)
        {
            var events = new List<RideEvent>();
            using (var sqlConnection = new NpgsqlConnection(ConnectionString))
            {
                sqlConnection.Open();

                var command = "SELECT * FROM ridesharing.ride WHERE id = @id";
                using (var sqlCommand = new Npgsql.NpgsqlCommand(command, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("id", NpgsqlDbType.Uuid, id);
                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var anEvent = 
                                (RideEvent)JsonConvert.DeserializeObject(
                                    Convert.ToString(reader["event"]), 
                                    RideEvents.EventTypeLookup.Single(x => x.Value ==Convert.ToString(reader["event_type"])).Key);
                            events.Add(anEvent);
                        }
                    }
                }
            }

            return events.Any() ? new Ride(id, events) : null;
        }

        public async Task Update(Ride entity, DateTimeOffset timeStamp)
        {
            await Save(entity, timeStamp);
        }
    }
    public static class RideEvents
    {
        public static readonly IReadOnlyDictionary<Type, string> EventTypeLookup =
            new Dictionary<Type, string>
            {
                { typeof(RideRequestedEvent), "RideSharing.RideApi.Model.RideRequestedEvent" },
                { typeof(RideAcceptedEvent), "RideSharing.RideApi.Model.RideAcceptedEvent" }
            };
    }
}