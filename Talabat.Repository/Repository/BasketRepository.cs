using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.RepositoryInterfaces;

namespace Talabat.Repository.Repository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _dataBase;
        public BasketRepository(IConnectionMultiplexer redis) {
            _dataBase = redis.GetDatabase();
        }
        public async Task<CustomerBasket?> GetBasketAsync(string basketId)
        {
            var Basket= await _dataBase.StringGetAsync(basketId);
            return Basket.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(Basket);
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await _dataBase.KeyDeleteAsync(basketId);
        }

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
        {
            var CreateOrDelete = await _dataBase.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(30));
            if(CreateOrDelete is false)return null;
            return await GetBasketAsync(basket.Id);
        }
    }
}
