using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mongo.Models.AppModels
{
    public class Person
    {
        public string Name { get; set; }

        public string Lastname { get; set; }

        public Guid Id { get; set; }

        public List<Group> Groups { get; set; } = new List<Group>();
    }
}
