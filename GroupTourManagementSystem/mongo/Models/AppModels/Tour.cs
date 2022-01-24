using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mongo.Models.AppModels
{
    public class Tour
    {
        public string Name { get; set; }

        public Guid Id { get; set; }

        public List<Expense> Expenses { get; set; } = new List<Expense>();

        public double TotalExpense => Expenses.Sum(exp => exp.TotalValue);

        public List<Person> Members { get; set; } = new List<Person>();

        public Person Captain { get; set; }
    }
}
