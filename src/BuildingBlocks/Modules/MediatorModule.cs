using BuildingBlocks.Behaviors;
using BuildingBlocks.IoC;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BuildingBlocks.Dependencies
{
    public class MediatorModule : ICoreModule
    {
        public MediatorModule(Assembly _assembly)
        {
            assembly = _assembly;
        }

        public Assembly assembly { get; set; }


        public void Load(IServiceCollection services)
        {
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(assembly);
                config.AddOpenBehavior(typeof(ValidationBehavior<,>));
                config.AddOpenBehavior(typeof(LoggingBehavior<,>));
            });
        }
    }
}
