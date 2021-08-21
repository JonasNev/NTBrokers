using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NTBrokers.Models
{
    public class HouseModel
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Adress { get; set; }
        public int Floor { get; set; }
        public int BuildingFloors { get; set; }
        public int Broker_id { get; set; }
        public int Company_id { get; set; }

    }
}
