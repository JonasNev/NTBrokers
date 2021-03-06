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

        public List<BrokerModel> SortBrokers(SortFilterModel sortFilter)
        {
            List<BrokerModel> brokers = new();

            string query = $@"SELECT * FROM dbo.Brokers
                              ORDER BY {sortFilter.sortBy}";
            using var command = new SqlCommand(query, _connection);
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

        public void UpdateBroker(BrokerModel model)
        {
            string command = $@"UPDATE dbo.Brokers
                                SET Name = '{model.Name}', Surname = '{model.Surname}'
                                WHERE Id = {model.Id}";
            _connection.Open();
            using var sqlCommand = new SqlCommand(command, _connection);
            sqlCommand.ExecuteNonQuery();
            _connection.Close();
        }

        public void DeleteBroker(int deleteid)
        {
            string command = $@"DELETE FROM Dbo.Brokers
                                WHERE Id = {deleteid}";
            DeleteJunction(deleteid);
            _connection.Open();
            using var sqlCommand = new SqlCommand(command, _connection);
            sqlCommand.ExecuteNonQuery();
            _connection.Close();
        }

        public void DeleteJunction(int deleteId)
        {
            string command = $"DELETE FROM dbo.CompaniesBrokers WHERE Broker_id = {deleteId};";

            _connection.Open();

            using var sqlCommand = new SqlCommand(command, _connection);
            sqlCommand.ExecuteNonQuery();

            _connection.Close();
        }
    }
}
