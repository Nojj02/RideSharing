using System;
using System.Threading.Tasks;
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
                        var command = "INSERT INTO ridesharing.ride (id, version, event_type, event, timestamp) VALUES(@id, @version, @event_type, @event, @timestamp)";
                        using (var sqlCommand = new Npgsql.NpgsqlCommand(command, sqlConnection))
                        {
                            sqlCommand.Parameters.AddWithValue("id", NpgsqlDbType.Uuid, 
                                entity.Id);
                            
                            sqlCommand.Parameters.AddWithValue("version", NpgsqlDbType.Integer,
                                1);
                            
                            sqlCommand.Parameters.AddWithValue("event_type", NpgsqlDbType.Varchar,
                                "Ride Request");
                            
                            sqlCommand.Parameters.AddWithValue("event", NpgsqlDbType.Jsonb,
                                JsonConvert.SerializeObject(entity));
                            
                            sqlCommand.Parameters.AddWithValue("timestamp", NpgsqlDbType.TimestampTZ, 
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

        public Ride Get(Guid id)
        {
            using (var sqlConnection = new NpgsqlConnection(ConnectionString))
            {
                sqlConnection.Open();

                var command = "SELECT * FROM ridesharing.ride WHERE id = @id";
                using (var sqlCommand = new Npgsql.NpgsqlCommand(command, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("id", NpgsqlDbType.Uuid, id);
                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var entity = (Ride)JsonConvert.DeserializeObject(Convert.ToString(reader["event"]), typeof(Ride));
                            return entity;
                        }
                    }
                }
            }

            return null;
        }

        public async Task Update(Ride entity, DateTimeOffset timeStamp)
        {
            using (var sqlConnection = new NpgsqlConnection(ConnectionString))
            {
                sqlConnection.Open();

                using (var transaction = sqlConnection.BeginTransaction())
                {
                    try
                    {
                        var command = "UPDATE ridesharing.ride SET event = @event, timestamp = @timestamp WHERE id = @id";
                        using (var sqlCommand = new Npgsql.NpgsqlCommand(command, sqlConnection))
                        {
                            sqlCommand.Parameters.AddWithValue("id", NpgsqlDbType.Uuid, 
                                entity.Id);
                            
                            sqlCommand.Parameters.AddWithValue("event", NpgsqlDbType.Jsonb,
                                JsonConvert.SerializeObject(entity));
                            
                            sqlCommand.Parameters.AddWithValue("timestamp", NpgsqlDbType.TimestampTZ, 
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
    }
}