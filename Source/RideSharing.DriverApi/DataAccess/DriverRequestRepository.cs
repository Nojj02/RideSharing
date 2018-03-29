using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Npgsql;
using NpgsqlTypes;
using RideSharing.DriverApi.Model;

namespace RideSharing.DriverApi.DataAccess
{
    public class DriverRequestRepository
    {
        private const string ConnectionString =
            "Host=localhost;Username=postgres;Password=thepassword;Database=postgres;Search Path=ridesharing";

        public async Task Save(DriverRequest entity, DateTimeOffset timeStamp)
        {
            using (var sqlConnection = new NpgsqlConnection(ConnectionString))
            {
                sqlConnection.Open();

                using (var transaction = sqlConnection.BeginTransaction())
                {
                    try
                    {
                        var command =
                            "INSERT INTO ridesharing.driverrequest (id, event, timestamp) VALUES (@id, @event, @timestamp)";
                        using (var sqlCommand = new Npgsql.NpgsqlCommand(command, sqlConnection))
                        {
                            sqlCommand.Parameters.AddWithValue("id", NpgsqlDbType.Uuid,
                                entity.Id);
                            sqlCommand.Parameters.AddWithValue("event", NpgsqlDbType.Jsonb,
                                JsonConvert.SerializeObject(entity));
                            sqlCommand.Parameters.AddWithValue("timestamp", NpgsqlDbType.TimestampTz,
                                timeStamp);
                            sqlCommand.ExecuteNonQuery();
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
        
        public List<DriverRequest> Get()
        {
            var driverRequests = new List<DriverRequest>();
            using (var sqlConnection = new NpgsqlConnection(ConnectionString))
            {
                sqlConnection.Open();

                var command = "SELECT * FROM ridesharing.driverrequest ORDER BY timestamp";
                using (var sqlCommand = new NpgsqlCommand(command, sqlConnection))
                {
                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            driverRequests.Add(JsonConvert.DeserializeObject<DriverRequest>(Convert.ToString(reader["event"])));
                        }
                    }
                }
            }
            
            return driverRequests;
        }
    }
    
}