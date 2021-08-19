using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace NTBrokers.Services
{
    public class BrokerService
    {
        private readonly SqlConnection _connection;

        public BrokerService(SqlConnection connection)
        {
            _connection = connection;
        }
        public void AddBroker(BrokerService broker)
        {
            _connection.Open();

            using var command = new SqlCommand($"INSERT INTO dbo.AthleteModel (Name, Surname, Country_id)" +
                                               $"VALUES ('{athlete.Name}', '{athlete.Surname}', '{athlete.Country_id}'); SELECT CAST(SCOPE_IDENTITY() AS INT)", _connection);
            command.ExecuteNonQuery();

            _connection.Close();

            int athleteId = GetLastAthleteId();
            if (athleteId == 0) return;

            AddAthleteSportJunctions(athlete, athleteId);
        }
    }
}
