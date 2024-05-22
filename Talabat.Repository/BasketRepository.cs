using StackExchange.Redis;
using System.Text.Json;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;

namespace Talabat.Infrastructure
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _dataBase;
        public BasketRepository(IConnectionMultiplexer redis)
        {
            _dataBase = redis.GetDatabase();
        }
        public async Task<bool> DeleteBasketAsync(string BasketId)
        {
            return await _dataBase.KeyDeleteAsync(BasketId);
        }

        public async Task<CustomerBasket?> GetBasketAsync(string basketId)
        {
            var basket = await _dataBase.StringGetAsync(basketId);
            return basket.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(basket);
        }

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
        {
            var CreatedOrUpdated = await _dataBase.StringSetAsync(basket.Id,JsonSerializer.Serialize(basket),TimeSpan.FromDays(30));
            if (CreatedOrUpdated is false) return null;
            return await GetBasketAsync(basket.Id);
        }
    }
}
