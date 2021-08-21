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

        public RealEstateService(SqlConnection connection,BrokerService brokerService, CompanyService companyService)
        {
            _connection = connection;
            _companyService = companyService;
            _brokerService = brokerService;
        }

        public RealEstateModel GetModelForCompanyCreate()
        {
            RealEstateModel model = new RealEstateModel();
            model.Brokers = _brokerService.GetBrokers();

            return model;
        }

        public RealEstateModel GetModelForIndex()
        {
            RealEstateModel model = new RealEstateModel();
            model.Brokers = _brokerService.GetBrokers();
            model.Companies = _companyService.GetCompanies();
            return model;
        }
    }
}
