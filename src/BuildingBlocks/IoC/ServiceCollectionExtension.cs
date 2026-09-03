using Microsoft.Extensions.DependencyInjection;


namespace BuildingBlocks.IoC
{
    public static class ServiceCollectionExtension
    {

        public static IServiceCollection AddDependencyResolvers(this IServiceCollection services, ICoreModule[] modules)
        {
            foreach (var module in modules)
            {
                module.Load(services);
            }

            return ServiceTool.Create(services);
        }
    }
}
