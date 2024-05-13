using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RentAMotorcycleRabbitMQ.Config;
using RentAMotorcycleRabbitMQ.Interface;
using System.Threading.Tasks;

namespace RentAMotorcycleRabbitMQ
{
    class Program
    {
        public IConfiguration Configuration { get; }

        private static IServiceCollection ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();
            return services.ResolveDependencies();
        }

        public static async Task Main()
        {
            var services = ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();
            await serviceProvider.GetService<IStartProcess>().Init();
        }
    }
}
