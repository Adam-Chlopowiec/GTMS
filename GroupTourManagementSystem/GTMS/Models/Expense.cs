// ***********************************************************************
// Assembly         : GTMS
// Author           : Adam
// Created          : 01-24-2022
//
// Last Modified By : Adam
// Last Modified On : 01-24-2022
// ***********************************************************************
// <copyright file="Expense.cs" company="">
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
    /// Class Expense.
    /// </summary>
    public class Expense
    {
        public string Name { get; set; }

        public long Id { get; set; }

        public double TotalValue => Payments.Values.Sum();

        public Dictionary<Person, double> Payments { get; set; } = new Dictionary<Person, double>();
    }
}
