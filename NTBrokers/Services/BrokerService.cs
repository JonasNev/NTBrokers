using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using NTBrokers.Models;

namespace NTBrokers.Services
{
    public class BrokerService
    {
        private readonly SqlConnection _connection;

        public BrokerService(SqlConnection connection)
        {
            _connection = connection;
        }
        public void AddBroker(BrokerModel broker)
        {
            _connection.Open();

            using var command = new SqlCommand($"INSERT INTO dbo.Brokers(Name,Surname) VALUES('{broker.Name}', '{broker.Surname}')", _connection);
            command.ExecuteNonQuery();

            _connection.Close();
        }

        public List<BrokerModel> GetBrokers()
        {
            List<BrokerModel> brokers = new();

            _connection.Open();

            using var command = new SqlCommand("SELECT * FROM dbo.Brokers;", _connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                BrokerModel broker = new()
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Surname = reader.GetString(2)
                };

                brokers.Add(broker);
            }

            _connection.Close();

            return brokers;
        }

    }
}
