using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mongo.Models.AppModels;
using MongoDB.Bson;
using MongoDB.Driver;

namespace mongo.Repos
{
    public class ExpenseRepository : Repository<Expense>
    {
        private PersonRepository personRepository;

        public ExpenseRepository()
        {
        }

        /// <summary>Adds the specified item.</summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public override async Task<Expense> Add(Expense item)
        {
            if (item.Id == default)
                item.Id = Guid.NewGuid();
            var dbExpense = new DBModels.ExpenseDB(item.Name, item.Id, item.Payments);
            await Collection.InsertOneAsync(dbExpense.ToBson());
            return item;
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public override async Task<bool> Delete(Guid id)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
            
            var result = await Collection.DeleteOneAsync(filter);
            if (result.DeletedCount == 1)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Edits the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public override async Task<bool> Edit(Expense item)
        {
            var dbExpense = new DBModels.ExpenseDB(item.Name, item.Id, item.Payments);
            var filter = Builders<BsonDocument>.Filter.Eq("_id", dbExpense.Id);
            var update = Builders<BsonDocument>.Update.Set("name", dbExpense.Name).Set("members", new BsonArray(dbExpense.Payments.Keys))
                .Set("expenses", dbExpense.Payments.Values);

            var result = await Collection.UpdateOneAsync(filter, update);
            if (result.MatchedCount == 1 && result.ModifiedCount == 1)
                return true;
            else
                return false;
        }

        public override async Task<IEnumerable<Expense>> GetAll()
        {
            var filter = Builders<BsonDocument>.Filter.Empty;
            var bsons = await Collection.FindAsync(filter);
            var items = await bsons.ToListAsync();
            var expenses = new List<Expense>();
            foreach (var bson in items)
            {
                expenses.Add(await FromBson(bson));
            }

            return expenses;
        }

        public override async Task<Expense> GetById(Guid id)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
            var bson = await Collection.FindAsync(filter);
            var document = bson.ToList()[0];
            return await FromBson(document);
        }

        private async Task<Expense> FromBson(BsonDocument bson)
        {
            var expense = DBModels.ExpenseDB.FromBson(bson);
            var payments = new Dictionary<Person, double>();
            foreach (var memberId in expense.Payments.Keys)
            {
                var person = await personRepository.GetById(memberId);
                payments.Add(person, expense.Payments[memberId]);
            }

            var appExpense = new Expense
            {
                Name = expense.Name,
                Id = expense.Id,
                Payments = payments
            };
            return appExpense;
        }

        public void SetPersonRepository(PersonRepository _personRepository)
        {
            personRepository = _personRepository;
        }

    }
}
