using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NTBrokers.Models
{
    public class ApartmentModel
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Address { get; set; }
        public int Floor { get; set; }
        public int HouseNR { get; set; }
        public int FlatNr { get; set; }
        public int BuildingFloors { get; set; }
        public BrokerModel Broker { get; set; }
        public int? Broker_id { get; set; }
        public CompanyModel Company { get; set; }
        public int Company_id { get; set; }

    }
}
