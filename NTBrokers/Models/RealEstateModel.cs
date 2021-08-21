using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NTBrokers.Models
{
    public class RealEstateModel
    {
        public List<BrokerModel> Brokers { get; set; }
        public List<HouseModel> Houses { get; set; }
        public List<CompanyModel> Companies { get; set; }


    }
}
