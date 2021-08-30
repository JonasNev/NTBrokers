using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NTBrokers.Models
{
    public class SortFilterModel
    {
        public string sortBy { get; set; }
        public List<string> sortBrokerSelection { get; set; } = new() { "Name", "Surname" };
        public string filterCity { get; set; }
        public int? filterCompany { get; set; }
        public int? filterBroker { get; set; }
    }
}
