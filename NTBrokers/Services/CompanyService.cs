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

            int lastCompanyId = GetLastId();
            if (lastCompanyId == 0)
            {
                return;
            }
            AddJunction(model,lastCompanyId);
        }

        public void DeleteCompany(int deleteid)
        {
            string command = $@"DELETE FROM Dbo.Companies
                                WHERE Id = {deleteid}";
            DeleteJunction(deleteid);
            _connection.Open();
            using var sqlCommand = new SqlCommand(command, _connection);
            sqlCommand.ExecuteNonQuery();
            _connection.Close();
        }


        public void AddJunction(RealEstateModel model, int lastcompanyId)
        {
            string query = "";
            foreach (var entry in model.BrokerIds)
            {
                query += $"({entry}, {lastcompanyId}), ";
            }

            query = query.Remove(query.Length - 2);

            string commandCP = @$"INSERT INTO dbo.CompaniesBrokers(Broker_id, Company_id)
                                                    VALUES {query}";
            _connection.Open();
            using var command = new SqlCommand(commandCP, _connection);
            command.ExecuteNonQuery();
            _connection.Close();
        }

        public void DeleteJunction(int deleteId)
        {
            string command = $"DELETE FROM dbo.CompaniesBrokers WHERE Company_id = {deleteId};";

            _connection.Open();

            using var sqlCommand = new SqlCommand(command, _connection);
            sqlCommand.ExecuteNonQuery();

            _connection.Close();
        }

        public void UpdateCompany(RealEstateModel model)
        {
            string generateAdress = $"{model.Companies[0].Street} {model.Companies[0].Number}, {model.Companies[0].City}";

            string command = $@"UPDATE dbo.Companies
                                SET Name = '{model.Companies[0].Name}', City = '{model.Companies[0].City}', Street = '{model.Companies[0].Street}', Address = '{generateAdress}', Number = '{model.Companies[0].Number}'
                                WHERE Id = {model.Companies[0].Id}";
            _connection.Open();

            using var sqlCommand = new SqlCommand(command, _connection);
            sqlCommand.ExecuteNonQuery();
            _connection.Close();
            DeleteJunction(model.Companies[0].Id);
            AddJunction(model, model.Companies[0].Id);
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
