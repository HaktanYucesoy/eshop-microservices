using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.IoC
{
    public interface ICoreModule
    {
        void Load(IServiceCollection services);
    }
}
