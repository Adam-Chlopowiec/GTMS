using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTMS.Models
{
    public class Expense
    {
        public string Name { get; set; }

        public long Id { get; set; }

        public double TotalValue => Payments.Values.Sum();

        public Dictionary<Person, double> Payments { get; set; } = new Dictionary<Person, double>();
    }
}
