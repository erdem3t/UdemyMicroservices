using FreeCourse.Services.Basket.Settings;
using StackExchange.Redis;

namespace FreeCourse.Services.Basket.Redis
{
    public class RedisService : IRedisService
    {
        private readonly string host;

        private readonly int port;

        private ConnectionMultiplexer connectionMultiplexer;

        public RedisService(IRedisSettings redisSettings)
        {
            host = redisSettings.Host;
            port = redisSettings.Port;
        }

        public void Connect() => connectionMultiplexer = ConnectionMultiplexer.Connect($"{host}:{port}");

        public IDatabase GetDatabase(int db = 1) => connectionMultiplexer.GetDatabase(db);
    }
}
