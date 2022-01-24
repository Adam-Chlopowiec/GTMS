using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mongo.Models.AppModels
{
    public class Expense
    {
        public string Name { get; set; }

        public Guid Id { get; set; }

        public double TotalValue => Payments.Values.Sum();

        public Dictionary<Person, double> Payments { get; set; } = new Dictionary<Person, double>();
    }
}
