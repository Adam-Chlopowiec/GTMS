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
    public class TourRepository : Repository<Tour>
    {
        private PersonRepository personRepository;
        private ExpenseRepository expenseRepository;

        public TourRepository()
        {
        }

        public override async Task<Tour> Add(Tour item)
        {
            if (item.Id == default)
                item.Id = Guid.NewGuid();
            var dbTour = new DBModels.TourDB(item.Name, item.Id, item.Captain, item.Expenses, item.Members);
            await Collection.InsertOneAsync(dbTour.ToBson());
            return item;
        }

        public override async Task<bool> Delete(Guid id)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);

            var result = await Collection.DeleteOneAsync(filter);
            if (result.DeletedCount == 1)
                return true;
            else
                return false;
        }

        public override async Task<bool> Edit(Tour item)
        {
            var dbTour = new DBModels.TourDB(item.Name, item.Id, item.Captain, item.Expenses, item.Members);
            var filter = Builders<BsonDocument>.Filter.Eq("_id", dbTour.Id);
            var update = Builders<BsonDocument>.Update.Set("name", dbTour.Name)
                .Set("captain", dbTour.Captain)
                .Set("members", new BsonArray(dbTour.Members))
                .Set("expenses", dbTour.Expenses);

            var result = await Collection.UpdateOneAsync(filter, update);
            if (result.MatchedCount == 1 && result.ModifiedCount == 1)
                return true;
            else
                return false;
        }

        public override async Task<IEnumerable<Tour>> GetAll()
        {
            var filter = Builders<BsonDocument>.Filter.Empty;
            var bsons = await Collection.FindAsync(filter);
            var items = await bsons.ToListAsync();
            var groups = new List<Tour>();
            foreach (var bson in items)
            {
                groups.Add(await FromBson(bson));
            }

            return groups;
        }

        public override async Task<Tour> GetById(Guid id)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
            var bson = await Collection.FindAsync(filter);
            var document = bson.ToList()[0];
            return await FromBson(document);
        }

        private async Task<Tour> FromBson(BsonDocument bson)
        {
            var tour = DBModels.TourDB.FromBson(bson);
            var captain = await personRepository.GetById(tour.Captain);
            var members = new List<Person>();
            var expenses = new List<Expense>();
            foreach (var id in tour.Members)
            {
                var member = await personRepository.GetById(id);
                members.Add(member);
            }

            foreach (var id in tour.Expenses)
            {
                var expense = await expenseRepository.GetById(id);
                expenses.Add(expense);
            }

            var appTour = new Tour
            {
                Name = tour.Name,
                Id = tour.Id,
                Captain = captain,
                Expenses = expenses,
                Members = members
            };

            return appTour;
        }

        public void SetPersonRepository(PersonRepository _personRepository)
        {
            personRepository = _personRepository;
        }

        public void SetExpenseRepository(ExpenseRepository _expenseRepository)
        {
            expenseRepository = _expenseRepository;
        }
    }
}
