using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using NTBrokers.Models;

namespace NTBrokers.Services
{
    public class RealEstateService
    {
        private SqlConnection _connection;
        private BrokerService _brokerService;
        private CompanyService _companyService;
        private ApartmentService _apartmentService;

        public RealEstateService(SqlConnection connection,BrokerService brokerService, CompanyService companyService, ApartmentService apartmentService)
        {
            _connection = connection;
            _companyService = companyService;
            _brokerService = brokerService;
            _apartmentService = apartmentService;
        }

        public RealEstateModel GetModelForCompanyCreate()
        {
            RealEstateModel model = new RealEstateModel();
            model.Brokers = _brokerService.GetBrokers();

            return model;
        }

        public RealEstateModel GetModelForApartmentCreate()
        {
            RealEstateModel model = new RealEstateModel();
            model.Brokers = _brokerService.GetBrokers();
            model.Companies = _companyService.GetCompanies();
            model.Cities = GetCompanyCities();

            return model;

        }

        public RealEstateModel GetModelForIndex()
        {
            RealEstateModel model = new RealEstateModel();
            model.Brokers = _brokerService.GetBrokers();
            model.Companies = _companyService.GetCompanies();
            model.Apartments = _apartmentService.GetApartments();
            return model;
        }

        public RealEstateModel GetModelForEdit(int editId)
        {
            RealEstateModel model = new RealEstateModel();

            List<CompanyModel> companies = _companyService.GetCompanies();
            CompanyModel company = companies.SingleOrDefault(x => x.Id == editId);

            model.Companies = new List<CompanyModel>() {company};
            model.Brokers = _brokerService.GetBrokers();
            model.BrokerIds = GetBrokerIds(editId);
            foreach (var item in model.Brokers)
            {
                if (model.BrokerIds.Contains(item.Id))
                {
                    company.BrokersEdit.Add(item);
                }
            }

            return model;
        }

        public List<int> GetBrokerIds(int companyId)
        {
            List<int> brokerIds = new List<int>();
            string command = $"SELECT Broker_id FROM dbo.CompaniesBrokers WHERE Company_id = {companyId}";
            _connection.Open();

            using var sqlCommand = new SqlCommand(command, _connection);
            using var reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                brokerIds.Add(reader.GetInt32(0));
            }

            _connection.Close();

            return brokerIds;
        }

        public List<string> GetCompanyCities()
        {
            List<string> cities = new List<string>();
            _connection.Open();
            using var command = new SqlCommand("SELECT City FROM dbo.Companies", _connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                cities.Add(reader.GetString(0));
            }

            _connection.Close();

            return cities;
        }
    }
}
