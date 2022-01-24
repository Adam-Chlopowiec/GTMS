using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTMS.Models
{
    public class Group
    {
        public string Name { get; set; }

        public long Id { get; set; }

        public Person Captain { get; set; }

        public List<Person> Members { get; set; } = new List<Person>();

        public List<Tour> Tours { get; set; } = new List<Tour>();

        public List<Expense> Expenses { get; set; } = new List<Expense>();

        public double TotalValue => Tours.Sum(tour => tour.TotalExpense) + Expenses.Sum(exp => exp.TotalValue);
    }
}
