using System;

namespace RentAMotorcycleRabbitMQ.Utils
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class BsonConnectionAttribute : Attribute
    {
        private string _collection;
        private int _clusterId;
        private string _database;

        public BsonConnectionAttribute(int clusterId, string database, string collection)
        {
            _collection = collection;
            _clusterId = clusterId;
            _database = database;
        }

        public string Collection => _collection;
        public int ClusterId => _clusterId;
        public string Database => _database;
    }
}
