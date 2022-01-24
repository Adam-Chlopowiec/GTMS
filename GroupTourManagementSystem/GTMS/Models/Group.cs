// ***********************************************************************
// Assembly         : GTMS
// Author           : Adam
// Created          : 01-24-2022
//
// Last Modified By : Adam
// Last Modified On : 01-24-2022
// ***********************************************************************
// <copyright file="Group.cs" company="">
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
    /// Class Group.
    /// </summary>
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
