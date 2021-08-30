using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NTBrokers.Models
{
    public class RealEstateModel
    {
        public List<BrokerModel> Brokers { get; set; }
        public List<ApartmentModel> Apartments { get; set; }
        public List<CompanyModel> Companies { get; set; }
        public List<int> BrokerIds { get; set; }
        public List<string> Cities { get; set; }
        public SortFilterModel SortFilter { get; set; }
    }
}
