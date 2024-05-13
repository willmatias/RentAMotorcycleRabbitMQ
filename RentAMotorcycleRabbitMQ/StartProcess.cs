using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RentAMotorcycleRabbitMQ.Interface;
using RentAMotorcycleRabbitMQ.Models;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RentAMotorcycleRabbitMQ
{
    public class StartProcess : IStartProcess
    {
        private readonly IRabbitSettings _rabbitSettings;
        private readonly IMongoRepository<Motors> _mongoRepository;
        public StartProcess(IRabbitSettings rabbitSettings, IMongoRepository<Motors> mongoRepository)
        {
            _rabbitSettings = rabbitSettings;
            _mongoRepository = mongoRepository;

        }
        public async Task Init()
        {
            ConnectionFactory factory = new ConnectionFactory()
            {
                HostName = _rabbitSettings.HostName,
                Port = _rabbitSettings.Port,
                UserName = _rabbitSettings.UserName,
                Password = _rabbitSettings.Password
            };

            IConnection connection = factory.CreateConnection();
            IModel channel;
            channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange: _rabbitSettings.ExchangeName, type: ExchangeType.Topic, true);


            CreateEventingBasicConsumer(channel);

        }

        private void CreateEventingBasicConsumer(IModel channel)
        {
            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                string message = Encoding.UTF8.GetString(ea.Body.ToArray());

                Motors motors = Newtonsoft.Json.JsonConvert.DeserializeObject<Motors>(message);

                await _mongoRepository.InsertOne(motors).ConfigureAwait(false);                

                channel.BasicAck(ea.DeliveryTag, false);
            };
            while (true)
            {
                Thread.Sleep(5000);
                var queueName = channel.QueueDeclare(_rabbitSettings.QueueName, true, false, false).QueueName;

                channel.QueueBind(queue: queueName,
                                        exchange: _rabbitSettings.ExchangeName,
                                        routingKey: queueName);

                channel.BasicConsume(queue: queueName,
                                    autoAck: false,
                                    consumer: consumer);
            }
        }
    }
}
