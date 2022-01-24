// ***********************************************************************
// Assembly         : GTMS
// Author           : Adam
// Created          : 01-24-2022
//
// Last Modified By : Adam
// Last Modified On : 01-24-2022
// ***********************************************************************
// <copyright file="Tour.cs" company="">
//     Copyright ©  2022
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTMS.Models
{
    /// <summary>
    /// Class Tour.
    /// </summary>
    public class Tour
    {
        public string Name { get; set; }

        public long Id { get; set; }

        public List<Expense> Expenses { get; set; } = new List<Expense>();

        public double TotalExpense => Expenses.Sum(exp => exp.TotalValue);

        public List<Person> Members { get; set; } = new List<Person>();

        public Person Captain { get; set; }
    }
}
