using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mongo.Models.DBModels;
using MongoDB.Bson;

namespace mongo.DBModels
{
    public class PersonDB :IDBModel
    {
        public string Name { get; set; }

        public string Lastname { get; set; }

        public Guid Id { get; set; }

        public List<Guid> Groups { get; set; } = new List<Guid>();

        public PersonDB()
        {

        }

        public PersonDB(string name, string lastname, Guid id, List<Models.AppModels.Group> groups)
        {
            Name = name;
            Lastname = lastname;
            Id = id;
            Groups = ObjectsToId(groups);
        }

        private List<Guid> ObjectsToId(List<Models.AppModels.Group> objects)
        {
            var ids = new List<Guid>();
            foreach (var obj in objects)
                ids.Add(obj.Id);
            return ids;
        }

        public BsonDocument ToBson()
        {
            return new BsonDocument
            {
                {"name", Name},
                {"_id", BsonBinaryData.Create(Id)},
                {"lastname", Lastname},
                {"groups", new BsonArray(Groups)}
            };
        }

        public static PersonDB FromBson(BsonDocument bson)
        {
            var person = new PersonDB();
            person.Id = (Guid)bson["_id"];
            person.Name = (string)bson["name"];
            person.Lastname = (string)bson["lastname"];
            var groups = bson["groups"].AsBsonArray.ToList();
            foreach (var group in groups)
                person.Groups.Add((Guid)group);

            return person;
        }
    }
}
