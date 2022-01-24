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
    public class GroupRepository : Repository<Group>
    {
        private PersonRepository personRepository;
        private TourRepository tourRepository;
        private ExpenseRepository expenseRepository;

        public GroupRepository()
        {
        }

        public override async Task<Group> Add(Group item)
        {
            if (item.Id == default)
                item.Id = Guid.NewGuid();
            var dbGroup = new DBModels.GroupDB(item.Name, item.Id, item.Captain, item.Members, item.Tours,item.Expenses);
            await Collection.InsertOneAsync(dbGroup.ToBson());
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

        public override async Task<bool> Edit(Group item)
        {
            var dbGroup = new DBModels.GroupDB(item.Name, item.Id, item.Captain, item.Members, item.Tours, item.Expenses);
            var filter = Builders<BsonDocument>.Filter.Eq("_id", dbGroup.Id);
            var update = Builders<BsonDocument>.Update.Set("name", dbGroup.Name)
                .Set("captain", dbGroup.Captain)
                .Set("members", new BsonArray(dbGroup.Members))
                .Set("tours", dbGroup.Tours)
                .Set("expenses", dbGroup.Expenses);

            var result = await Collection.UpdateOneAsync(filter, update);
            if (result.MatchedCount == 1 && result.ModifiedCount == 1)
                return true;
            else
                return false;
        }

        public override async Task<IEnumerable<Group>> GetAll()
        {
            var filter = Builders<BsonDocument>.Filter.Empty;
            var bsons = await Collection.FindAsync(filter);
            var items = await bsons.ToListAsync();
            var groups = new List<Group>();
            foreach (var bson in items)
            {
                groups.Add(await FromBson(bson));
            }

            return groups;
        }

        public override async Task<Group> GetById(Guid id)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
            var bson = await Collection.FindAsync(filter);
            var document = bson.ToList()[0];
            return await FromBson(document);
        }

        private async Task<Group> FromBson(BsonDocument bson)
        {
            var group = DBModels.GroupDB.FromBson(bson);
            var captain = await personRepository.GetById(group.Captain);
            var members = new List<Person>();
            var tours = new List<Tour>();
            var expenses = new List<Expense>();
            foreach (var id in group.Members)
            {
                var member = await personRepository.GetById(id);
                members.Add(member);
            }

            foreach (var id in group.Tours)
            {
                var tour = await tourRepository.GetById(id);
                tours.Add(tour);
            }

            foreach (var id in group.Expenses)
            {
                var expense = await expenseRepository.GetById(id);
                expenses.Add(expense);
            }

            var appGroup = new Group
            {
                Name = group.Name,
                Id = group.Id,
                Captain = captain,
                Expenses = expenses,
                Members = members,
                Tours = tours
            };
            return appGroup;
        }

        public void SetPersonRepository(PersonRepository _personRepository)
        {
            personRepository = _personRepository;
        }

        public void SetTourRepository(TourRepository _tourRepository)
        {
            tourRepository = _tourRepository;
        }

        public void SetExpenseRepository(ExpenseRepository _expenseRepository)
        {
            expenseRepository = _expenseRepository;
        }
    }
}
