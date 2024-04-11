using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SalesGraph.Core.Configuration;
using SalesGraph.Core.DataAccess.Context;
using SalesGraph.Core.Services.Implementation;
using SalesGraph.Core.Services.Interfaces;

namespace SalesGraph.Tests
{
    public class Startup
    {
        public IConfiguration? Configuration { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
            services.AddTransient<IConfiguration>(sp =>
            {
                IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
                configurationBuilder
                    .AddJsonFile("appsettings.json");
                
                return configurationBuilder.Build();
            });

            var serviceProvider = services.BuildServiceProvider();
            Configuration = serviceProvider.GetService<IConfiguration>();
            
            ConfigureSettings(services);
            
            var dbSettings = new DbSettings();
            Configuration?.GetSection("DB").Bind(dbSettings);

            services.AddDbContext<SalesGraphContext>(options =>
                options.UseSqlServer(dbSettings.ConnectionStrings.Api));
            
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<ISalesService, SalesService>();
        }

        private void ConfigureSettings(IServiceCollection services)
        {
            services.Configure<DbSettings>(options => { Configuration?.GetSection("DB").Bind(options); });
        }

    
    }
}
