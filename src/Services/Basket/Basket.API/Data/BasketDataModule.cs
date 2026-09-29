using BuildingBlocks.IoC;

namespace Basket.API.Data
{
    public class BasketDataModule : ICoreModule
    {
        public void Load(IServiceCollection services)
        {
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.Decorate<IBasketRepository, CachedBasketRepository>();
        }
    }
}
