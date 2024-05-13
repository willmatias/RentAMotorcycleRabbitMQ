using MongoDB.Driver;
using System.Threading.Tasks;

namespace RentAMotorcycleRabbitMQ.Interface
{
    public interface IMongoRepository<T> where T : class
    {
        Task<UpdateResult> UpdateOne(FilterDefinition<T> filter, UpdateDefinition<T> update, UpdateOptions options);
        Task<T> InsertOne(T document);
    }
}
