using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mongo.Models.DBModels;
using MongoDB.Bson;

namespace mongo.DBModels
{
    public class ExpenseDB : IDBModel
    {
        public string Name { get; set; }

        public Guid Id { get; set; }

        public Dictionary<Guid, double> Payments { get; set; } = new Dictionary<Guid, double>();

        public ExpenseDB()
        {

        }

        public ExpenseDB(string name, Guid id, Dictionary<Models.AppModels.Person, double> payments)
        {
            Name = name;
            Id = id;
            ConvertPayments(payments);
        }

        private void ConvertPayments(Dictionary<Models.AppModels.Person, double> payments)
        {
            foreach (var kvp in payments)
            {
                var person = kvp.Key;
                var payment = kvp.Value;
                Payments.Add(person.Id, payment);
            }
        }

        public BsonDocument ToBson()
        {
            return new BsonDocument
                    {
                        {"name", Name},
                        {"_id", BsonBinaryData.Create(Id)},
                        {"members", new BsonArray(Payments.Keys)},
                        {"expenses", new BsonArray(Payments.Values)}
                    };
        }

        public static ExpenseDB FromBson(BsonDocument bson)
        {
            var expense = new ExpenseDB();
            expense.Id = (Guid)bson["_id"];
            expense.Name = (string)bson["name"];
            var members = bson["members"].AsBsonArray.ToList();
            var expenses = bson["expenses"].AsBsonArray.ToList();
            for(var i = 0; i < members.Count; i++)
            {
                expense.Payments.Add((Guid)members[i], (double)expenses[i]);
            }

            return expense;
        }
    }
}
