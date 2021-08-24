﻿using System;
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

        public ApartmentService(SqlConnection connection)
        {
            _connection = connection;
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

        public List<ApartmentModel> GetApartments()
        {
            List<ApartmentModel> apartments = new();

            _connection.Open();

            using var command = new SqlCommand("SELECT * FROM dbo.Apartments;", _connection);
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

                apartments.Add(apartment);
            }

            _connection.Close();

            return apartments;
        }
    }
}