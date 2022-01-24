using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mongo.Models.DBModels;
using MongoDB.Bson;

namespace mongo.DBModels
{
    public class GroupDB : IDBModel
    {
        public string Name { get; set; }

        public Guid Id { get; set; }

        public Guid Captain { get; set; }

        public List<Guid> Members { get; set; } = new List<Guid>();

        public List<Guid> Tours { get; set; } = new List<Guid>();

        public List<Guid> Expenses { get; set; } = new List<Guid>();

        public GroupDB()
        {

        }

        public GroupDB(string name, Guid id, Models.AppModels.Person captain, List<Models.AppModels.Person> members, 
            List<Models.AppModels.Tour> tours, List<Models.AppModels.Expense> expenses)
        {
            Name = name;
            Id = id;
            Captain = captain.Id;
            Members = ObjectsToId(members);
            Tours = ObjectsToId(tours);
            Expenses = ObjectsToId(expenses);
        }

        private List<Guid> ObjectsToId(List<Models.AppModels.Person> objects)
        {
            var ids = new List<Guid>();
            foreach(var obj in objects)
                ids.Add(obj.Id);
            return ids;
        }

        private List<Guid> ObjectsToId(List<Models.AppModels.Tour> objects)
        {
            var ids = new List<Guid>();
            foreach (var obj in objects)
                ids.Add(obj.Id);
            return ids;
        }

        private List<Guid> ObjectsToId(List<Models.AppModels.Expense> objects)
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
                {"captain", BsonBinaryData.Create(Captain)},
                {"members", new BsonArray(Members)},
                {"expenses", new BsonArray(Expenses)},
                {"tours", new BsonArray(Tours)}
            };
        }

        public static GroupDB FromBson(BsonDocument bson)
        {
            var group = new GroupDB();
            group.Id = (Guid)bson["_id"];
            group.Name = (string)bson["name"];
            group.Captain = (Guid)bson["captain"];
            var members = bson["members"].AsBsonArray.ToList();
            var expenses = bson["expenses"].AsBsonArray.ToList();
            var tours = bson["tours"].AsBsonArray.ToList();
            foreach (var member in members)
                group.Members.Add((Guid)member);

            foreach (var expense in expenses)
                group.Expenses.Add((Guid)expense);

            foreach (var tour in tours)
                group.Tours.Add((Guid)tour);

            return group;
        }
    }
}
