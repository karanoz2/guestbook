using Microsoft.Extensions.DependencyInjection;
using test.book.DAL.Postgras;
using test.book.DAL;
using test.book.dal.Postgras;
using test.book.dal.health;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace test.book.dal
{
    public static class Module
    {
        public static void AddDAL(this IServiceCollection services) {
            services.AddTransient<IMessageManager, DataContextOfPostgrass>();
            services.AddTransient<IDatabaseInit, DataContextOfPostgrass>();
            services.AddTransient<IDbHealthCheck, PostgrasDbHealthCheck>();
            services.AddTransient<IHealthCheck, DbHealthCheck>();            
        }

        public static async Task InitDB(this IServiceProvider serviceProvider) {
            {
                using var scope = serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<IDatabaseInit>();
                await context.Init();
            }
        }
    }
}
