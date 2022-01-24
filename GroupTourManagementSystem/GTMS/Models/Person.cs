using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTMS.Models
{
    public class Person
    {
        public string Name { get; set; }

        public string Lastname { get; set; }

        public long Id { get; set; }

        public List<Group> Groups { get; set; } = new List<Group>();
    }
}
