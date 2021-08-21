using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using NTBrokers.Models;

namespace NTBrokers.Services
{
    public class CompanyService
    {
        private readonly SqlConnection _connection;

        public CompanyService(SqlConnection connection)
        {
            _connection = connection;
        }
        public void AddCompany(RealEstateModel model)
        {
            _connection.Open();

            string generateAdress = $"{model.Companies[0].Street} {model.Companies[0].Number}, {model.Companies[0].City}";
            using var command = new SqlCommand(@$"INSERT INTO dbo.Companies(Name,City,Street,Address,Number)
                                                    VALUES('{model.Companies[0].Name}', '{model.Companies[0].City}', '{model.Companies[0].Street}','{generateAdress}','{model.Companies[0].Number}')", _connection);
            command.ExecuteNonQuery();

            _connection.Close();

            int athleteId = GetLastId();
            if (athleteId == 0) return;

            foreach (var entry in model.Companies)
            {
                
            }

            _connection.Open();
            using var commandCP = new SqlCommand(@$"INSERT INTO dbo.CompaniesBrokers(Broker_id, Company_id)
                                                    VALUES({ model.Brokers[0].Id}, { model.Companies[0].Id});", _connection);
            commandCP.ExecuteNonQuery();
            _connection.Close();
        }

        public List<CompanyModel> GetCompanies()
        {
            List<CompanyModel> companies = new();

            _connection.Open();

            using var command = new SqlCommand("SELECT * FROM dbo.Companies;", _connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                CompanyModel company = new()
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    City = reader.GetString(2),
                    Street = reader.GetString(3),
                    Address = reader.GetString(4),
                    Number = reader.GetString(5)
                };

                companies.Add(company);
            }

            _connection.Close();

            return companies;
        }

        public void AddCompanyBrokerJunction(CompanyModel model, int id)
        {
            
        }

        private int GetLastId()
        {
            int id = 0;

            _connection.Open();

            using var command = new SqlCommand($"SELECT TOP 1 Id FROM dbo.Companies ORDER BY Id DESC;", _connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                id = reader.GetInt32(0);
            }

            _connection.Close();

            return id;
        }
    }
}
