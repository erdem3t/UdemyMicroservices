using StackExchange.Redis;

namespace FreeCourse.Services.Basket.Redis
{
    public interface IRedisService
    {
        void Connect();

        IDatabase GetDatabase(int db = 1);
    }
}
