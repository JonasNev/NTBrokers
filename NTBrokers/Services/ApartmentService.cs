using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using NTBrokers.Models;

namespace NTBrokers.Services
{
    public class ApartmentService
    {
        private SqlConnection _connection;
        private CompanyService _companyService;
        private BrokerService _brokerService;

        public ApartmentService(SqlConnection connection, CompanyService companyService, BrokerService brokerService)
        {
            _connection = connection;
            _companyService = companyService;
            _brokerService = brokerService;
        }

        public RealEstateModel AddApartment(RealEstateModel model)
        {
            _connection.Open();

            string generateAdress = $"{model.Apartments[0].Street} {model.Apartments[0].HouseNR} - {model.Apartments[0].FlatNr}, {model.Apartments[0].City}";
            using var command = new SqlCommand(@$"INSERT INTO dbo.Apartments(City,Street,Address,Floor,BuildingFloor,Company_id,Broker_id,HouseNR,FlatNr)
                                                    VALUES('{model.Apartments[0].City}', '{model.Apartments[0].Street}', '{generateAdress}','{model.Apartments[0].Floor}','{model.Apartments[0].BuildingFloors}','{model.Apartments[0].Company_id}','{0}','{model.Apartments[0].HouseNR}','{model.Apartments[0].FlatNr}')", _connection);
            command.ExecuteNonQuery();

            _connection.Close();

            return model;
        }

        public void RemoveApartmentBroker(int id)
        {
            _connection.Open();

            using var command = new SqlCommand(@$"UPDATE dbo.Apartments
                                                SET Broker_id = 0
                                                WHERE Id = {id} ", _connection);
            command.ExecuteNonQuery();

            _connection.Close();

        }

        public void RemoveBrokerFromAll(int id)
        {
            _connection.Open();

            using var command = new SqlCommand(@$"UPDATE dbo.Apartments
                                                SET Broker_id = 0
                                                WHERE Broker_id = {id} ", _connection);
            command.ExecuteNonQuery();

            _connection.Close();

        }

        public List<ApartmentModel> SortFilterApartments(SortFilterModel sortFilterModel)
        {
            List<ApartmentModel> model = new();

            string querystring = "";

            if (sortFilterModel.filterCity != null)
            {
                querystring += $"AND City = '{sortFilterModel.filterCity}' ";
            }
            if (sortFilterModel.filterCompany != null)
            {
                querystring += $"AND Company_id = '{sortFilterModel.filterCompany}' ";
            }
            if (sortFilterModel.filterBroker != null)
            {
                querystring += $"AND Broker_id = '{sortFilterModel.filterBroker}' ";
            }

            string query = $@"SELECT * FROM dbo.Apartments
                                WHERE '' = '' {querystring}";
            model = ParseModels(query);

            return model;
        }


        public List<ApartmentModel> GetApartments()
        {
            List<ApartmentModel> apartments = new();

            string query = "SELECT* FROM dbo.Apartments;";
            apartments = ParseModels(query);
            return apartments;
        }

        public void UpdateApartment(RealEstateModel model)
        {
            string generateAdress = $"{model.Apartments[0].Street} {model.Apartments[0].HouseNR}, {model.Apartments[0].FlatNr},{model.Apartments[0].City}";

            string command = $@"UPDATE dbo.Apartments
                                SET City = '{model.Apartments[0].City}', Street = '{model.Apartments[0].Street}', Address = '{generateAdress}', HouseNR = '{model.Apartments[0].HouseNR}', FlatNr = '{model.Apartments[0].FlatNr}', Floor = '{model.Apartments[0].BuildingFloors}', Broker_id = '{model.BrokerIds[0]}'
                                WHERE Id = {model.Apartments[0].Id}";
            _connection.Open();

            using var sqlCommand = new SqlCommand(command, _connection);
            sqlCommand.ExecuteNonQuery();
        }

        public void DeleteApartment(int deleteid)
        {
            string command = $@"DELETE FROM Dbo.Apartments
                                WHERE Id = {deleteid}";
            _connection.Open();
            using var sqlCommand = new SqlCommand(command, _connection);
            sqlCommand.ExecuteNonQuery();
            _connection.Close();
        }

        public List<ApartmentModel> ParseModels(string query)
        {
            List<ApartmentModel> models = new();
            List<CompanyModel> companies = _companyService.GetCompanies();
            List<BrokerModel> brokers = _brokerService.GetBrokers();
            _connection.Open();

            using var command = new SqlCommand(query, _connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                ApartmentModel apartment = new()
                {
                    Id = reader.GetInt32(0),
                    City = reader.GetString(1),
                    Street = reader.GetString(2),
                    Address = reader.GetString(3),
                    Floor = reader.GetInt32(4),
                    BuildingFloors = reader.GetInt32(5),
                    Company_id = reader.GetInt32(6),
                    Broker_id = reader.GetInt32(7),
                    HouseNR = reader.GetInt32(8),
                    FlatNr = reader.GetInt32(9)
                };

                models.Add(apartment);
            }

            foreach (var apartment in models)
            {
                apartment.Company = companies.Single(a => a.Id == apartment.Company_id);
                if (apartment.Broker_id == 0)
                {
                    continue;
                }
                else
                {
                    apartment.Broker = brokers.Single(a => a.Id == apartment.Broker_id);
                }

            }
            _connection.Close();
            return models;
        }
    }
}