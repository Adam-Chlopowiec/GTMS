// ***********************************************************************
// Assembly         : GTMS
// Author           : Adam
// Created          : 01-24-2022
//
// Last Modified By : Adam
// Last Modified On : 01-24-2022
// ***********************************************************************
// <copyright file="Person.cs" company="">
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
    /// Class Person.
    /// </summary>
    public class Person
    {
        public string Name { get; set; }

        public string Lastname { get; set; }

        public long Id { get; set; }

        public List<Group> Groups { get; set; } = new List<Group>();
    }
}
