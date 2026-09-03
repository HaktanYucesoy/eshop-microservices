using BuildingBlocks.IoC;
using Carter;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace BuildingBlocks.Modules
{
    public class CarterDependencyModule : ICoreModule
    {
        public CarterDependencyModule(Assembly assembly)
        {
            this.assembly = assembly;
        }

        public Assembly assembly {  get;  set; }


        public void Load(IServiceCollection services)
        {
            var carterModules = assembly.GetTypes()
                .Where(t => typeof(ICarterModule).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface)
                .ToArray();
            
            services.AddCarter();
  
            foreach (var module in carterModules)
            {
                services.AddTransient(typeof(ICarterModule), module);
            }
        }
    }
}
