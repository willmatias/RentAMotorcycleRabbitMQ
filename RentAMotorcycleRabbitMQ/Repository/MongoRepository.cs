using MongoDB.Driver;
using RentAMotorcycleRabbitMQ.Interface;
using System.Threading.Tasks;

namespace RentAMotorcycleRabbitMQ.Repository
{
    public class MongoRepository<T> : IMongoRepository<T> where T : class
    {
        private readonly IMongoCollection<T> _collection;
        public async Task<UpdateResult> UpdateOne(FilterDefinition<T> filter, UpdateDefinition<T> update, UpdateOptions options)
        {
            return await _collection.UpdateOneAsync(filter, update, options)
                .ConfigureAwait(false);
        }

        public async Task<T> InsertOne(T document)
        {
            await _collection.InsertOneAsync(document);

            return document;
        }
    }
}
