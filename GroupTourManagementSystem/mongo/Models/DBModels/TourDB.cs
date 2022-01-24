using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mongo.Models.DBModels;
using MongoDB.Bson;

namespace mongo.DBModels
{
    public class TourDB : IDBModel
    {
        public string Name { get; set; }

        public Guid Id { get; set; }

        public Guid Captain { get; set; }

        public List<Guid> Expenses { get; set; } = new List<Guid>();

        public List<Guid> Members { get; set; } = new List<Guid>();

        public TourDB()
        {

        }

        public TourDB(string name, Guid id, Models.AppModels.Person captain, List<Models.AppModels.Expense> expenses, List<Models.AppModels.Person> members)
        {
            Name = name;
            Id = id;
            Captain = captain.Id;
            Expenses = ObjectsToId(expenses);
            Members = ObjectsToId(members);
        }

        private List<Guid> ObjectsToId(List<Models.AppModels.Expense> objects)
        {
            var ids = new List<Guid>();
            foreach (var obj in objects)
                ids.Add(obj.Id);
            return ids;
        }

        private List<Guid> ObjectsToId(List<Models.AppModels.Person> objects)
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
                {"expenses", new BsonArray(Expenses)},
                {"members", new BsonArray(Members)}
            };
        }

        public static TourDB FromBson(BsonDocument bson)
        {
            var tour = new TourDB();
            tour.Id = (Guid)bson["_id"];
            tour.Name = (string)bson["name"];
            tour.Captain = (Guid)bson["captain"];
            var expenses = bson["groups"].AsBsonArray.ToList();
            var members = bson["groups"].AsBsonArray.ToList();
            foreach (var expense in expenses)
                tour.Expenses.Add((Guid)expense);

            foreach (var member in members)
                tour.Members.Add((Guid)member);

            return tour;
        }
    }
}
