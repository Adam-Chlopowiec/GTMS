using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mongo.Models.AppModels;
using MongoDB.Bson;
using MongoDB.Driver;

namespace mongo.Repos
{
    public class PersonRepository : Repository<Person>
    {
        private GroupRepository groupRepository;

        public PersonRepository()
        {
        }

        public override async Task<Person> Add(Person item)
        {
            if (item.Id == default)
                item.Id = Guid.NewGuid();
            var dbGroup = new DBModels.PersonDB(item.Name, item.Lastname, item.Id, item.Groups);
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

        public override async Task<bool> Edit(Person item)
        {
            var dbPerson = new DBModels.PersonDB(item.Name, item.Lastname, item.Id, item.Groups);
            var filter = Builders<BsonDocument>.Filter.Eq("_id", dbPerson.Id);
            var update = Builders<BsonDocument>.Update.Set("name", dbPerson.Name)
                .Set("lastname", dbPerson.Lastname)
                .Set("groups", new BsonArray(dbPerson.Groups));

            var result = await Collection.UpdateOneAsync(filter, update);
            if (result.MatchedCount == 1 && result.ModifiedCount == 1)
                return true;
            else
                return false;
        }

        public override async Task<IEnumerable<Person>> GetAll()
        {
            var filter = Builders<BsonDocument>.Filter.Empty;
            var bsons = await Collection.FindAsync(filter);
            var items = await bsons.ToListAsync();
            var groups = new List<Person>();
            foreach (var bson in items)
            {
                groups.Add(await FromBson(bson));
            }

            return groups;
        }

        public override async Task<Person> GetById(Guid id)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
            var bson = await Collection.FindAsync(filter);
            var document = bson.ToList()[0];
            return await FromBson(document);
        }

        private async Task<Person> FromBson(BsonDocument bson)
        {
            var person = DBModels.PersonDB.FromBson(bson);
            var groups = new List<Group>();
            foreach (var id in person.Groups)
            {
                var group = await groupRepository.GetById(id);
                groups.Add(group);
            }

            var appPerson = new Person
            {
                Name = person.Name,
                Id = person.Id,
                Lastname = person.Lastname,
                Groups = groups
            };

            return appPerson;
        }

        public void SetGroupRepository(GroupRepository _groupRepository)
        {
            groupRepository = _groupRepository;
        }
    }
}
