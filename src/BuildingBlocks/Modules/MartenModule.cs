using BuildingBlocks.IoC;
using Marten;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Modules
{
    public class MartenModule : ICoreModule
    {
        public MartenModule(Action<StoreOptions> storeOptions)
        {
           
            _storeOptions = storeOptions;
          
        }

        public Action<StoreOptions> _storeOptions { get; set; }
     
        public void Load(IServiceCollection services)
        {


            services.AddMarten(_storeOptions).UseLightweightSessions();
        }
    }
}
