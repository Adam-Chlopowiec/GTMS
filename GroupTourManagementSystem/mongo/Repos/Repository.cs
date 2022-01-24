using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace mongo.Repos
{
    public abstract class Repository<E> : IRepository<E>
    {
        protected MongoClient Client;
        protected IMongoDatabase Database;
        protected IMongoCollection<BsonDocument> Collection;

        protected Repository()
        {
        }

        public abstract Task<E> Add(E item);

        public abstract Task<bool> Delete(Guid id);

        public abstract Task<bool> Edit(E item);

        public abstract Task<IEnumerable<E>> GetAll();

        public abstract Task<E> GetById(Guid id);

        public void Connect(string collection)
        {
            Client = new MongoClient("mongodb://127.0.0.1:27017/?directConnection=true&serverSelectionTimeoutMS=2000&appName=mongosh+1.1.9");
            Database = Client.GetDatabase("GTMS");
            Collection = Database.GetCollection<BsonDocument>(collection);
        }
    }
}
