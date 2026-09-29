using Basket.API.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.API.Data
{
    public class CachedBasketRepository(IBasketRepository repository,IDistributedCache cache) : IBasketRepository
    {
        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
        {
            var deletedResult= await repository.DeleteBasket(userName, cancellationToken);
            await cache.RemoveAsync(userName, cancellationToken);

            return deletedResult;
        }

        public async Task<ShoppingCart> GetBasket(string Username, CancellationToken cancellationToken = default)
        {
            var cachedBasket=await cache.GetStringAsync(Username, cancellationToken);
            if (!String.IsNullOrEmpty(cachedBasket))
                return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;

            var basket=await repository.GetBasket(Username, cancellationToken);
            await cache.SetStringAsync(Username,JsonSerializer.Serialize(basket),cancellationToken);
            return basket;

        }

        public async Task<ShoppingCart> StoreBasket(ShoppingCart shoppingCart, CancellationToken cancellationToken = default)
        {
            var addedNewBasket=await repository.StoreBasket(shoppingCart, cancellationToken);
            await cache.SetStringAsync(addedNewBasket.UserName,JsonSerializer.Serialize(addedNewBasket),cancellationToken);

            return addedNewBasket;
        }
    }
}
