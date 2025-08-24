using Abdm.Calculation.ColumnCalculation;
using Abdm.Calculation.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Abdm.Calculation.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MainDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("MainConnection")));

            services.AddScoped<IPassageIntervalRepository, PassageIntervalRepository>();
            services.AddScoped<IPassTypeCalculator, PassTypeCalculator>();
        }
    }
}
