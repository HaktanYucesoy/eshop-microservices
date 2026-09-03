using Basket.API.Models;

namespace Basket.API.Data
{
    public interface IBasketRepository
    {
        Task<ShoppingCart> GetBasket(string Username,CancellationToken cancellationToken= default);

        Task<ShoppingCart> StoreBasket(ShoppingCart shoppingCart,CancellationToken cancellationToken= default);

        Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default);
    }
}
